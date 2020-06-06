using System;
using System.Collections;
using System.Collections.Generic;

namespace DungeonTools.SaveFiles.Profiles {
    public class ProfileSaveFile {
        public int Version { get; set; }
        public DateTime TimeStamp { get; set; } // Format as Unix timestamp
        public DateTime? CreationDate { get; set; } // Format as "MMM d, yyyy", serialize null as empty string

        public Guid PlayerGuid { get; set; }
        public string ProfileName { get; set; } // Can be empty string
        public string SkinName { get; set; }

        public bool IsCloned { get; set; }
        public bool IsCustomized { get; set; }

        public int GearPower { get; set; }
        public int Experience { get; set; }
        public IEnumerable<Currency> Currencies { get; set; } // Can be empty

        public Difficulties? Difficulties { get; set; }
        public ThreatLevels? ThreatLevels { get; set; }


        public IEnumerable<string> Milestones { get; set; } // Can be empty
        public IDictionary<string, int>? CompletedObjectives { get; set; }
        public IDictionary<string, LevelProgress>? CompletedLevels { get; set; }
        public IEnumerable<string> BonusLevels { get; set; } // Can be empty
        public IEnumerable CompletedTrials { get; set; } // TODO: This is not currently available in the game.

        public IEnumerable<Cosmetic> Cosmetics { get; set; } // Can be empty
        public IEnumerable<string> CosmeticsHistory { get; set; } // Can be empty
        public Reward? PendingReward { get; set; }

        public IEnumerable<Item> Inventory { get; set; } // Can be empty
        public IEnumerable<string> ItemsHistory { get; set; } // Can be empty

        public LobbyChest? LobbyChest { get; set; }

        public IDictionary<string, int>? MobKills { get; set; }
        public MapSettings? MapSettings { get; set; }
        public IEnumerable<Hint> HintsShown { get; set; } // Can be empty
    }
}
