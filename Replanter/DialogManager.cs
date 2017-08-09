using System;
using System.Collections.Generic;
using StardewValley;

namespace Replanter
{
    internal class DialogManager
    {
        public static ReplanterConfig config;

        /**
         * Performs a string replacement of certain variables inside text strings, allowing
         * the dialog to use elements of the actual situation.
         */
        public static string performReplacement(string message, ReplanterStats stats)
        {

            String retVal = message.Replace("%%cropsHarvested%%", stats.cropsHarvested.ToString());
            retVal = retVal.Replace("%%runningSeedCost%%", stats.runningSeedCost.ToString());
            retVal = retVal.Replace("%%runningSellPrice%%", stats.runningSellPrice.ToString());
            retVal = retVal.Replace("%%runningLaborCost%%", stats.farmhandCost.ToString());
            retVal = retVal.Replace("%%totalCrops%%", stats.totalCrops.ToString());
            retVal = retVal.Replace("%%cropsWatered%%", stats.cropsWatered.ToString());
            retVal = retVal.Replace("%%totalCost%%", stats.totalCost.ToString());
            retVal = retVal.Replace("%%numUnharvested%%", stats.numUnharvested.ToString());
            retVal = retVal.Replace("%%checker%%", config.whoChecks);
            if (Game1.player.isMarried())
                retVal = retVal.Replace("%%spouse%%", Game1.player.getSpouse().getName());
            else
                retVal = retVal.Replace("%%spouse%%", config.whoChecks);

            return retVal;
        }

        /**
         * Gets a set of dialogue strings that are identified in the source document by an index
         * in the format indexGroup_number.  The numbers should be unique within each index group.
         */
        public static Dictionary<int, string> getDialog(string identifier, Dictionary<string, string> source)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (KeyValuePair<string, string> msgPair in source)
            {
                if (msgPair.Key.Contains("_"))
                {
                    string[] nameid = msgPair.Key.Split('_');
                    if (nameid.Length == 2)
                    {
                        if (nameid[0] == identifier)
                        {
                            result.Add(Convert.ToInt32(nameid[1]), msgPair.Value);
                        }
                    }
                    else
                    {
                        Log.force_ERROR("Malformed dialog string encountered. Ensure key is in the form of indexGroup_number:, where 'number' is unique within its indexGroup.");
                    }
                }
                else
                {
                    Log.force_ERROR("Malformed dialog string encountered. Ensure key is in the form of indexGroup_number:, where 'number' is unique within its indexGroup.");
                }
            }

            return result;
        }
    }
}
