using System.Diagnostics.CodeAnalysis;

namespace DungeonTools.SaveFiles.Profiles {
    public class LevelProgress {
        [DisallowNull] public string Difficulty { get; set; }
        [DisallowNull] public string ThreatLevel { get; set; }
    }
}
