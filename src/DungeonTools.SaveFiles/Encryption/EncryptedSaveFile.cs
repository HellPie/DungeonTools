using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DungeonTools.SaveFiles.Encryption {
    public static class EncryptedSaveFile {
        private static readonly byte[] Magic = {0x44, 0x30, 0x30, 0x31, 0x00, 0x00, 0x00, 0x00};
        private static readonly byte[] Iv = new byte[16];
        private static readonly byte[] Key = new byte[32];

        private static readonly SymmetricAlgorithm Algorithm = new AesManaged {
            Key = Key,
            IV = Iv,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.None,
        };

        public static FileSupportStatus GetSupportStatus(Stream input) {
            if(input.Length <= Magic.Length) {
                return FileSupportStatus.Unsupported;
            }

            long origPos = input.Position;
            input.Seek(0, SeekOrigin.Begin);

            byte[] magic = new byte[Magic.Length];
            input.Read(magic);

            if(magic.SequenceEqual(Magic)) {
                input.Seek(origPos, SeekOrigin.Begin);
                return FileSupportStatus.Encrypted;
            }

            // Read until a valid Json "begin object" token (a literal "{") is found)
            input.Seek(0, SeekOrigin.Begin);
            FileSupportStatus status = FileSupportStatus.Unsupported;
            using BinaryReader reader = new BinaryReader(input, Encoding.UTF8, true);
            while(reader.BaseStream.Position < reader.BaseStream.Length) {
                char ch = reader.ReadChar();

                if(char.IsWhiteSpace(ch)) {
                    continue;
                }

                if(ch == '{') {
                    status = FileSupportStatus.Unencrypted;
                }

                break;
            }

            input.Seek(origPos, SeekOrigin.Begin);
            return status;
        }

        public static Stream Encrypt(Stream input) {
            ICryptoTransform encryption = Algorithm.CreateEncryptor();

            byte[] data = new byte[input.Length];
            input.Read(data, 0, data.Length);

            data = encryption.TransformFinalBlock(data, 0, data.Length);

            MemoryStream stream = new MemoryStream();
            stream.Write(Magic);
            stream.Write(data);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Stream Decrypt(Stream input) {
            ICryptoTransform decryption = Algorithm.CreateDecryptor();

            byte[] data = new byte[input.Length - Magic.Length];
            input.Seek(Magic.Length, SeekOrigin.Begin);
            input.Read(data);

            data = decryption.TransformFinalBlock(data, 0, data.Length);
            return new MemoryStream(data);
        }
    }
}
