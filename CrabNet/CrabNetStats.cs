namespace CrabNet
{
    internal class CrabNetStats
    {
        // The total number of crab pots that are placed
        public int numTotal = 0;

        // The number of crab pots that were successfully checked
        public int numChecked = 0;

        // The number of crab pots that were successfully emptied
        public int numEmptied = 0;

        // The number of crab pots that were successfully baited
        public int numBaited = 0;

        // The number of crab pots that were completed.  Possibly deprecated, check for occurences and delete.
        public int numCompleted = 0;

        // The number of crab pots that were traversed, but not checked.
        public int notChecked = 0;

        // The number of crab pots that were not emptied.
        public int notEmptied = 0;

        // The number of crab pots that were not baited.
        public int notBaited = 0;

        // The number of crab pots that had nothing to retrieve.  Possibly deprecated, check for occurrences and delete.
        public int nothingToRetrieve = 0;

        // The number of crab pots that did not need to be baited.  Possibly deprecated, check for occurences and delete.
        public int nothingToBait = 0;

        // A running total of the costs, used to check for "can afford" while waiting to deduct costs until the end.
        public int runningTotal = 0;


        // Whether all pots were checked, emptied, and baited.
        public bool hasUnfinishedBusiness()
        {
            int tot = (numBaited + nothingToBait) + (numEmptied + nothingToRetrieve) + numChecked;

            return (tot != (numTotal * 3));
        }
    }
}
