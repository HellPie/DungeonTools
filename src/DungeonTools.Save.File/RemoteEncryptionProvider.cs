using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DungeonTools.Encryption {
    public class RemoteEncryptionProvider : IEncryptionProvider {
#if DEBUG
        private const string ClientAddress = "http://127.0.0.1:5000";
#else
        private const string ClientAddress = "https://dungeons.tools";
#endif
        public class ApiEncryptionModel {
            public string? Encrypted { get; set; }
            public string? Decrypted { get; set; }
        }

        private static readonly HttpClient Client = new HttpClient {
            BaseAddress = new Uri(ClientAddress),
        };

        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        /// <inheritdoc />
        public async ValueTask<Stream> DecryptAsync(Stream encrypted) {
            ApiEncryptionModel model = new ApiEncryptionModel {Encrypted = await GetBase64Data(encrypted)};
            ApiEncryptionModel body = await CallEndpoint(model, "api/encryption/decrypt");
            MemoryStream output = new MemoryStream(Convert.FromBase64String(PadBase64String(body.Decrypted!)));
            return output;
        }

        /// <inheritdoc />
        public async ValueTask<Stream> EncryptAsync(Stream decrypted) {
            ApiEncryptionModel model = new ApiEncryptionModel {Decrypted = await GetBase64Data(decrypted)};
            ApiEncryptionModel response = await CallEndpoint(model, "api/encryption/encrypt");
            MemoryStream output = new MemoryStream(Convert.FromBase64String(PadBase64String(response.Encrypted!)));
            return output;
        }

        private static async ValueTask<ApiEncryptionModel> CallEndpoint(ApiEncryptionModel input, string endpoint) {
            using HttpContent content = new StringContent(JsonSerializer.Serialize(input));
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = await Client.PostAsync(endpoint, content);
            return JsonSerializer.Deserialize<ApiEncryptionModel>(await response.Content.ReadAsStringAsync(), SerializerOptions);
        }

        public static async ValueTask<string> GetBase64Data(Stream stream) {
            byte[] data = new byte[stream.Length-stream.Position];
            await stream.ReadAsync(data);
            return Convert.ToBase64String(data);
        }

        public static string PadBase64String(string base64) {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }
    }
}
