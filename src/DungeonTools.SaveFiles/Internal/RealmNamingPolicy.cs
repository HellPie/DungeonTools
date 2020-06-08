using System;
using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Internal {
    public class RealmNamingPolicy : INamingPolicy<Realm> {
        /// <inheritdoc />
        public Realm ConvertName(string name) {
            return (Realm) Enum.Parse(typeof(Realm), name.Replace("Realm", ""), true);
        }

        /// <inheritdoc />
        public string ConvertValue(Realm value) {
            string enumString = typeof(Realm).GetEnumName(value)!;
            return $"{enumString}Realm";
        }
    }
}
