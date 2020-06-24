using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace DungeonTools.Server.Shared {
    public class EncryptionData {
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public string? Encrypted { get; set; }
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public string? Decrypted { get; set; }

        public Stream? EncryptedStream => Encrypted != null ? new MemoryStream(Convert.FromBase64String(PadBase64String(Encrypted))) : null;
        public Stream? DecryptedStream => Decrypted != null ? new MemoryStream(Convert.FromBase64String(PadBase64String(Decrypted))) : null;

        public static async ValueTask<EncryptionData> From(Stream? encrypted, Stream? decrypted) {
            EncryptionData result = new EncryptionData();

            if(encrypted != null) {
                result.Encrypted = await GetBase64Data(encrypted);
            }

            if(decrypted != null) {
                result.Decrypted = await GetBase64Data(decrypted);
            }

            return result;
        }

        private static async ValueTask<string> GetBase64Data(Stream stream) {
            byte[] data = new byte[stream.Length-stream.Position];
            await stream.ReadAsync(data);
            return Convert.ToBase64String(data);
        }

        private static string PadBase64String(string base64) {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }
    }
}
