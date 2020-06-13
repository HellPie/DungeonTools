using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using CommandLine;
using DungeonTools.Encryption;
using DungeonTools.SaveFiles.Helpers;

namespace DungeonTools.Cli {
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Program {
        private static void Main(string[] args) {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Parser.Default.ParseArguments<RawOptions>(args).MapResult(RawMain, _ => 1);
        }

        private static int RawMain(RawOptions options) {
            bool errorDuringExecution = false;
            foreach(string filePath in options.Input) {
                if(!TryProcessFile(new FileInfo(filePath), options.Overwrite) && !errorDuringExecution) {
                    errorDuringExecution = true;
                }
            }

            return errorDuringExecution ? 1 : 0;
        }

        private static bool TryProcessFile(FileInfo file, bool overwrite) {
            if(!file.Exists) {
                Console.Error.WriteLine($"[  ERROR  ] File \"{file.FullName}\" could not be found and has been skipped.");
                return false;
            }

            using FileStream inputStream = file.OpenRead();
            DataType inputDataType = SaveDataHelper.GetDataType(inputStream);
            if(inputDataType == DataType.Unsupported) {
                Console.WriteLine($"[  ERROR  ] File \"{file.Name}\" could not be identified as either JSON data or encrypted data.");
                return false;
            }

            (Stream extracted, DataType extractedDataType) = Extract(inputStream);
            using(extracted) {
                if(extractedDataType == DataType.Unsupported) {
                    Console.WriteLine($"[  ERROR  ] Content of file \"{file.Name}\" could not be extracted to a supported format.");
                    return false;
                }

                string outputFile = GetOutputFilePath(file, inputDataType == DataType.Encrypted, overwrite);
                using FileStream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
                extracted.CopyTo(outputStream);
            }

            return true;
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        [SuppressMessage("ReSharper", "InvertIf")]
        private static (Stream Extracted, DataType NewDataType) Extract(Stream data) {
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
                    data = service.Decrypt(data);
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
