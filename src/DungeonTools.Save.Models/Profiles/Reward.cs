using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class Reward {
        [JsonPropertyName("type")]
        public string Name { get; set; }
        [JsonPropertyName("rarity")]
        public Rarity Rarity { get; set; }
        [JsonPropertyName("power")]
        public int Power { get; set; }
    }
}
