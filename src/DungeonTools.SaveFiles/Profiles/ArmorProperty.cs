using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class ArmorProperty {
        [JsonPropertyName("id")]
        public ArmorPropertyType Type { get; set; }
        [JsonPropertyName("rarity")]
        public Rarity Rarity { get; set; }
    }
}
