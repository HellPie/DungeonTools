using System.Diagnostics.CodeAnalysis;

namespace DungeonTools.SaveFiles.Profiles {
    public class Cosmetic {
        [DisallowNull] public string Name { get; set; }
        [DisallowNull] public string Type { get; set; }
    }
}
