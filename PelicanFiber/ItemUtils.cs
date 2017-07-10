using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using xTile.Dimensions;
using Object = StardewValley.Object;
using StardewValley;
using StardewModdingAPI;
using StardewLib;
using Log = StardewLib.Log;

namespace PelicanFiber
{
    public class ItemUtils
    {


        public static List<Item> getShopStock(bool Pierres, bool unfiltered = false)
        {
            List<Item> objList1 = new List<Item>();
            if (Pierres)
            {
                if (Game1.currentSeason.Equals("spring") || unfiltered)
                {
                    objList1.Add((Item)new Object(Vector2.Zero, 472, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 473, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 474, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 475, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 427, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 429, int.MaxValue));
                    objList1.Add((Item)new Object(745, int.MaxValue, false, 100, 0));
                    objList1.Add((Item)new Object(Vector2.Zero, 477, int.MaxValue));
                    objList1.Add((Item)new Object(628, int.MaxValue, false, 1700, 0));
                    objList1.Add((Item)new Object(629, int.MaxValue, false, 1000, 0));
                    if (Game1.year > 1 || unfiltered)
                        objList1.Add((Item)new Object(Vector2.Zero, 476, int.MaxValue));
                }
                if (Game1.currentSeason.Equals("summer") || unfiltered)
                {
                    objList1.Add((Item)new Object(Vector2.Zero, 480, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 482, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 483, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 484, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 479, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 302, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 453, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 455, int.MaxValue));
                    objList1.Add((Item)new Object(630, int.MaxValue, false, 2000, 0));
                    objList1.Add((Item)new Object(631, int.MaxValue, false, 3000, 0));
                    if (Game1.year > 1 || unfiltered)
                        objList1.Add((Item)new Object(Vector2.Zero, 485, int.MaxValue));
                }
                if (Game1.currentSeason.Equals("fall") || unfiltered)
                {
                    objList1.Add((Item)new Object(Vector2.Zero, 487, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 488, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 490, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 299, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 301, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 492, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 491, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 493, int.MaxValue));
                    objList1.Add((Item)new Object(431, int.MaxValue, false, 100, 0));
                    objList1.Add((Item)new Object(Vector2.Zero, 425, int.MaxValue));
                    objList1.Add((Item)new Object(632, int.MaxValue, false, 3000, 0));
                    objList1.Add((Item)new Object(633, int.MaxValue, false, 2000, 0));
                    if (Game1.year > 1 || unfiltered)
                        objList1.Add((Item)new Object(Vector2.Zero, 489, int.MaxValue));
                }

                objList1.Add((Item)new Object(Vector2.Zero, 297, int.MaxValue));
                objList1.Add((Item)new Object(Vector2.Zero, 245, int.MaxValue));
                objList1.Add((Item)new Object(Vector2.Zero, 246, int.MaxValue));
                objList1.Add((Item)new Object(Vector2.Zero, 423, int.MaxValue));

                Random random = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
                List<Item> objList2 = objList1;
                Wallpaper wallpaper1 = new Wallpaper(random.Next(112), false);
                wallpaper1.stack = int.MaxValue;
                Wallpaper wallpaper2 = wallpaper1;
                objList2.Add((Item)wallpaper2);
                List<Item> objList3 = objList1;
                Wallpaper wallpaper3 = new Wallpaper(random.Next(40), true);
                wallpaper3.stack = int.MaxValue;
                Wallpaper wallpaper4 = wallpaper3;
                objList3.Add((Item)wallpaper4);
                if (Game1.player.achievements.Contains(38))
                    objList1.Add((Item)new Object(Vector2.Zero, 458, int.MaxValue));
            }
            else
            {
                if (Game1.currentSeason.Equals("spring") || unfiltered)
                    objList1.Add((Item)new Object(Vector2.Zero, 478, int.MaxValue));
                if (Game1.currentSeason.Equals("summer") || unfiltered)
                {
                    objList1.Add((Item)new Object(Vector2.Zero, 486, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 481, int.MaxValue));
                }
                if (Game1.currentSeason.Equals("fall") || unfiltered)
                {
                    objList1.Add((Item)new Object(Vector2.Zero, 493, int.MaxValue));
                    objList1.Add((Item)new Object(Vector2.Zero, 494, int.MaxValue));
                }
                objList1.Add((Item)new Object(Vector2.Zero, 88, int.MaxValue));
                objList1.Add((Item)new Object(Vector2.Zero, 90, int.MaxValue));
            }
            return objList1;
        }


