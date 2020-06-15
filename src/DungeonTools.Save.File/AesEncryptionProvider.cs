using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DungeonTools.Save.File {
    public partial class AesEncryptionProvider : IEncryptionProvider {
        public static readonly SymmetricAlgorithm Algorithm = new AesManaged {
            Mode = CipherMode.ECB,
            Padding = PaddingMode.Zeros,
            Key = Key,
            IV = IV,
        };

        /// <inheritdoc />
        public async ValueTask<Stream> DecryptAsync(Stream encrypted) {
            return await TransformAsync(encrypted, Algorithm.CreateDecryptor());
        }

        /// <inheritdoc />
        public async ValueTask<Stream> EncryptAsync(Stream decrypted) {
            await using Stream padded = await GetPaddedToBlock(decrypted);
            return await TransformAsync(padded, Algorithm.CreateEncryptor());
        }

        private static async ValueTask<Stream> TransformAsync(Stream input, ICryptoTransform transform) {
            MemoryStream output = new MemoryStream();

            await using(CryptoStream crypto = new CryptoStream(input, transform, CryptoStreamMode.Read, true)) {
                await crypto.CopyToAsync(output);
            }

            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        private static async ValueTask<Stream> GetPaddedToBlock(Stream input) {
            MemoryStream output = new MemoryStream();
            await input.CopyToAsync(output);
            await output.WriteAsync(Enumerable.Repeat((byte) ' ', (int) (input.Length % 16)).ToArray());
            return output;
        }
    }
}
