using System.Collections.Generic;
using System.Linq;
using StardewLib;

namespace CrabNet
{
    internal class CrabNetStats : IStats
    {
        /*********
        ** Accessors
        *********/
        // The total number of crab pots that are placed
        public int numTotal { get; set; }

        // The number of crab pots that were successfully checked
        public int numChecked { get; set; }

        // The number of crab pots that were successfully emptied
        public int numEmptied { get; set; }

        // The number of crab pots that were successfully baited
        public int numBaited { get; set; }

        // The number of crab pots that were completed.  Possibly deprecated, check for occurences and delete.
        public int numCompleted { get; set; }

        // The number of crab pots that were traversed, but not checked.
        public int notChecked { get; set; }

        // The number of crab pots that were not emptied.
        public int notEmptied { get; set; }

        // The number of crab pots that were not baited.
        public int notBaited { get; set; }

        // The number of crab pots that had nothing to retrieve.  Possibly deprecated, check for occurrences and delete.
        public int nothingToRetrieve { get; set; }

        // The number of crab pots that did not need to be baited.  Possibly deprecated, check for occurences and delete.
        public int nothingToBait { get; set; }

        // A running total of the costs, used to check for "can afford" while waiting to deduct costs until the end.
        public int runningTotal { get; set; }


        /*********
        ** Public methods
        *********/
        // Whether all pots were checked, emptied, and baited.
        public bool hasUnfinishedBusiness()
        {
            int tot = (numBaited + nothingToBait) + (numEmptied + nothingToRetrieve) + numChecked;

            return (tot != (numTotal * 3));
        }

        public IDictionary<string, object> GetFields()
        {
            IDictionary<string, object> fields = typeof(CrabNetStats)
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(this));

            // TODO: fix this bug (carried over from previous code)
            fields["numChecked"] = this.numTotal;

            return fields;
        }
    }
}
