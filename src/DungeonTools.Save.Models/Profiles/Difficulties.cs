using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class Difficulties {
        [JsonPropertyName("selected")]
        public Difficulty Selected { get; set; }
        [JsonPropertyName("unlocked")]
        public Difficulty? Unlocked { get; set; }
        [JsonPropertyName("announced")]
        public Difficulty? Announced { get; set; }
    }
}
