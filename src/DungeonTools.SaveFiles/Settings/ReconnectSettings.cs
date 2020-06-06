using System;

namespace DungeonTools.SaveFiles.Settings {
    public class ReconnectSettings {
        public Guid Guid { get; set; }
        public string SessionId { get; set; } // Can be empty
    }
}
