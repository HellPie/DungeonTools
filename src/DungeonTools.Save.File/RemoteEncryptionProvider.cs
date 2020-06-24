using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DungeonTools.Server.Shared;

namespace DungeonTools.Save.File {
    public class RemoteEncryptionProvider : IEncryptionProvider {
#if DEBUG
        private const string ClientAddress = ServerConstants.LocalServer;
#else
        private const string ClientAddress = ServerConstants.RemoteServer;
#endif

        private static readonly HttpClient Client = new HttpClient {
            BaseAddress = new Uri(ClientAddress),
        };

        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            IgnoreReadOnlyProperties = true,
        };

        /// <inheritdoc />
        public async ValueTask<Stream> DecryptAsync(Stream encrypted) {
            return (await CallEndpoint(encrypted, null, ServerConstants.DecryptEndpoint)).DecryptedStream!;
        }

        /// <inheritdoc />
        public async ValueTask<Stream> EncryptAsync(Stream decrypted) {
            return (await CallEndpoint(null, decrypted, ServerConstants.EncryptEndpoint)).EncryptedStream!;
        }

        private static async ValueTask<EncryptionData> CallEndpoint(Stream? encrypted, Stream? decrypted, string endpoint) {
            using HttpContent content = new StringContent(JsonSerializer.Serialize(await EncryptionData.From(encrypted, decrypted)));
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = await Client.PostAsync(endpoint, content);
            return JsonSerializer.Deserialize<EncryptionData>(await response.Content.ReadAsStringAsync(), SerializerOptions);
        }
    }
}
