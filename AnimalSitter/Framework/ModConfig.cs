using Microsoft.Xna.Framework;
using StardewLib;

namespace AnimalSitter.Framework
{
    internal class ModConfig : IConfig
    {
        public string KeyBind { get; set; } = "O";
        public bool GrowUpEnabled { get; set; } = true;
        public bool MaxHappinessEnabled { get; set; } = true;
        public bool MaxFullnessEnabled { get; set; } = true;
        public bool HarvestEnabled { get; set; } = true;
        public bool PettingEnabled { get; set; } = true;
        public bool MaxFriendshipEnabled { get; set; }
        public int CostPerAction { get; set; } = 25;
        public string WhoChecks { get; set; } = "spouse";
        public bool EnableMessages { get; set; } = true;
        public bool TakeTrufflesFromPigs { get; set; } = true;
        public bool BypassInventory { get; set; } = true;
        public Vector2 ChestCoords { get; set; } = new Vector2(73, 14);
        public string ChestDefs { get; set; } = "";
    }
}
