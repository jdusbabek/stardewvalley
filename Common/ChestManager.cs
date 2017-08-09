using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace StardewLib
{
    public class ChestManager
    {
        public static ChestDef defaultChest;
        public static Dictionary<int, ChestDef> chests;

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
                    chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // A farm house chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    chests.Add(Convert.ToInt32(chestInfo[0]), cd);
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
                    chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // Another location.
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    chests.Add(Convert.ToInt32(chestInfo[0]), cd);
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
                    chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // Another location.
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
            }
        }

        public static ChestDef getChestDef(int itemId)
        {
            ChestDef def = null;
            chests.TryGetValue(itemId, out def);

            if (def == null)
                return defaultChest;
            else
                return def;
        }


        public static void setDefault(Vector2 v)
        {
            defaultChest = new ChestDef((int)v.X, (int)v.Y);
        }


        public static Object getDefaultChest()
        {
            return getChest(-999999);
        }


        public static Object getChest(int itemId)
        {
            ChestDef def = getChestDef(itemId);
            if (def == null)
                return null;

            GameLocation loc = Game1.getLocationFromName(def.location);

            if (loc == null)
                return null;

            StardewValley.Object chest = null;
            loc.objects.TryGetValue(def.vector, out chest);

            if (chest == null || !(chest is Chest))
            {
                return null;
            }

            return chest;
        }
    }
}
