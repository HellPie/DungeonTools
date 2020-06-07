using System.Collections.Generic;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Item {
        public string Name { get; set; } // Should be Enum but possible values are mixed with other game assets
        public Rarity Rarity { get; set; }

        public int Power { get; set; }
        public bool IsUpgraded { get; set; }

        public string? EquipmentSlot { get; set; }
        public int? InventorySlot { get; set; }

        public IEnumerable<Enchantment>? Enchantments { get; set; }
        public IEnumerable<ArmorProperty>? ArmorProperties { get; set; }

        public bool? IsNew { get; set; }
    }
}
