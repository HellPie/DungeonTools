using System.IO;

namespace DungeonTools.Encryption {
    public interface IEncryptionService {
        Stream Decrypt(Stream encrypted);
        Stream Encrypt(Stream unencrypted);
    }
}
