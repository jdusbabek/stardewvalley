using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace StardewLib
{
    internal class DialogueManager
    {
        private static LocalizedContentManager content;
        private static Dictionary<string, Dictionary<int, string>> dialogueLookups = new Dictionary<string, Dictionary<int, string>>();
        private static Dictionary<string, string> allmessages = new Dictionary<string, string>();
        public static IConfig config;

        private static Random random = new Random();

        public static void initialize(IServiceProvider provider, string path)
        {
            DialogueManager.content = new LocalizedContentManager(provider, path);
        }

        public static string performReplacement(string message, IStats stats, IConfig config)
        {
            String retVal = message;

            FieldInfo[] fields = stats.getFieldList();

            foreach (FieldInfo field in fields)
            {
                retVal = retVal.Replace(("%%" + field.Name + "%%"), field.GetValue(stats).ToString());
            }

            retVal = retVal.Replace("%%checker%%", config.WhoChecks);
            if (Game1.player.isMarried())
                retVal = retVal.Replace("%%spouse%%", Game1.player.getSpouse().getName());
            else
                retVal = retVal.Replace("%%spouse%%", config.WhoChecks);

            return retVal;
        }


        private static Dictionary<int, string> readDialogue(string identifier)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (KeyValuePair<string, string> msgPair in (Dictionary<string, string>)allmessages)
            {
                if (msgPair.Key.Contains("_"))
                {
                    string[] nameid = msgPair.Key.Split('_');
                    if (nameid.Length == 2)
                    {
                        if (nameid[0] == identifier)
                        {
                            //Log.INFO("Adding to " + identifier + ": " + nameid[1] + ">" + msgPair.Value);
                            result.Add(Convert.ToInt32(nameid[1]), msgPair.Value);
                        }
                    }
                    else
                    {
                        Log.force_ERROR((object)"Malformed dialog string encountered. Ensure key is in the form of indexGroup_number:, where 'number' is unique within its indexGroup.");
                    }
                }
                else
                {
                    Log.force_ERROR((object)"Malformed dialog string encountered. Ensure key is in the form of indexGroup_number:, where 'number' is unique within its indexGroup.");
                }
            }

            if (identifier.Equals("smalltalk"))
            {
                Dictionary<int, string> characterDialog = DialogueManager.readDialogue(config.WhoChecks);

                if (characterDialog.Count > 0)
                {
                    int index = result.Count + 1;
                    foreach (KeyValuePair<int, string> d in characterDialog)
                    {
                        result.Add(index, d.Value);
                        index++;
                    }
                }
            }

            dialogueLookups.Add(identifier, result);

            return result;
        }


        public static string getRandomMessage(string messageStoreName)
        {
            string value = "";

            Dictionary<int, string> messagePool = null;
            dialogueLookups.TryGetValue(messageStoreName, out messagePool);

            if (messagePool == null)
            {
                messagePool = readDialogue(messageStoreName);
            }
            else if (messagePool.Count == 0)
            {
                return "...$h#$e#";
            }

            int rand = random.Next(1, messagePool.Count + 1);
            messagePool.TryGetValue(rand, out value);

            if (value == null)
            {
                return "...$h#$e#";
            }
            else
            {
                return value;
            }
        }


        public static string getMessageAt(int index, string messageStoreName)
        {
            Dictionary<int, string> messagePool = null;
            dialogueLookups.TryGetValue(messageStoreName, out messagePool);

            if (messagePool == null)
            {
                messagePool = readDialogue(messageStoreName);
            }
            else if (messagePool.Count == 0)
            {
                return "...$h#$e#";
            }
            else if (messagePool.Count < index)
            {
                return "...$h#$e#";
            }

            Log.INFO("[jwdred-StardewLib] Returning message " + index + ": " + messagePool[index]);
            return messagePool[index];
            //return messagePool.ElementAt(index).Value;
        }


        /**
         * Loads the dialog.xnb file and sets up each of the dialog lookup files.
         */
        public static void readInMessages()
        {
            //Dictionary<int, string> objects = Game1.content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
            try
            {
                allmessages = DialogueManager.content.Load<Dictionary<string, string>>("dialog");
            }
            catch (Exception ex)
            {
                Log.force_ERROR((object)("[jwdred-StardewLib] Exception loading content:" + ex.ToString()));
            }
        }
    }
}
