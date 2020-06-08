using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Cosmetic {
        [JsonPropertyName("id")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public CosmeticType Type { get; set; }
    }
}
