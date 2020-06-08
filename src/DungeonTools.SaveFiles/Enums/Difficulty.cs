using System.Text.Json.Serialization;
using DungeonTools.SaveFiles.Internal;

namespace DungeonTools.SaveFiles.Enums {
    [JsonConverter(typeof(CustomNamingEnumJsonConverter<Difficulty, DifficultyNamingPolicy>))]
    public enum Difficulty {
        Default, // Original: Difficulty_1
        Adventure, // Original: Difficulty_2
        Apocalypse, // Original: Difficulty_3
    }
}
