using System;
using System.Text.Json.Serialization;

namespace DungeonTools.SaveFiles.Settings {
    public class ReconnectSettings {
        [JsonPropertyName("guid")]
        public Guid Guid { get; set; }
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } // Can be empty
    }
}
