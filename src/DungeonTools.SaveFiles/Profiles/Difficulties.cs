using System.Diagnostics.CodeAnalysis;

namespace DungeonTools.SaveFiles.Profiles {
    public class Difficulties {
        [DisallowNull] public string Selected { get; set; }
        [AllowNull] public string Unlocked { get; set; }
        [AllowNull] public string Announced { get; set; }
    }
}
