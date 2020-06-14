using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class Threats {
        [JsonPropertyName("unlocked")]
        public Threat Unlocked { get; set; }
    }
}
