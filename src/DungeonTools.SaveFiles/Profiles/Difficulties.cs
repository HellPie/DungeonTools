using DungeonTools.SaveFiles.Enums;

namespace DungeonTools.SaveFiles.Profiles {
    public class Difficulties {
        public Difficulty Selected { get; set; }
        public Difficulty? Unlocked { get; set; }
        public Difficulty? Announced { get; set; }
    }
}
