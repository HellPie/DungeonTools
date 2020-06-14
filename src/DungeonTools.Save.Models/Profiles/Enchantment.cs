using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class Enchantment {
        [JsonPropertyName("id")]
        public EnchantmentType Type { get; set; }
        [JsonPropertyName("level")]
        public int Level { get; set; } // Min: 0; Max: 3
    }
}
