using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class LevelProgress {
        [JsonPropertyName("completedDifficulty")]
        public Difficulty Difficulty { get; set; }
        [JsonPropertyName("completedThreatLevel")]
        public Threat ThreatLevel { get; set; }
    }
}
