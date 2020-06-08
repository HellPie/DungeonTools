using System.Text.Json.Serialization;

namespace DungeonTools.SaveFiles.Profiles {
    public class LobbyChest {
        [JsonPropertyName("unlockedTimes")]
        public int TimesUnlocked { get; set; }
    }
}