        public static List<Item> getCarpenterStock(bool unfiltered = false)
        {
            List<Item> stock = new List<Item>();
            stock.Add((Item)new Object(Vector2.Zero, 388, int.MaxValue));
            stock.Add((Item)new Object(Vector2.Zero, 390, int.MaxValue));
            Random r = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            stock.Add((Item)new Furniture(1614, Vector2.Zero));
            stock.Add((Item)new Furniture(1616, Vector2.Zero));

            if (unfiltered)
            {
                stock.AddRange(getAllFurniture());
            }
            else
            {
                switch (Game1.dayOfMonth % 7)
                {
                    case 0:
                        stock.Add((Item)getRandomFurniture(r, stock, 1296, 1391));
                        stock.Add((Item)getRandomFurniture(r, stock, 416, 537));
                        break;
                    case 1:
                        stock.Add((Item)new Furniture(0, Vector2.Zero));
                        stock.Add((Item)new Furniture(192, Vector2.Zero));
                        stock.Add((Item)new Furniture(704, Vector2.Zero));
                        stock.Add((Item)new Furniture(1120, Vector2.Zero));
                        stock.Add((Item)new Furniture(1216, Vector2.Zero));
                        stock.Add((Item)new Furniture(1391, Vector2.Zero));
                        break;
                    case 2:
                        stock.Add((Item)new Furniture(3, Vector2.Zero));
                        stock.Add((Item)new Furniture(197, Vector2.Zero));
                        stock.Add((Item)new Furniture(709, Vector2.Zero));
                        stock.Add((Item)new Furniture(1122, Vector2.Zero));
                        stock.Add((Item)new Furniture(1218, Vector2.Zero));
                        stock.Add((Item)new Furniture(1393, Vector2.Zero));
                        break;
                    case 3:
                        stock.Add((Item)new Furniture(6, Vector2.Zero));
                        stock.Add((Item)new Furniture(202, Vector2.Zero));
                        stock.Add((Item)new Furniture(714, Vector2.Zero));
                        stock.Add((Item)new Furniture(1124, Vector2.Zero));
                        stock.Add((Item)new Furniture(1220, Vector2.Zero));
                        stock.Add((Item)new Furniture(1395, Vector2.Zero));
                        break;
                    case 4:
                        stock.Add((Item)getRandomFurniture(r, stock, 1296, 1391));
                        stock.Add((Item)getRandomFurniture(r, stock, 1296, 1391));
                        break;
                    case 5:
                        stock.Add((Item)getRandomFurniture(r, stock, 1443, 1450));
                        stock.Add((Item)getRandomFurniture(r, stock, 288, 313));
                        break;
                    case 6:
                        stock.Add((Item)getRandomFurniture(r, stock, 1565, 1607));
                        stock.Add((Item)getRandomFurniture(r, stock, 12, 129));
                        break;
                }
                stock.Add((Item)getRandomFurniture(r, stock, 0, 1462));
                stock.Add((Item)getRandomFurniture(r, stock, 0, 1462));
                while (r.NextDouble() < 0.25)
                    stock.Add((Item)getRandomFurniture(r, stock, 1673, 1815));
                stock.Add((Item)new Furniture(1402, Vector2.Zero));
                stock.Add((Item)new TV(1466, Vector2.Zero));
                stock.Add((Item)new TV(1680, Vector2.Zero));
                if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
                    stock.Add((Item)new TV(1468, Vector2.Zero));
                if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
                    stock.Add((Item)new Furniture(1226, Vector2.Zero));
            }

            if (!Game1.player.craftingRecipes.ContainsKey("Wooden Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 143, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            else if (!Game1.player.craftingRecipes.ContainsKey("Stone Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 144, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            else if (!Game1.player.craftingRecipes.ContainsKey("Barrel Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 150, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            else if (!Game1.player.craftingRecipes.ContainsKey("Stump Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 147, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            else if (!Game1.player.craftingRecipes.ContainsKey("Gold Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 145, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            else if (!Game1.player.craftingRecipes.ContainsKey("Carved Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 148, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            else if (!Game1.player.craftingRecipes.ContainsKey("Skull Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 149, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            else if (!Game1.player.craftingRecipes.ContainsKey("Marble Brazier"))
            {
                List<Item> objList = stock;
                Torch torch1 = new Torch(Vector2.Zero, 151, true);
                torch1.isRecipe = true;
                Torch torch2 = torch1;
                objList.Add((Item)torch2);
            }
            if (!Game1.player.craftingRecipes.ContainsKey("Wood Lamp-post"))
                stock.Add((Item)new Object(Vector2.Zero, 152, true)
                {
                    isRecipe = true
                });
            if (!Game1.player.craftingRecipes.ContainsKey("Iron Lamp-post"))
                stock.Add((Item)new Object(Vector2.Zero, 153, true)
                {
                    isRecipe = true
                });
            if (!Game1.player.craftingRecipes.ContainsKey("Wood Floor"))
                stock.Add((Item)new Object(328, 1, true, 50, 0));
            if (!Game1.player.craftingRecipes.ContainsKey("Stone Floor"))
                stock.Add((Item)new Object(329, 1, true, 50, 0));
            if (!Game1.player.craftingRecipes.ContainsKey("Stepping Stone Path"))
                stock.Add((Item)new Object(415, 1, true, 50, 0));
            if (!Game1.player.craftingRecipes.ContainsKey("Straw Floor"))
                stock.Add((Item)new Object(401, 1, true, 100, 0));
            if (!Game1.player.craftingRecipes.ContainsKey("Crystal Path"))
                stock.Add((Item)new Object(409, 1, true, 100, 0));
            return stock;
        }


        private static bool isFurnitureOffLimitsForSale(int index)
        {
            switch (index)
            {
                case 1669:
                case 1671:
                case 1680:
                case 1733:
                case 1541:
                case 1545:
                case 1554:
                case 1298:
                case 1299:
                case 1300:
                case 1301:
                case 1302:
                case 1303:
                case 1304:
                case 1305:
                case 1306:
                case 1307:
                case 1308:
                case 1402:
                case 1466:
                case 1468:
                case 131:
                case 1226:
                    return true;
                default:
                    return false;
            }
        }


        public static List<Item> getAllFurniture()
        {
            List<Item> list = new List<Item>();

            foreach (KeyValuePair<int, string> keyValuePair in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture"))
            {
                if (!isFurnitureOffLimitsForSale(keyValuePair.Key))
                    list.Add((Item)new Furniture(keyValuePair.Key, Vector2.Zero));
            }

            return list;
        }

        public static Dictionary<Item, int[]> getAllFurnituresForFree()
        {
            Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
            foreach (KeyValuePair<int, string> keyValuePair in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture"))
            {
                if (!isFurnitureOffLimitsForSale(keyValuePair.Key))
                    dictionary.Add((Item)new Furniture(keyValuePair.Key, Vector2.Zero), new int[2] { 0, int.MaxValue });
            }
            dictionary.Add((Item)new Furniture(1402, Vector2.Zero), new int[2] { 0, int.MaxValue });
            dictionary.Add((Item)new TV(1680, Vector2.Zero), new int[2] { 0, int.MaxValue });
            dictionary.Add((Item)new TV(1466, Vector2.Zero), new int[2] { 0, int.MaxValue });
            dictionary.Add((Item)new TV(1468, Vector2.Zero), new int[2] { 0, int.MaxValue });
            return dictionary;
        }


        private static Furniture getRandomFurniture(Random r, List<Item> stock, int lowerIndexBound = 0, int upperIndexBound = 1462)
        {
            Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Furniture");
            int num;
            do
            {
                num = r.Next(lowerIndexBound, upperIndexBound);
                if (stock != null)
                {
                    foreach (Item obj in stock)
                    {
                        if (obj is Furniture && obj.parentSheetIndex == num)
                            num = -1;
                    }
                }
            }
            while (isFurnitureOffLimitsForSale(num) || !dictionary.ContainsKey(num));
            Furniture furniture = new Furniture(num, Vector2.Zero);
            furniture.stack = int.MaxValue;
            return furniture;
        }


        public static Dictionary<Item, int[]> getBlacksmithStock(bool unfiltered = false)
        {
            if (unfiltered)
            {
                return new Dictionary<Item, int[]>() {
                { (Item)new Object(Vector2.Zero, 378, int.MaxValue), new int[2] { 75, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 380, int.MaxValue), new int[2] { 150, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 382, int.MaxValue), new int[2] { 150, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 384, int.MaxValue), new int[2] { 400, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 386, int.MaxValue), new int[2] { 400, int.MaxValue } },

                { (Item)new Object(Vector2.Zero, 334, int.MaxValue), new int[2] { 150, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 335, int.MaxValue), new int[2] { 250, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 336, int.MaxValue), new int[2] { 750, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 337, int.MaxValue), new int[2] { 2000, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 338, int.MaxValue), new int[2] { 100, int.MaxValue } }
            };
            }
            else
            {
                return new Dictionary<Item, int[]>() {
                { (Item)new Object(Vector2.Zero, 378, int.MaxValue), new int[2] { 75, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 380, int.MaxValue), new int[2] { 150, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 382, int.MaxValue), new int[2] { 150, int.MaxValue } },
                { (Item)new Object(Vector2.Zero, 384, int.MaxValue), new int[2] { 400, int.MaxValue } }
            };
            }
            
        }


        public static Dictionary<Item, int[]> getFishShopStock(Farmer who, bool unfiltered = false)
        {
            Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
            dictionary.Add((Item)new Object(219, 1, false, -1, 0), new int[2]
            {
        250,
        int.MaxValue
            });
            if (Game1.player.fishingLevel >= 2 || unfiltered)
                dictionary.Add((Item)new Object(685, 1, false, -1, 0), new int[2] { 5, int.MaxValue });
            if (Game1.player.fishingLevel >= 3 || unfiltered)
                dictionary.Add((Item)new Object(710, 1, false, -1, 0), new int[2]
                {
          1500,
          int.MaxValue
                });
            if (Game1.player.fishingLevel >= 6 || unfiltered)
            {
                dictionary.Add((Item)new Object(686, 1, false, -1, 0), new int[2]
                {
          500,
          int.MaxValue
                });
                dictionary.Add((Item)new Object(694, 1, false, -1, 0), new int[2]
                {
          500,
          int.MaxValue
                });
                dictionary.Add((Item)new Object(692, 1, false, -1, 0), new int[2]
                {
          200,
          int.MaxValue
                });
            }
            if (Game1.player.fishingLevel >= 7 || unfiltered)
            {
                dictionary.Add((Item)new Object(693, 1, false, -1, 0), new int[2]
                {
          750,
          int.MaxValue
                });
                dictionary.Add((Item)new Object(695, 1, false, -1, 0), new int[2]
                {
          750,
          int.MaxValue
                });
            }
            if (Game1.player.fishingLevel >= 8 || unfiltered)
            {
                dictionary.Add((Item)new Object(691, 1, false, -1, 0), new int[2]
                {
          1000,
          int.MaxValue
                });
                dictionary.Add((Item)new Object(687, 1, false, -1, 0), new int[2]
                {
          1000,
          int.MaxValue
                });
            }
            if (Game1.player.fishingLevel >= 9 || unfiltered)
                dictionary.Add((Item)new Object(703, 1, false, -1, 0), new int[2]
                {
          1000,
          int.MaxValue
                });
            dictionary.Add((Item)new FishingRod(0), new int[2]
            {
        500,
        int.MaxValue
            });
            if (Game1.player.fishingLevel >= 2 || unfiltered)
                dictionary.Add((Item)new FishingRod(2), new int[2]
                {
          1800,
          int.MaxValue
                });
            if (Game1.player.fishingLevel >= 6 || unfiltered)
                dictionary.Add((Item)new FishingRod(3), new int[2]
                {
          7500,
          int.MaxValue
                });

            if (unfiltered)
            {
                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("Fish -4"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                        Item i = new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2, 0);

                        dictionary.Add(i, new int[2] { Convert.ToInt32(strArray[1]) * 4, int.MaxValue });
                    }
                }
            }
            
            return dictionary;
        }


        public static List<Item> getSaloonStock(bool unfiltered = false)
        {
            List<Item> objList = new List<Item>();
            objList.Add((Item)new Object(Vector2.Zero, 346, int.MaxValue));
            objList.Add((Item)new Object(Vector2.Zero, 196, int.MaxValue));
            objList.Add((Item)new Object(Vector2.Zero, 216, int.MaxValue));
            objList.Add((Item)new Object(Vector2.Zero, 224, int.MaxValue));
            objList.Add((Item)new Object(Vector2.Zero, 206, int.MaxValue));
            objList.Add((Item)new Object(Vector2.Zero, 395, int.MaxValue));

            if (Game1.dishOfTheDay.stack > 0 && !unfiltered)
                objList.Add((Item)Game1.dishOfTheDay);
            else if (unfiltered)
            {
                // 194 - 239
                for (int i = 194; i < 240; i++)
                {
                    int parentSheetIndex = i;
                    if (parentSheetIndex == 217)
                        parentSheetIndex = 216;
                    objList.Add((Item)(new Object(Vector2.Zero, parentSheetIndex, Game1.random.Next(1, 4 + (Game1.random.NextDouble() < 0.08 ? 10 : 0)))));
                }
            }

            if (!Game1.player.cookingRecipes.ContainsKey("Hashbrowns"))
                objList.Add((Item)new Object(210, 1, true, 25, 0));
            if (!Game1.player.cookingRecipes.ContainsKey("Omelet"))
                objList.Add((Item)new Object(195, 1, true, 50, 0));
            if (!Game1.player.cookingRecipes.ContainsKey("Pancakes"))
                objList.Add((Item)new Object(211, 1, true, 50, 0));
            if (!Game1.player.cookingRecipes.ContainsKey("Bread"))
                objList.Add((Item)new Object(216, 1, true, 50, 0));
            if (!Game1.player.cookingRecipes.ContainsKey("Tortilla"))
                objList.Add((Item)new Object(229, 1, true, 50, 0));
            if (!Game1.player.cookingRecipes.ContainsKey("Pizza"))
                objList.Add((Item)new Object(206, 1, true, 75, 0));
            if (!Game1.player.cookingRecipes.ContainsKey("Maki Roll"))
                objList.Add((Item)new Object(228, 1, true, 150, 0));

            return objList;
        }


        public static List<Item> getLeahShopStock(bool unfiltered = false)
        {
            List<Item> stock = new List<Item>();

            foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
            {
                if (Game1.objectInformation[keyValuePair.Key].Contains("/Basic -81"))
                {
                    string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                    Item i = new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2, 0);
                    stock.Add(i);
                }
                else if (keyValuePair.Key == 406 || keyValuePair.Key == 414 || keyValuePair.Key == 396)
                {
                    string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                    Item i = new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2, 0);
                    stock.Add(i);
                }

            }  

            return stock;
        }


        public static List<Item> getRecipesStock(bool unfiltered = false)
        {
            List<Item> stock = new List<Item>();

            if (unfiltered)
            {
                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("/Cooking -7"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                        Item i = new Object(keyValuePair.Key, int.MaxValue, true, Convert.ToInt32(strArray[1]) * 10, 0);

                        if (!Game1.player.cookingRecipes.ContainsKey(strArray[0]))
                        {
                            stock.Add(i);
                        }

                    }
                }
            }

            return stock;
        }


        public static List<Item> getMineralsAndArtifactsStock(bool unfiltered = false)
        {
            List<Item> stock = new List<Item>();

            if (unfiltered)
            {
                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("/Minerals -"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                        Item i = new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2, 0);

                        stock.Add(i);
                    }
                }

                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("/Arch/"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');

                        Item i = null;

                        if (keyValuePair.Key == 114)
                        {
                            i = new Object(keyValuePair.Key, int.MaxValue, false, 20000, 0);
                        }
                        else if (keyValuePair.Key == 96 || keyValuePair.Key == 97 || keyValuePair.Key == 98 || keyValuePair.Key == 99)
                        {
                            i = new Object(keyValuePair.Key, int.MaxValue, false, 5000, 0);
                        }
                        else
                            i = new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 3, 0);

                        stock.Add(i);
                    }
                }
            }          

            return stock;
        }


        public static List<Object> getPurchaseAnimalStock()
        {
            //string locationName = ((AnimalHouse)Game1.currentLocation).Name;
            string locationName = ((AnimalHouse)Game1.currentLocation).getBuilding().buildingType;

            return new List<Object>() {
                new Object(100, 1, false, 400, 0){ name = "Chicken", type = locationName.Equals("Coop") || locationName.Equals("Deluxe Coop") || locationName.Equals("Big Coop") ? (string)null : "You gotta be in a Coop" },
                new Object(100, 1, false, 750, 0) { name = "Dairy Cow", type = locationName.Equals("Barn") || locationName.Equals("Deluxe Barn") || locationName.Equals("Big Barn") ? (string)null : "You gotta be in a Barn" },
                new Object(100, 1, false, 2000, 0){ name = "Goat", type = locationName.Equals("Big Barn") || locationName.Equals("Deluxe Barn") ? (string)null : "You gotta be in a Big Barn" },
                new Object(100, 1, false, 2000, 0) { name = "Duck", type = locationName.Equals("Big Coop") || locationName.Equals("Deluxe Coop") ? (string)null : "You gotta be in a Big Coop" },
                new Object(100, 1, false, 4000, 0) { name = "Sheep", type = locationName.Equals("Deluxe Barn") ? (string)null : "You gotta be in a Deluxe Barn" },
                new Object(100, 1, false, 4000, 0) { name = "Rabbit", type = locationName.Equals("Deluxe Coop") ? (string)null : "You gotta be in a Deluxe Coop" },
                new Object(100, 1, false, 8000, 0){ name = "Pig", type = locationName.Equals("Deluxe Barn") ? (string)null : "You gotta be in a Deluxe Barn" } };
        }

        public static void finishAllBundles()
        {
            foreach (KeyValuePair<int, bool[]> bundle in (Dictionary<int, bool[]>)(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles)
            {
                for (int index = 0; index < bundle.Value.Length; ++index)
                    bundle.Value[index] = true;
            }
        }

        private static Dictionary<int, int> bundleToAreaDictionary;

        public static List<Item> getJunimoStock()
        {
            List<Item> junimoItems = new List<Item>();
            Dictionary<int,string> junContent = PelicanFiber.content.Load<Dictionary<int, string>>("bundles");
            Dictionary<int, bool[]> bundleInfo = (Dictionary<int, bool[]>)(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles;
            ItemUtils.bundleToAreaDictionary = new Dictionary<int, int>();

            foreach (KeyValuePair<int, string> kvp in junContent)
            {
                int id = kvp.Key;
                string[] parts = kvp.Value.Split('/');

                if (parts.Length >= 7)
                {
                    string name = parts[0];
                    int price = Int32.Parse(parts[1]);
                    int category = Int32.Parse(parts[3].Split(' ')[1]);
                    int bundleIndex = Int32.Parse(parts[4]);
                    string area = parts[5];
                    string lore = parts[6];

                    Object o = new Object(id, 1, true, price);
                    o.category = category;
                    o.name = name;
                    o.specialVariable = bundleIndex;

                    ItemUtils.bundleToAreaDictionary.Add(bundleIndex, ItemUtils.getAreaNumberFromName(area));

                    foreach (KeyValuePair<int,bool[]> bundle in bundleInfo)
                    {
                        if (bundle.Key == bundleIndex)
                        {
                            for (int index = 0; index < bundle.Value.Length; ++index)
                            {
                                if (bundle.Value[index] == false)
                                {
                                    junimoItems.Add(o);
                                    break;
                                }
                            }
                        }  
                    }   
                }
                else
                {
                    Log.force_ERROR("Parts not >= 6: " + kvp.Key + ": " + kvp.Value);
                }
            }

            return junimoItems;
        }


        public static void addBundle(int bundleId)
        {
            CommunityCenter c = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;

            // Check if the player has access to the community center
            if (! Game1.player.mailReceived.Contains("ccDoorUnlock") )
                Game1.player.mailReceived.Add("ccDoorUnlock");        
            if (!Game1.player.mailReceived.Contains("seenJunimoNote"))
            {
                Game1.player.removeQuest(26);
                Game1.player.mailReceived.Add("seenJunimoNote");
            }
            if (!Game1.player.mailReceived.Contains("wizardJunimoNote"))
                Game1.player.mailReceived.Add("wizardJunimoNote");
            if (!Game1.player.mailReceived.Contains("canReadJunimoText"))
                Game1.player.mailReceived.Add("canReadJunimoText");


            // Check if the note is there, if not, add it.
            int area = ItemUtils.bundleToAreaDictionary[bundleId];
            if (!c.isJunimoNoteAtArea(area))
                c.addJunimoNote(area);

            // Add the bundle.
            for (int index = 0; index < c.bundles[bundleId].Length; ++index)
                c.bundles[bundleId][index] = true;

            c.bundleRewards[bundleId] = true;
        }

        // Single bundle
        //for (int index = 0; index< (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[Convert.ToInt32(strArray[1])].Length; ++index)
        //        (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[Convert.ToInt32(strArray[1])][index] = true;

        // Add Junimo Note
        //(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).addJunimoNote(Convert.ToInt32(strArray[1]));


        public static int getAreaNumberFromName(string name)
        {
            switch (name)
            {
                case "Pantry":
                    return 0;
                case "Crafts Room":
                case "CraftsRoom":
                    return 1;
                case "Fish Tank":
                case "FishTank":
                    return 2;
                case "Boiler Room":
                case "BoilerRoom":
                    return 3;
                case "Vault":
                    return 4;
                case "BulletinBoard":
                case "Bulletin Board":
                case "Bulletin":
                    return 5;
                default:
                    return -1;
            }
        }

    }
}
