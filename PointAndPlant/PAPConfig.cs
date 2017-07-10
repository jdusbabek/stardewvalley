using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointAndPlant
{
    public class PAPConfig : Config
    {

        public string plowKey { get; set; }
        public string plantKey { get; set; }
        public string growKey { get; set; }
        public string harvestKey { get; set; }

        public int plantRadius { get; set; }
        public int growRadius { get; set; }
        public int harvestRadius { get; set; }
        public int plowWidth { get; set; }
        public int plowHeight { get; set; }

        public bool plowEnabled { get; set; }
        public bool plantEnabled { get; set; }
        public bool growEnabled { get; set; }
        public bool harvestEnabled { get; set; }

        public int fertilizer { get; set; }

        public bool loggingEnabled { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            plowKey = "Z";
            plantKey = "A";
            growKey = "S";
            harvestKey = "D";

            plantRadius = 4;
            growRadius = 4;
            harvestRadius = 4;

            plowWidth = 3;
            plowHeight = 6;

            plowEnabled = true;
            plantEnabled = true;
            growEnabled = true;
            harvestEnabled = true;

            fertilizer = 0;

            loggingEnabled = false;

            return this as T;
        }

    }
}
