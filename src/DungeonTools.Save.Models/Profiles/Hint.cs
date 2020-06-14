using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Enums;

namespace DungeonTools.Save.Models.Profiles {
    public class Hint {
        [JsonPropertyName("hintType")]
        public HintType Type { get; set; }
    }
}
