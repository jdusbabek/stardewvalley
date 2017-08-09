using StardewLib;

namespace ExtremePetting
{
    public class AnimalSitterConfig : IConfig
    {
        public string keybind { get; set; }
        public bool growUpEnabled { get; set; }
        public bool maxHappinessEnabled { get; set; }
        public bool maxFullnessEnabled { get; set; }
        public bool harvestEnabled { get; set; }
        public bool pettingEnabled { get; set; }
        public bool maxFriendshipEnabled { get; set; }
        public bool verboseLogging { get; set; }
        public int costPerAction { get; set; }
        //public string whoChecks { get; set; }
        public bool enableMessages { get; set; }
        public bool takeTrufflesFromPigs { get; set; }
        public bool bypassInventory { get; set; }
        public Microsoft.Xna.Framework.Vector2 chestCoords { get; set; }
        public string chestDefs { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            keybind = "O";
            growUpEnabled = false;
            maxHappinessEnabled = false;
            maxFullnessEnabled = false;
            harvestEnabled = true;
            pettingEnabled = true;
            maxFriendshipEnabled = false;
            verboseLogging = false;
            costPerAction = 0;
            whoChecks = "spouse";
            enableMessages = true;
            takeTrufflesFromPigs = true;
            chestCoords = new Microsoft.Xna.Framework.Vector2(73f, 14f);
            bypassInventory = false;
            chestDefs = "";

            return this as T;
        }
    }
}
