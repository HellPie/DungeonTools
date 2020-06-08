using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Settings {
    public class KeyBinding {
        [JsonPropertyName("platform")]
        public Platform Platform { get; set; }
        [JsonPropertyName("action")]
        public string Action { get; set; } // Should be Enum but possible values are mixed with other game assets
        [JsonPropertyName("key")]
        public string Key { get; set; } // Should be Enum but Platform.KeyboardMouse introduces too many possible values
    }
}
