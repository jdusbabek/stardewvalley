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
        /*********
        ** Properties
        *********/
        private readonly Log Log;
        private ChestDef DefaultChest;
        private Dictionary<int, ChestDef> Chests;


        /*********
        ** Public methods
        *********/
        public ChestManager(Log log)
        {
            this.Log = log;
        }

        public void ParseChests(string chestString)
        {
            this.Chests = new Dictionary<int, ChestDef>();

            string[] chestDefinitions = chestString.Split('|');

            foreach (string def in chestDefinitions)
            {
                string[] chestInfo = def.Split(',');
                if (chestInfo.Length == 3)
                {
                    // A Farm chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), "Farm");
                    this.Chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // A farm house chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    this.Chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
            }
        }

        public void ParseChests(string[] chestList)
        {
            this.Chests = new Dictionary<int, ChestDef>();

            foreach (string def in chestList)
            {
                string[] chestInfo = def.Split(',');
                if (chestInfo.Length == 3)
                {
                    // A Farm chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), "Farm");
                    this.Chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // Another location.
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    this.Chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
            }
        }

        public void ParseChests(List<string> chestList)
        {
            this.Chests = new Dictionary<int, ChestDef>();

            foreach (string def in chestList)
            {
                string[] chestInfo = def.Split(',');
                if (chestInfo.Length == 3)
                {
                    // A Farm chest
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), "Farm");
                    this.Chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
                else if (chestInfo.Length == 4)
                {
                    // Another location.
                    ChestDef cd = new ChestDef(Convert.ToInt32(chestInfo[1]), Convert.ToInt32(chestInfo[2]), chestInfo[3]);
                    this.Chests.Add(Convert.ToInt32(chestInfo[0]), cd);
                    Log.INFO("Parsing chest: " + cd.x + "," + cd.y + " for item: " + chestInfo[0] + ", location: " + cd.location);
                }
            }
        }

        public void SetDefault(Vector2 v)
        {
            this.DefaultChest = new ChestDef((int)v.X, (int)v.Y);
        }


        public Object GetDefaultChest()
        {
            return this.GetChest(-999999);
        }

        public Object GetChest(int itemId)
        {
            ChestDef def = this.GetChestDef(itemId);
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


        /*********
        ** Private methods
        *********/
        private ChestDef GetChestDef(int itemId)
        {
            ChestDef def;
            this.Chests.TryGetValue(itemId, out def);

            if (def == null)
                return this.DefaultChest;
            else
                return def;
        }
    }
}
