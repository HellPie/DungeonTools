using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Hint {
        [JsonPropertyName("hintType")]
        public HintType Type { get; set; }
    }
}
