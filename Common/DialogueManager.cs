﻿using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace StardewLib
{
    internal class DialogueManager
    {
        /*********
        ** Properties
        *********/
        private readonly LocalizedContentManager Content;
        private readonly Dictionary<string, Dictionary<int, string>> DialogueLookups = new Dictionary<string, Dictionary<int, string>>();
        private readonly IConfig Config;
        private readonly Random Random = new Random();
        private Log Log;
        private Dictionary<string, string> AllMessages = new Dictionary<string, string>();


        /*********
        ** Public methods
        *********/
        public DialogueManager(IConfig config, IServiceProvider provider, string path, Log log)
        {
            this.Config = config;
            this.Content = new LocalizedContentManager(provider, path);
            this.Log = log;
        }

        public string PerformReplacement(string message, IStats stats, IConfig config)
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

        public string GetRandomMessage(string messageStoreName)
        {
            string value = "";

            Dictionary<int, string> messagePool = null;
            this.DialogueLookups.TryGetValue(messageStoreName, out messagePool);

            if (messagePool == null)
            {
                messagePool = readDialogue(messageStoreName);
            }
            else if (messagePool.Count == 0)
            {
                return "...$h#$e#";
            }

            int rand = this.Random.Next(1, messagePool.Count + 1);
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

        public string GetMessageAt(int index, string messageStoreName)
        {
            Dictionary<int, string> messagePool = null;
            this.DialogueLookups.TryGetValue(messageStoreName, out messagePool);

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
        public void ReadInMessages()
        {
            //Dictionary<int, string> objects = Game1.content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
            try
            {
                this.AllMessages = this.Content.Load<Dictionary<string, string>>("dialog");
            }
            catch (Exception ex)
            {
                Log.force_ERROR((object)("[jwdred-StardewLib] Exception loading content:" + ex.ToString()));
            }
        }


        /*********
        ** Private methods
        *********/
        private Dictionary<int, string> readDialogue(string identifier)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (KeyValuePair<string, string> msgPair in this.AllMessages)
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
                Dictionary<int, string> characterDialog = this.readDialogue(this.Config.WhoChecks);

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

            this.DialogueLookups.Add(identifier, result);

            return result;
        }
    }
}
