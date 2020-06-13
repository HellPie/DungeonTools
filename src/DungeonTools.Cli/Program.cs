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

        private static async ValueTask<bool> TryProcessFile(FileInfo file, bool overwrite) {
            if(!file.Exists) {
                await Console.Error.WriteLineAsync($"[  ERROR  ] File \"{file.FullName}\" could not be found and has been skipped.");
                return false;
            }

            await using FileStream inputStream = file.OpenRead();
            DataType inputDataType = SaveDataHelper.GetDataType(inputStream);
            if(inputDataType == DataType.Unsupported) {
                Console.WriteLine($"[  ERROR  ] File \"{file.Name}\" could not be identified as either JSON data or encrypted data.");
                return false;
            }

            (Stream extracted, DataType extractedDataType) = await Extract(inputStream);
            await using(extracted) {
                if(extractedDataType == DataType.Unsupported) {
                    Console.WriteLine($"[  ERROR  ] Content of file \"{file.Name}\" could not be extracted to a supported format.");
                    return false;
                }

                string outputFile = GetOutputFilePath(file, inputDataType == DataType.Encrypted, overwrite);
                await using FileStream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
                await extracted.CopyToAsync(outputStream);
            }

            return true;
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
                    IEncryptionService service = EncryptionServices.Default;
                    data = await service.DecryptAsync(data);
                    type = SaveDataHelper.GetDataType(data);
                }

                return type switch {
                    DataType.UnsafeJson => (SaveDataHelper.ExtractSafeJsonData(data), DataType.Json),
                    _ => (data, DataType.Json),
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
