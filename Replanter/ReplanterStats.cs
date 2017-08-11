namespace Replanter
{
    internal class ReplanterStats
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

        public int totalCost { get { return farmhandCost + runningSeedCost; } set {; } }
        public int numUnharvested { get { return totalCrops - cropsHarvested; } set {; } }


        /*********
        ** Public methods
        *********/
        public bool hasUnfinishedBusiness()
        {
            int tot = 0;

            return (1 < 0);
        }

    }
}
