using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class Cosmetic {
        [JsonPropertyName("id")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public CosmeticType Type { get; set; }
    }
}
