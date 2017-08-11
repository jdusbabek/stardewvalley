using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;

namespace CrabNet
{
    internal class DialogManager
    {
        /*********
        ** Properties
        *********/
        private readonly CrabNetConfig Config;


        /*********
        ** Public methods
        *********/
        public DialogManager(CrabNetConfig config)
        {
            this.Config = config;
        }

        /**
         * Performs a string replacement of certain variables inside text strings, allowing
         * the dialog to use elements of the actual situation.
         */
        public string PerformReplacement(string message, CrabNetStats stats)
        {
            String retVal = message.Replace("%%numTotal%%", stats.numTotal.ToString());
            retVal = retVal.Replace("%%numChecked%%", stats.numTotal.ToString());
            retVal = retVal.Replace("%%numEmptied%%", stats.numEmptied.ToString());
            retVal = retVal.Replace("%%numBaited%%", stats.numBaited.ToString());
            retVal = retVal.Replace("%%notChecked%%", stats.notChecked.ToString());
            retVal = retVal.Replace("%%notEmptied%%", stats.notEmptied.ToString());
            retVal = retVal.Replace("%%notBaited%%", stats.notBaited.ToString());
            retVal = retVal.Replace("%%runningTotal%%", stats.runningTotal.ToString());
            if (Game1.player.isMarried())
                retVal = retVal.Replace("%%spouse%%", Game1.player.getSpouse().getName());
            else
                retVal = retVal.Replace("%%spouse%%", this.Config.whoChecks);

            return retVal;
        }

        /**
         * Gets a set of dialogue strings that are identified in the source document by an index
         * in the format indexGroup_number.  The numbers should be unique within each index group.
         */
        public Dictionary<int, string> GetDialog(string identifier, Dictionary<string, string> source)
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
                        Log.Error("Malformed dialog string encountered. Ensure key is in the form of indexGroup_number:, where 'number' is unique within its indexGroup.");
                    }
                }
                else
                {
                    Log.Error("Malformed dialog string encountered. Ensure key is in the form of indexGroup_number:, where 'number' is unique within its indexGroup.");
                }
            }

            return result;
        }
    }
}
