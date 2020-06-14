using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using DungeonTools.Encryption;
using DungeonTools.SaveFiles.Helpers;

namespace DungeonTools.Cli {
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Program {
        private static readonly IEncryptionService EncryptionService = EncryptionServices.Default;

        private static async Task Main(string[] args) {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            await Parser.Default.ParseArguments<RawOptions>(args).WithParsedAsync(RawMain);
        }

        private static async Task RawMain(RawOptions options) {
            foreach(string filePath in options.Input) {
                await TryProcessFile(new FileInfo(filePath), options.Overwrite);
            }
        }

        private static async ValueTask TryProcessFile(FileInfo file, bool overwrite) {
            if(!file.Exists) {
                await Console.Error.WriteLineAsync($"[  ERROR  ] File \"{file.FullName}\" could not be found and has been skipped.");
                return;
            }

            await using FileStream inputStream = file.OpenRead();
            DataType inputType = SaveDataHelper.GetDataType(inputStream);
            if(inputType == DataType.Unsupported) {
                Console.WriteLine($"[  ERROR  ] File \"{file.Name}\" could not be identified as either JSON data or encrypted data.");
                return;
            }

            await using Stream? processed = inputType == DataType.UnsafeEncrypted ? await Extract(inputStream) : await Combine(inputStream);
            if(processed == null) {
                Console.WriteLine($"[  ERROR  ] Content of file \"{file.Name}\" could not be converted to a supported format.");
                return;
            }

            string outputFile = GetOutputFilePath(file, inputType == DataType.UnsafeEncrypted, overwrite);
            await using FileStream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
            await processed.CopyToAsync(outputStream);
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        private static async ValueTask<Stream?> Extract(Stream data) {
            await using Stream encrypted = SaveDataHelper.ExtractEncryptedData(data);
            Stream decrypted = await EncryptionService.DecryptAsync(encrypted);

            DataType newType = SaveDataHelper.GetDataType(decrypted);
            if(newType == DataType.Unsupported) {
                return null;
            }

            if(newType == DataType.Json) {
                return decrypted;
            }

            // Can only be DataType.UnsafeJson at this point
            Stream json = SaveDataHelper.ExtractSafeJsonData(decrypted);
            await decrypted.DisposeAsync();
            return json;
        }

        private static async ValueTask<Stream?> Combine(Stream data) {
            await using Stream encrypted = await EncryptionService.EncryptAsync(data);
            return SaveDataHelper.CombineEncryptedData(encrypted);
        }

        private static string GetOutputFilePath(FileInfo fileInfo, bool isEncrypted, bool overwrite) {
            string extension = fileInfo.Extension.ToUpperInvariant() switch {
                "" => isEncrypted ? ".json" : "", // Special case for Switch which has no file extension
                _ => isEncrypted ? ".json" : ".dat",
            };

            string idealFileName = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}{extension}";
            if(string.Equals(fileInfo.Name, idealFileName, StringComparison.CurrentCultureIgnoreCase)) {
                idealFileName = $"{Path.GetFileNameWithoutExtension(idealFileName)}_{(isEncrypted ? "Decrypted" : "Encrypted")}{extension}";
            }

            string outFileName = Path.Combine(fileInfo.DirectoryName, idealFileName);
            if(overwrite || !File.Exists(outFileName)) {
                return outFileName;
            }

            int fileNumber = 1;
            while(File.Exists(outFileName)) {
                outFileName = $"{outFileName.Substring(outFileName.Length - extension.Length)}_{fileNumber++}{extension}";
            }

            return outFileName;
        }
    }
}
