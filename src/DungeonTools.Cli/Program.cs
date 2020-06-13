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

            (Stream processed, DataType processedType) = await (inputType == DataType.Encrypted ? Extract(inputStream) : Combine(inputStream));
            await using(processed) {
                if(processedType == DataType.Unsupported) {
                    Console.WriteLine($"[  ERROR  ] Content of file \"{file.Name}\" could not be converted to a supported format.");
                    return;
                }

                string outputFile = GetOutputFilePath(file, inputType == DataType.Encrypted, overwrite);
                await using FileStream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
                await processed.CopyToAsync(outputStream);
            }
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        [SuppressMessage("ReSharper", "InvertIf")]
        private static async ValueTask<(Stream Extracted, DataType NewDataType)> Extract(Stream data) {
            DataType type = SaveDataHelper.GetDataType(data);
            if(type == DataType.Unsupported) {
                return (data, DataType.Unsupported);
            }

            if(type == DataType.UnsafeEncrypted) {
                data = SaveDataHelper.ExtractEncryptedData(data);
                type = DataType.Encrypted;
            }

            if(type == DataType.Encrypted) {
                data = await EncryptionService.DecryptAsync(data);
                type = SaveDataHelper.GetDataType(data);
            }

            return type switch {
                DataType.UnsafeJson => (SaveDataHelper.ExtractSafeJsonData(data), DataType.Json),
                _ => (data, DataType.Json),
            };
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        [SuppressMessage("ReSharper", "InvertIf")]
        private static async ValueTask<(Stream Combined, DataType NewDataType)> Combine(Stream data) {
            DataType type = SaveDataHelper.GetDataType(data);
            if(type == DataType.Unsupported) {
                return (data, DataType.Unsupported);
            }

            if(type == DataType.Json || type == DataType.UnsafeJson) {
                data = await EncryptionService.EncryptAsync(data);
                type = DataType.Encrypted;
            }

            return type switch {
                DataType.Encrypted => (SaveDataHelper.CombineEncryptedData(data), DataType.UnsafeEncrypted),
                _ => (data, DataType.UnsafeEncrypted),
            };
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
