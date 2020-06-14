using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DungeonTools.SaveFiles.Mapping;

namespace DungeonTools.SaveFiles.Profiles {
    public static class ProfileParser {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions();

        static ProfileParser() {
            Options.Converters.Add(new AttributeBasedConverterFactory());
            Options.Converters.Add(new GuidConverterFactory());
            Options.Converters.Add(new JsonStringEnumConverter());
        }

        public static async ValueTask<ProfileSaveFile> Read(Stream stream) {
            using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false);
            await using MemoryStream sanitized = new MemoryStream();
            while(reader.BaseStream.Position < reader.BaseStream.Length) {
                byte b = reader.ReadByte();
                if(char.IsControl((char) b)) {
                    continue;
                }

                sanitized.Write(new[] { b });
            }

            sanitized.Seek(0, SeekOrigin.Begin);
            return await JsonSerializer.DeserializeAsync<ProfileSaveFile>(sanitized, Options);
        }

        public static async ValueTask<Stream> Write(ProfileSaveFile settings) {
            Stream stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, settings, Options);
            return stream;
        }
    }
}
