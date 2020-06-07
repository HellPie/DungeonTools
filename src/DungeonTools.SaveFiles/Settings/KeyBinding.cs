using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Settings {
    public class KeyBinding {
        public Platform Platform { get; set; }
        public string Action { get; set; } // Should be Enum but possible values are mixed with other game assets
        public string Key { get; set; } // Should be Enum but Platform.KeyboardMouse introduces too many possible values
    }
}
