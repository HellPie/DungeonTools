using System;
using System.IO;
using System.Text;
using CommandLine;
using DungeonTools.SaveFiles.Encryption;

namespace DungeonTools.Cli {
    internal class Program {
        private static void Main(string[] args) {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Parser.Default.ParseArguments<RawOptions>(args).MapResult(RawMain, _ => 1);
        }

        private static int RawMain(RawOptions options) {
            FileInfo fileInfo = new FileInfo(options.Input);
            if(!fileInfo.Exists) {
                Console.Error.WriteLine("Selected input file could not be found.");
                return -1;
            }

            bool isDatFile = string.Equals(".DAT", fileInfo.Extension, StringComparison.OrdinalIgnoreCase);
            if(options.Encrypt && isDatFile) {
                Console.Out.WriteLine("[WARNING] Trying to encrypt a .DAT file, which is usually already encrypted.");
            }

            bool isJsonFile = string.Equals(".JSON", fileInfo.Extension, StringComparison.OrdinalIgnoreCase);
            if(!options.Encrypt && isJsonFile) {
                Console.Out.WriteLine("[WARNING] Trying to decrypt a .JSON file, which is usually already decrypted.");
            }

            if(!isDatFile && !isJsonFile) {
                Console.Out.WriteLine("[WARNING] This command should only be used with .JSON or .DAT files.");
            }

            string extension = fileInfo.Extension.ToUpperInvariant() switch {
                ".DAT" => ".json",
                ".JSON" => ".dat",
                _ => options.Encrypt ? ".DAT" : ".JSON",
            };

            int counter = 1;
            string originalOutFilePath = Path.Combine(fileInfo.DirectoryName, $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}{extension}");
            string outFilePath = originalOutFilePath;
            while(File.Exists(outFilePath)) {
                outFilePath = $"{originalOutFilePath.Substring(0, originalOutFilePath.Length - extension.Length)}_{counter++}{extension}";
            }

            using FileStream fileStream = fileInfo.OpenRead();
            using Stream resultStream = options.Encrypt ? EncryptedSaveFile.Encrypt(fileStream) : EncryptedSaveFile.Decrypt(fileStream);
            using FileStream outFileStream = File.OpenWrite(outFilePath);
            resultStream.CopyTo(outFileStream);

            return 0;
        }
    }
}
