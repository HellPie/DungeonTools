using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class MapSettings {
        [JsonPropertyName("selectedRealm")]
        public Realm Realm { get; set; }
        [JsonPropertyName("selectedMission")]
        public Level Level { get; set; }
        [JsonPropertyName("selectedDifficulty")]
        public Difficulty Difficulty { get; set; }
        [JsonPropertyName("selectedThreatLevel")]
        public Threat ThreatLevel { get; set; }

        [JsonPropertyName("panPosition")]
        public MapPosition VisualPanning { get; set; }
    }
}
