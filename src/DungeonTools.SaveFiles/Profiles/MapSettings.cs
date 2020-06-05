using System.Diagnostics.CodeAnalysis;

namespace DungeonTools.SaveFiles.Profiles {
    public class MapSettings {
        [DisallowNull] public string Realm { get; set; }
        [DisallowNull] public string Level { get; set; }
        [DisallowNull] public string Difficulty { get; set; }
        [DisallowNull] public string ThreatLevel { get; set; }
        public MapPosition VisualPanning { get; set; }
    }
}
