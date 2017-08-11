using Microsoft.Xna.Framework;
using StardewLib;

namespace Replanter
{
    internal class ReplanterConfig : IConfig
    {
        public string keybind { get; set; } = "J";

        public bool enableLogging { get; set; }

        public bool free { get; set; }

        public int seedDiscount { get; set; }

        public string WhoChecks { get; set; } = "spouse";

        public bool enableMessages { get; set; } = true;

        // If negative, don't add to inventory.
        public float costPerCropHarvested { get; set; } = 0.5f;

        public bool sellHarvestedCropsImmediately { get; set; }

        public bool waterCrops { get; set; }

        public string ignoreList { get; set; } = "591|593|595|597|376";

        public string alwaysSellList { get; set; } = "";

        public string neverSellList { get; set; } = "";

        // The X, Y coordinates of a chest, into which surplus items can be deposited.  The farmers inventory will be tried first, unless bypassInventory is true.
        public Vector2 chestCoords { get; set; } = new Vector2(70f, 14f);

        // Whether to bypass the user's inventory and try depositing to the chest first.  Will fall back to the inventory if no chest is present.
        public bool bypassInventory { get; set; }

        public string chestDefs { get; set; } = "613,70,14|643,73,14";

        public bool clearDeadPlants { get; set; } = true;

        public bool smartReplantingEnabled { get; set; } = true;
    }
}
