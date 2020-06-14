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

            (Stream processed, DataType processedType) = await (inputType == DataType.UnsafeEncrypted ? Extract(inputStream) : Combine(inputStream));
            await using(processed) {
                if(processedType == DataType.Unsupported) {
                    Console.WriteLine($"[  ERROR  ] Content of file \"{file.Name}\" could not be converted to a supported format.");
                    return;
                }

                string outputFile = GetOutputFilePath(file, inputType == DataType.UnsafeEncrypted, overwrite);
                await using FileStream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
                await processed.CopyToAsync(outputStream);
            }
        }

        [SuppressMessage("ReSharper", "TailRecursiveCall")]
        private static async ValueTask<(Stream Extracted, DataType NewDataType)> Extract(Stream data, DataType? original = null) {
            DataType type = original ?? SaveDataHelper.GetDataType(data);
            switch(type) {
                case DataType.UnsafeEncrypted: {
                    await using Stream stream = SaveDataHelper.ExtractEncryptedData(data);
                    return await Extract(data, DataType.Encrypted);
                }
                case DataType.Encrypted: {
                    await using Stream stream = await EncryptionService.DecryptAsync(data);
                    return await Extract(stream);
                }
                case DataType.UnsafeJson:
                    return (SaveDataHelper.ExtractSafeJsonData(data), DataType.Json);
                case DataType.Json:
                    return (data, DataType.Json);
                case DataType.Unsupported:
                    return (data, DataType.Unsupported);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Invalid data type detected.");
            }
        }

        [SuppressMessage("ReSharper", "TailRecursiveCall")]
        private static async ValueTask<(Stream Combined, DataType NewDataType)> Combine(Stream data, DataType? original = null) {
            DataType type = original ?? SaveDataHelper.GetDataType(data);
            switch(type) {
                case DataType.UnsafeEncrypted:
                    return (data, DataType.UnsafeEncrypted);
                case DataType.Encrypted:
                    return (SaveDataHelper.CombineEncryptedData(data), DataType.UnsafeEncrypted);
                case DataType.UnsafeJson: {
                    await using Stream stream = await EncryptionService.EncryptAsync(data);
                    return await Combine(stream, DataType.Encrypted);
                }
                case DataType.Json: {
                    await using Stream stream = await EncryptionService.EncryptAsync(data);
                    return await Combine(stream, DataType.Encrypted);
                }
                case DataType.Unsupported:
                    return (data, DataType.Unsupported);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Invalid data type detected.");
            }
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
