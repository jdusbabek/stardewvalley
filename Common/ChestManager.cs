using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace StardewLib
{
    internal class ChestManager
    {
        private static ChestDef defaultChest;
        private static Dictionary<int, ChestDef> chests;

        public static void parseChests(string chestString)
        {
            ChestManager.chests = new Dictionary<int, ChestDef>();

            string[] chestDefinitions = chestString.Split('|');

            foreach (string def in chestDefinitions)
            {
                string[] chestInfo = def.Split(',');
                if (chestInfo.Length == 3)
                {
                    // A Farm chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), "Farm");
                    ChestManager.chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // A farm house chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    ChestManager.chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
            }
        }

        public static void parseChests(string[] chestList)
        {
            ChestManager.chests = new Dictionary<int, ChestDef>();

            foreach (string def in chestList)
            {
                string[] chestInfo = def.Split(',');
                if (chestInfo.Length == 3)
                {
                    // A Farm chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), "Farm");
                    ChestManager.chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // Another location.
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    ChestManager.chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
            }
        }

        public static void parseChests(List<string> chestList)
        {
            ChestManager.chests = new Dictionary<int, ChestDef>();

            foreach (string def in chestList)
            {
                string[] chestInfo = def.Split(',');
                if (chestInfo.Length == 3)
                {
                    // A Farm chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), "Farm");
                    ChestManager.chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // Another location.
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    ChestManager.chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
            }
        }

        private static ChestDef getChestDef(int itemId)
        {
            ChestDef def = null;
            ChestManager.chests.TryGetValue(itemId, out def);

            if (def == null)
                return ChestManager.defaultChest;
            else
                return def;
        }


        public static void setDefault(Vector2 v)
        {
            ChestManager.defaultChest = new ChestDef((int)v.X, (int)v.Y);
        }


        public static Object getDefaultChest()
        {
            return ChestManager.getChest(-999999);
        }


        public static Object getChest(int itemId)
        {
            ChestDef def = ChestManager.getChestDef(itemId);
            if (def == null)
                return null;

            GameLocation loc = Game1.getLocationFromName(def.location);

            if (loc == null)
                return null;

            Object chest;
            loc.objects.TryGetValue(def.vector, out chest);

            if (chest == null || !(chest is Chest))
            {
                return null;
            }

            return chest;
        }
    }
}
