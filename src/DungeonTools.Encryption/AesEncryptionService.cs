using System.IO;
using System.Security.Cryptography;

namespace DungeonTools.Encryption {
    public partial class AesEncryptionService : IEncryptionService {
        /// <inheritdoc />
        public Stream Decrypt(Stream encrypted) {
            return Transform(encrypted, Algorithm.CreateDecryptor());
        }

        /// <inheritdoc />
        public Stream Encrypt(Stream decrypted) {
            return Transform(decrypted, Algorithm.CreateEncryptor());
        }

        private static Stream Transform(Stream input, ICryptoTransform transform) {
            MemoryStream output = new MemoryStream();

            using(CryptoStream crypto = new CryptoStream(input, transform, CryptoStreamMode.Read, true)) {
                crypto.CopyTo(output);
            }

            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
    }
}
