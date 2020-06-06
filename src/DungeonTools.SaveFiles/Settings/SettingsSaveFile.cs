using System.Collections.Generic;

namespace DungeonTools.SaveFiles.Settings {
    public class SettingsSaveFile {
        public int Version { get; set; }
        public string Locale { get; set; } // Can be empty

        public bool IsLinkedAccount { get; set; }

        public ReconnectSettings ReconnectSettings { get; set; }
        public IDictionary<string, string> RecentProfiles { get; set; }

        public IDictionary<string, IEnumerable<KeyBinding>?> KeyBindings { get; set; }
        public IDictionary<string, int?> GameSettings { get; set; }

        public IEnumerable<string> SkinsSeen { get; set; }
        public IDictionary<string, object> TrackedStats { get; set; }
    }
}
