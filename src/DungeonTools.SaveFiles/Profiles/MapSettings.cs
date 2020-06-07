using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class MapSettings {
        public Realm Realm { get; set; }
        public Level Level { get; set; }
        public Difficulty Difficulty { get; set; }
        public Threat ThreatLevel { get; set; }
        public MapPosition VisualPanning { get; set; }
    }
}
