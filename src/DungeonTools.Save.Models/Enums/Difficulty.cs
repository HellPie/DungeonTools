using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Mapping;

namespace DungeonTools.Save.Models.Enums {
    [JsonConverter(typeof(CustomNamingEnumJsonConverter<Difficulty, DifficultyNamingPolicy>))]
    public enum Difficulty {
        Default, // Original: Difficulty_1
        Adventure, // Original: Difficulty_2
        Apocalypse, // Original: Difficulty_3
    }
}
