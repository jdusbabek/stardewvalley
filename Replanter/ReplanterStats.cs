using System.Collections.Generic;
using System.Linq;
using StardewLib;

namespace Replanter
{
    internal class ReplanterStats : IStats
    {
        /*********
        ** Accessors
        *********/
        public int cropsHarvested { get; set; }
        public int runningSeedCost { get; set; }
        public int runningSellPrice { get; set; }
        public int farmhandCost { get; set; }
        public int totalCrops { get; set; }
        public int cropsWatered { get; set; }
        public int plantsCleared { get; set; }

        public int totalCost { get { return farmhandCost + runningSeedCost; } }
        public int numUnharvested { get { return totalCrops - cropsHarvested; } }


        /*********
        ** Public methods
        *********/
        public bool hasUnfinishedBusiness()
        {
            int tot = 0;

            return (1 < 0);
        }

        public IDictionary<string, object> GetFields()
        {
            IDictionary<string, object> fields = typeof(ReplanterStats)
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(this));
            fields["runningLaborCost"] = this.farmhandCost;
            return fields;
        }
    }
}
