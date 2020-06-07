using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using CommandLine;
using DungeonTools.SaveFiles.Encryption;

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
                FileInfo fileInfo = new FileInfo(filePath);
                if(!fileInfo.Exists) {
                    Console.Error.WriteLine($"[  ERROR  ] File \"{filePath}\" could not be found and has been skipped.");
                    errorDuringExecution = true;
                    continue;
                }

                if(!TryProcessFile(fileInfo, options.Overwrite) && !errorDuringExecution) {
                    errorDuringExecution = true;
                }
            }

            return errorDuringExecution ? 1 : 0;
        }

        private static bool TryProcessFile(FileInfo file, bool overwrite) {
            using FileStream inStream = file.OpenRead();
            FileSupportStatus status = EncryptedSaveFile.GetSupportStatus(inStream);
            if(status == FileSupportStatus.Unsupported) {
                Console.WriteLine($"[  ERROR  ] File \"{file.Name}\" could not be identified as either JSON data or encrypted data.");
                return false;
            }

            bool isDatFile = string.Equals(".DAT", file.Extension, StringComparison.OrdinalIgnoreCase);
            bool isJsonFile = string.Equals(".JSON", file.Extension, StringComparison.OrdinalIgnoreCase);
            if(isDatFile && status == FileSupportStatus.Unencrypted) {
                Console.WriteLine($"[ WARNING ] File \"{file.Name}\" is an unencrypted DAT file. Usually only JSON files are unencrypted. It will be encrypted to another DAT file.");
            }

            if(isJsonFile && status == FileSupportStatus.Encrypted) {
                Console.WriteLine($"[ WARNING ] File \"{file.Name}\" is an encrypted JSON file. Usually only DAT files are encrypted. It will be decrypted to another JSON file.");
            }

            if(!isDatFile && !isJsonFile && !string.IsNullOrWhiteSpace(file.Extension)) {
                Console.WriteLine($"[ WARNING ] File \"{file.Name}\" is not a DAT or JSON file. It has been detected as {(status == FileSupportStatus.Encrypted ? "encrypted" : "unencrypted")} and will be treated as such.");
            }

            // Build output file name

            string outFilePath = GetOutputFilePath(file, status, overwrite);
            if(File.Exists(outFilePath)) {
                File.Delete(outFilePath);
            }

            using Stream processedStream = status == FileSupportStatus.Encrypted ? EncryptedSaveFile.Decrypt(inStream) : EncryptedSaveFile.Encrypt(inStream);
            using FileStream outStream = File.OpenWrite(outFilePath);
            processedStream.CopyTo(outStream);
            return true;
        }

        private static string GetOutputFilePath(FileInfo fileInfo, FileSupportStatus status, bool overwrite) {
            string extension = fileInfo.Extension.ToUpperInvariant() switch {
                "" => status == FileSupportStatus.Encrypted ? ".json" : "", // Special case for Switch which has no file extension
                _ => status == FileSupportStatus.Encrypted ? ".json" : ".dat",
            };

            string idealFileName = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}{extension}";
            if(string.Equals(fileInfo.Name, idealFileName, StringComparison.CurrentCultureIgnoreCase)) {
                idealFileName = $"{Path.GetFileNameWithoutExtension(idealFileName)}_{(status == FileSupportStatus.Encrypted ? "Decrypted" : "Encrypted")}{extension}";
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
