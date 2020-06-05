using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DungeonTools.SaveFiles.Profiles {
    public class ProfileSaveFile {
        public int Version { get; set; }
        public DateTime TimeStamp { get; set; } // Format as Unix timestamp
        public DateTime? CreationDate { get; set; } // Format as "MMM d, yyyy", serialize null as empty string

        public Guid PlayerGuid { get; set; }
        [DisallowNull] public string ProfileName { get; set; } // Can be empty string
        [DisallowNull] public string SkinName { get; set; }

        public bool IsCloned { get; set; }
        public bool IsCustomized { get; set; }

        public uint GearPower { get; set; }
        public uint Experience { get; set; }
        public IEnumerable<Currency> Currencies { get; set; } // Can be empty

        public Difficulties? Difficulties { get; set; }
        public ThreatLevels? ThreatLevels { get; set; }


        public IEnumerable<string> Milestones { get; set; } // Can be empty
        public IDictionary<string, uint>? CompletedObjectives { get; set; }
        public IDictionary<string, LevelProgress>? CompletedLevels { get; set; }
        public IEnumerable<string> BonusLevels { get; set; } // Can be empty
        public IEnumerable CompletedTrials { get; set; } // TODO: Unknown data structure

        public IEnumerable<Cosmetic> Cosmetics { get; set; } // Can be empty
        public IEnumerable<string> CosmeticsHistory { get; set; } // Can be empty
        public object? PendingRewards { get; set; } // TODO: Unknown data structure

        public IEnumerable Inventory { get; set; } // Can be empty
        public IEnumerable<string> ItemsHistory { get; set; } // Can be empty

        public LobbyChest? LobbyChest { get; set; }

        public IDictionary<string, uint>? MobKills { get; set; }
        public MapSettings? MapSettings { get; set; }
        public IEnumerable<Hint> HintsShown { get; set; } // Can be empty
    }
}
