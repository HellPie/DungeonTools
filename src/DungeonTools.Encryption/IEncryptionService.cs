using System.IO;
using System.Threading.Tasks;

namespace DungeonTools.Encryption {
    public interface IEncryptionService {
        ValueTask<Stream> DecryptAsync(Stream encrypted);
        ValueTask<Stream> EncryptAsync(Stream decrypted);
    }
}
