using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DungeonTools.SaveFiles.Internal {
    public class GuidConverterFactory : JsonConverterFactory {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert) {
            return typeToConvert == typeof(Guid);
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
            return new GuidJsonConverter();
        }
    }
}
