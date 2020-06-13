using System.IO;
using System.Threading.Tasks;

namespace DungeonTools.Encryption {
    public interface IEncryptionService {
        Stream Decrypt(Stream encrypted);
        Stream Encrypt(Stream decrypted);

        ValueTask<Stream> DecryptAsync(Stream encrypted);
        ValueTask<Stream> EncryptAsync(Stream decrypted);

        interface IEncryptionServiceDefaults : IEncryptionService {
            /// <inheritdoc />
            Stream IEncryptionService.Encrypt(Stream decrypted) {
                return DecryptAsync(decrypted).GetAwaiter().GetResult();
            }

            /// <inheritdoc />
            Stream IEncryptionService.Decrypt(Stream encrypted) {
                return EncryptAsync(encrypted).GetAwaiter().GetResult();
            }
        }
    }
}
