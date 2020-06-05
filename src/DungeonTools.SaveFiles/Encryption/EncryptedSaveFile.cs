using System.IO;
using System.Security.Cryptography;

namespace DungeonTools.SaveFiles.Encryption {
    public static class EncryptedSaveFile {
        private static readonly byte[] Magic = {0x44, 0x30, 0x30, 0x31, 0x00, 0x00, 0x00, 0x00};
        private static readonly byte[] Iv = new byte[16];
        private static readonly byte[] Key = new byte[32];

        public static Stream Encrypt(Stream input) {
            ICryptoTransform encryption = GetEncryptionAlgorithm().CreateEncryptor();

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
            ICryptoTransform decryption = GetEncryptionAlgorithm().CreateDecryptor();

            byte[] data = new byte[input.Length - Magic.Length];
            input.Seek(Magic.Length, SeekOrigin.Begin);
            input.Read(data);

            data = decryption.TransformFinalBlock(data, 0, data.Length);
            return new MemoryStream(data);
        }

        private static Aes GetEncryptionAlgorithm() {
            return new AesManaged {
                Key = Key,
                IV = Iv,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None,
            };
        }
    }
}
