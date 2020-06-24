using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DungeonTools.Save.File {
    public partial class AesEncryptionProvider : IEncryptionProvider {
        private static readonly SymmetricAlgorithm Algorithm = new AesManaged {
            Mode = CipherMode.ECB,
            Padding = PaddingMode.Zeros,
            Key = Key,
            IV = Iv,
        };

        /// <inheritdoc />
        public ValueTask<Stream> DecryptAsync(Stream encrypted) {
            return TransformAsync(encrypted, Algorithm.CreateDecryptor());
        }

        /// <inheritdoc />
        public ValueTask<Stream> EncryptAsync(Stream decrypted) {
            return TransformAsync(decrypted, Algorithm.CreateEncryptor());
        }

        private static async ValueTask<Stream> TransformAsync(Stream input, ICryptoTransform transform) {
            MemoryStream output = new MemoryStream();

            await using(CryptoStream crypto = new CryptoStream(input, transform, CryptoStreamMode.Read, true)) {
                await crypto.CopyToAsync(output);
            }

            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
    }
}
