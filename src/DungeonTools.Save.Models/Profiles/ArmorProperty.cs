using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class ArmorProperty {
        [JsonPropertyName("id")]
        public ArmorPropertyType Type { get; set; }
        [JsonPropertyName("rarity")]
        public Rarity Rarity { get; set; }
    }
}
