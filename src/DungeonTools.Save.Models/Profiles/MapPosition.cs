using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Mapping;

namespace DungeonTools.Save.Models.Profiles {
    public class MapPosition {
        [JsonPropertyName("x")]
        [JsonConverter(typeof(TextDoubleJsonConverter))]
        public double X { get; set; } // Serialize as string
        [JsonPropertyName("y")]
        [JsonConverter(typeof(TextDoubleJsonConverter))]
        public double Y { get; set; } // Serialize as string
    }
}
