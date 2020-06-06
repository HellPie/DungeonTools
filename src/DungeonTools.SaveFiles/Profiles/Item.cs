using System.Collections.Generic;

namespace DungeonTools.SaveFiles.Profiles {
    public class Item {
        public string Name { get; set; }
        public string Rarity { get; set; }

        public int Power { get; set; } // TODO: Check if Item::Power can be a negative value
        public bool IsUpgraded { get; set; }

        public string? EquipmentSlot { get; set; }
        public int? InventorySlot { get; set; }

        public IEnumerable<Enchantment>? Enchantments { get; set; }
        public IEnumerable<ArmorProperty>? ArmorProperties { get; set; }

        public bool? IsNew { get; set; }
    }
}
