using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DungeonTools.Save.Models.Mapping;

namespace DungeonTools.Save.Models.Enums {
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "CommentTypo")]
    [JsonConverter(typeof(CustomNamingEnumJsonConverter<Realm, RealmNamingPolicy>))]
    public enum Realm {
        ArchIllager, // Original: ArchIllagerRealm
        Islands, // Jungle DLC; Winter DLC; Original: IslandsRealm
    }
}
