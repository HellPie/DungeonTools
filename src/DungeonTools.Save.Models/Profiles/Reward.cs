using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Reward {
        [JsonPropertyName("type")]
        public string Name { get; set; }
        [JsonPropertyName("rarity")]
        public Rarity Rarity { get; set; }
        [JsonPropertyName("power")]
        public int Power { get; set; }
    }
}
