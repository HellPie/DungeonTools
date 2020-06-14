using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Threats {
        [JsonPropertyName("unlocked")]
        public Threat Unlocked { get; set; }
    }
}
