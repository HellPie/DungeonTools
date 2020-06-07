using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Enchantment {
        public EnchantmentType Type { get; set; }
        public int Level { get; set; } // Min: 0; Max: 3
    }
}
