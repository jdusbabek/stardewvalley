using Microsoft.Xna.Framework;
using StardewModdingAPI;


namespace Replanter
{
    public class ReplanterConfig : Config
    {
        public string keybind { get; set; }

        public bool enableLogging { get; set; }

        public bool free { get; set; }

        public int seedDiscount { get; set; }

        public string whoChecks { get; set; }

        public bool enableMessages { get; set; }

        // If negative, don't add to inventory.
        public float costPerCropHarvested { get; set; }

        public bool sellHarvestedCropsImmediately { get; set; }

        public bool waterCrops { get; set; }

        public string ignoreList { get; set; }

        public string alwaysSellList { get; set; }

        public string neverSellList { get; set; }

        // The X, Y coordinates of a chest, into which surplus items can be deposited.  The farmers inventory will be tried first, unless bypassInventory is true.
        public Vector2 chestCoords { get; set; }

        // Whether to bypass the user's inventory and try depositing to the chest first.  Will fall back to the inventory if no chest is present.
        public bool bypassInventory { get; set; }

        public string chestDefs { get; set; }

        public bool clearDeadPlants { get; set; }

        public bool smartReplantingEnabled { get; set; }


        public override T GenerateDefaultConfig<T>()
        {
            keybind = "J";
            enableLogging = false;
            free = false;
            seedDiscount = 0;
            whoChecks = "spouse";
            enableMessages = true;
            costPerCropHarvested = 0.5f;
            sellHarvestedCropsImmediately = false;
            waterCrops = false;
            ignoreList = "591|593|595|597|376";
            alwaysSellList = "";
            neverSellList = "";
            chestCoords = new Vector2(70f, 14f);
            bypassInventory = false;
            chestDefs = "613,70,14|643,73,14";
            clearDeadPlants = true;
            smartReplantingEnabled = true;

            return this as T;
        }

    }
}
