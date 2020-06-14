using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Settings {
    public class SettingsSaveFile {
        [JsonPropertyName("version")]
        public int Version { get; set; }
        [JsonPropertyName("locale")]
        public string Locale { get; set; } // Can be empty

        [JsonPropertyName("msa_account_linked")]
        public bool IsLinkedAccount { get; set; }

        [JsonPropertyName("reconnect")]
        public ReconnectSettings ReconnectSettings { get; set; }
        [JsonPropertyName("recentFiles")]
        public IDictionary<string, string> RecentProfiles { get; set; }

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        [JsonPropertyName("keybinds2")]
        public IDictionary<string, IEnumerable<KeyBinding>?> KeyBindings { get; set; }
        [JsonPropertyName("settings")]
        public IDictionary<string, int?> GameSettings { get; set; }

        [JsonPropertyName("skinsSelected")]
        //[JsonConverter(typeof(CustomNamingEnumJsonConverter<Skin, SnakeCaseNamingPolicy<Skin>>))]
        public IEnumerable<Skin> SkinsSeen { get; set; }
        [JsonPropertyName("trackedStats")]
        public IDictionary<string, object> TrackedStats { get; set; }
    }
}
