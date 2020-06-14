using System.Text.Json.Serialization;

namespace DungeonTools.Save.Models.Profiles {
    public class LobbyChest {
        [JsonPropertyName("unlockedTimes")]
        public int TimesUnlocked { get; set; }
    }
}
