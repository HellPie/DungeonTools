using System.Diagnostics.CodeAnalysis;

namespace DungeonTools.SaveFiles.Profiles {
    public class Currency {
        [DisallowNull] public string Name { get; set; }
        public uint Amount { get; set; }
    }
}
