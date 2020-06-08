using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DungeonTools.SaveFiles.Internal;

namespace DungeonTools.SaveFiles.Settings {
    public static class SettingsReader {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions();

        static SettingsReader() {
            Options.Converters.Add(new AttributeBasedConverterFactory());
            Options.Converters.Add(new GuidConverterFactory());
            Options.Converters.Add(new JsonStringEnumConverter());
        }

        public static async ValueTask<SettingsSaveFile> Read(Stream stream) {
            return await JsonSerializer.DeserializeAsync<SettingsSaveFile>(stream, Options);
        }

        public static async ValueTask<Stream> Write(SettingsSaveFile settings) {
            Stream stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, settings, Options);
            return stream;
        }
    }
}
