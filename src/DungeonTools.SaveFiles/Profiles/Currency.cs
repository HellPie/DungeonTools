using System.Text.Json.Serialization;

namespace DungeonTools.SaveFiles.Profiles {
    public class Currency {
        [JsonPropertyName("type")]
        public string Name { get; set; }
        [JsonPropertyName("count")]
        public uint Amount { get; set; }
    }
}
