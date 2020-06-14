using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class LevelProgress {
        [JsonPropertyName("completedDifficulty")]
        public Difficulty Difficulty { get; set; }
        [JsonPropertyName("completedThreatLevel")]
        public Threat ThreatLevel { get; set; }
    }
}
