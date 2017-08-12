using Microsoft.Xna.Framework;
using StardewLib;

namespace ExtremePetting
{
    internal class AnimalSitterConfig : IConfig
    {
        public string keybind { get; set; } = "O";
        public bool growUpEnabled { get; set; }
        public bool maxHappinessEnabled { get; set; }
        public bool maxFullnessEnabled { get; set; }
        public bool harvestEnabled { get; set; } = true;
        public bool pettingEnabled { get; set; } = true;
        public bool maxFriendshipEnabled { get; set; }
        public int costPerAction { get; set; }
        public string WhoChecks { get; set; } = "spouse";
        public bool enableMessages { get; set; } = true;
        public bool takeTrufflesFromPigs { get; set; } = true;
        public bool bypassInventory { get; set; }
        public Vector2 chestCoords { get; set; } = new Vector2(73f, 14f);
        public string chestDefs { get; set; } = "";
    }
}
