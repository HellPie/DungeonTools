using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Difficulties {
        [JsonPropertyName("selected")]
        public Difficulty Selected { get; set; }
        [JsonPropertyName("unlocked")]
        public Difficulty? Unlocked { get; set; }
        [JsonPropertyName("announced")]
        public Difficulty? Announced { get; set; }
    }
}
