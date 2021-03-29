using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.Tools;
using Object = StardewValley.Object;

namespace PelicanFiber.Framework
{
    internal class ItemUtils
    {
        /*********
        ** Properties
        *********/
        private readonly IContentHelper Content;
        private readonly IDataHelper Data;
        private readonly IMonitor Monitor;
        private Dictionary<int, int> BundleToAreaDictionary;


        /*********
        ** Public methods
        *********/
        public ItemUtils(IContentHelper content, IDataHelper data, IMonitor monitor)
        {
            this.Content = content;
            this.Data = data;
            this.Monitor = monitor;
        }

        public List<ISalable> GetShopStock(bool isPierre, bool unfiltered = false)
        {
            List<ISalable> stock = new List<ISalable>();
            if (isPierre)
            {
                if (Game1.currentSeason.Equals("spring") || unfiltered)
                {
                    stock.AddRange(new[]
                    {
                        new Object(Vector2.Zero, 472, int.MaxValue),
                        new Object(Vector2.Zero, 473, int.MaxValue),
                        new Object(Vector2.Zero, 474, int.MaxValue),
                        new Object(Vector2.Zero, 475, int.MaxValue),
                        new Object(Vector2.Zero, 427, int.MaxValue),
                        new Object(Vector2.Zero, 477, int.MaxValue),
                        new Object(Vector2.Zero, 429, int.MaxValue),
                        new Object(745, int.MaxValue, false, 100)
                    });
                    if (Game1.year > 1 || unfiltered)
                    {
                        stock.Add(new Object(Vector2.Zero, 476, int.MaxValue));
                        stock.Add(new Object(Vector2.Zero, 273, int.MaxValue));
                    }
                }
                if (Game1.currentSeason.Equals("summer") || unfiltered)
                {
                    stock.AddRange(new[]
                    {
                        new Object(Vector2.Zero, 479, int.MaxValue),
                        new Object(Vector2.Zero, 480, int.MaxValue),
                        new Object(Vector2.Zero, 481, int.MaxValue),
                        new Object(Vector2.Zero, 482, int.MaxValue),
                        new Object(Vector2.Zero, 483, int.MaxValue),
                        new Object(Vector2.Zero, 484, int.MaxValue),
                        new Object(Vector2.Zero, 453, int.MaxValue),
                        new Object(Vector2.Zero, 455, int.MaxValue),
                        new Object(Vector2.Zero, 302, int.MaxValue),
                        new Object(Vector2.Zero, 487, int.MaxValue),
                        new Object(431, int.MaxValue, false, 100)
                    });
                    if (Game1.year > 1 || unfiltered)
                        stock.Add(new Object(Vector2.Zero, 485, int.MaxValue));
                }
                if (Game1.currentSeason.Equals("fall") || unfiltered)
                {
                    stock.AddRange(new[]
                    {
                        new Object(Vector2.Zero, 490, int.MaxValue),
                        new Object(Vector2.Zero, 487, int.MaxValue),
                        new Object(Vector2.Zero, 488, int.MaxValue),
                        new Object(Vector2.Zero, 491, int.MaxValue),
                        new Object(Vector2.Zero, 492, int.MaxValue),
                        new Object(Vector2.Zero, 493, int.MaxValue),
                        new Object(Vector2.Zero, 483, int.MaxValue),
                        new Object(431, int.MaxValue, false, 100),
                        new Object(Vector2.Zero, 425, int.MaxValue),
                        new Object(Vector2.Zero, 299, int.MaxValue),
                        new Object(Vector2.Zero, 301, int.MaxValue)
                    });
                    if (Game1.year > 1 || unfiltered)
                        stock.Add(new Object(Vector2.Zero, 489, int.MaxValue));
                }

                stock.Add(new Object(Vector2.Zero, 297, int.MaxValue));
                if (!Game1.player.craftingRecipes.ContainsKey("Grass Starter"))
                    stock.Add(new Object(297, 1, true, 1000, 0));
                stock.AddRange(new[]
                {
                    new Object(628, int.MaxValue, false, 1700),
                    new Object(629, int.MaxValue, false, 1000),
                    new Object(630, int.MaxValue, false, 2000),
                    new Object(631, int.MaxValue, false, 3000),
                    new Object(632, int.MaxValue, false, 3000),
                    new Object(633, int.MaxValue, false, 2000),
                    new Object(Vector2.Zero, 245, int.MaxValue),
                    new Object(Vector2.Zero, 246, int.MaxValue),
                    new Object(Vector2.Zero, 423, int.MaxValue),
                    new Object(Vector2.Zero, 247, int.MaxValue),
                    new Object(Vector2.Zero, 419, int.MaxValue)
                });
                if ((int) Game1.stats.DaysPlayed >= 15)
                {
                    stock.Add(new Object(368, int.MaxValue, false, 50));
                    stock.Add(new Object(370, int.MaxValue, false, 50));
                    stock.Add(new Object(465, int.MaxValue, false, 50));
                }
                if (Game1.year > 1)
                {
                    stock.Add(new Object(369, int.MaxValue, false, 75));
                    stock.Add(new Object(371, int.MaxValue, false, 75));
                    stock.Add(new Object(466, int.MaxValue, false, 75));
                }

                Random random = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
                int which = random.Next(112);
                if (which == 21)
                    which = 36;
                stock.AddRange(new[]
                {
                    new Wallpaper(which) { Stack = int.MaxValue },
                    new Wallpaper(random.Next(56), true) { Stack = int.MaxValue }
                });
                stock.Add(new Furniture(1308, Vector2.Zero) { Stack = int.MaxValue });
                if (Game1.player.hasAFriendWithHeartLevel(8, true))
                    stock.Add(new Object(Vector2.Zero, 458, int.MaxValue));
            }
            else
            {
                stock.AddRange(new[]
                {
                    new Object(Vector2.Zero, 802, int.MaxValue) { Price = 75 },
                    new Object(Vector2.Zero, 478, int.MaxValue),
                    new Object(Vector2.Zero, 486, int.MaxValue),
                    new Object(Vector2.Zero, 494, int.MaxValue),
                    new Object(Vector2.Zero, 196, false) { Stack = int.MaxValue }
                });
                if (unfiltered)
                {
                    stock.AddRange(new[]
                    {
                        new Object(Vector2.Zero, 88, int.MaxValue),
                        new Object(Vector2.Zero, 90, int.MaxValue),
                        new Object(Vector2.Zero, 749, int.MaxValue),
                        new Object(Vector2.Zero, 466, int.MaxValue),
                        new Object(Vector2.Zero, 340, int.MaxValue),
                        new Object(Vector2.Zero, 371, int.MaxValue),
                        new Object(Vector2.Zero, 233, int.MaxValue)
                    });
                }
                else
                {
                    switch (Game1.dayOfMonth % 7)
                    {
                        case 0:
                            stock.Add(new Object(Vector2.Zero, 233, int.MaxValue));
                            break;
                        case 1:
                            stock.Add(new Object(Vector2.Zero, 88, int.MaxValue));
                            break;
                        case 2:
                            stock.Add(new Object(Vector2.Zero, 90, int.MaxValue));
                            break;
                        case 3:
                            stock.Add(new Object(Vector2.Zero, 749, int.MaxValue));
                            break;
                        case 4:
                            stock.Add(new Object(Vector2.Zero, 466, int.MaxValue));
                            break;
                        case 5:
                            stock.Add(new Object(Vector2.Zero, 340, int.MaxValue));
                            break;
                        case 6:
                            stock.Add(new Object(Vector2.Zero, 371, int.MaxValue));
                            break;                            
                    }
                }
                stock.Add(new Clothing(1000 + new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2).Next((int)sbyte.MaxValue)) { Price = 1000, Stack = int.MaxValue });
            }
            return stock;
        }


        public List<ISalable> GetCarpenterStock(bool unfiltered = false)
        {
            Random r = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);

            List<ISalable> stock = new List<ISalable>
            {
                new Object(Vector2.Zero, 388, int.MaxValue),
                new Object(Vector2.Zero, 390, int.MaxValue),
                new Furniture(1614, Vector2.Zero) { Stack = int.MaxValue },
                new Furniture(1616, Vector2.Zero) { Stack = int.MaxValue },
                new Workbench(Vector2.Zero) { Stack = int.MaxValue }
            };
            if (Game1.currentSeason == "winter" || Game1.year >= 2 || unfiltered)
                stock.Add(new WoodChipper(Vector2.Zero) { Stack = int.MaxValue });
            if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0 || unfiltered)
                stock.Add(new Object(Vector2.Zero, 216, false) { Stack = int.MaxValue });

			if (unfiltered)
			{
				stock.AddRange(this.GetAllFurniture());
			}
			else
			{
				switch (Game1.dayOfMonth % 7)
				{
					case 0:
						stock.Add(this.GetRandomFurniture(r, stock, 1296, 1391));
						stock.Add(this.GetRandomFurniture(r, stock, 416, 537));
						break;
					case 1:
						stock.AddRange(new[]
						{
							new Furniture(0, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(192, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(704, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1120, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1216, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1391, Vector2.Zero) { Stack = int.MaxValue }
						});
						break;
					case 2:
						stock.AddRange(new[]
						{
							new Furniture(3, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(197, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(709, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1122, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1218, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1393, Vector2.Zero) { Stack = int.MaxValue }
						});
						break;
					case 3:
						stock.AddRange(new[]
						{
							new Furniture(6, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(202, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(714, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1124, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1220, Vector2.Zero) { Stack = int.MaxValue },
							new Furniture(1395, Vector2.Zero) { Stack = int.MaxValue }
						});
						break;
					case 4:
						stock.AddRange(new[]
						{
							this.GetRandomFurniture(r, stock, 1296, 1391),
							this.GetRandomFurniture(r, stock, 1296, 1391)
						});
						break;
					case 5:
						stock.AddRange(new[]
						{
							this.GetRandomFurniture(r, stock, 1443, 1450),
							this.GetRandomFurniture(r, stock, 288, 313)
						});
						break;
					case 6:
						stock.AddRange(new[]
						{
							this.GetRandomFurniture(r, stock, 1565, 1607),
							this.GetRandomFurniture(r, stock, 12, 129)
						});
						break;
				}
				stock.Add(this.GetRandomFurniture(r, stock, 0, 1462));
				stock.Add(this.GetRandomFurniture(r, stock, 0, 1462));
				while (r.NextDouble() < 0.25)
					stock.Add(this.GetRandomFurniture(r, stock, 1673, 1815));
				stock.Add(new Furniture(1402, Vector2.Zero) { Stack = int.MaxValue });
				stock.Add(new TV(1466, Vector2.Zero) { Stack = int.MaxValue });
				stock.Add(new TV(1680, Vector2.Zero) { Stack = int.MaxValue });
				if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
					stock.Add(new TV(1468, Vector2.Zero) { Stack = int.MaxValue });
                if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
                    stock.Add(new Furniture(1226, Vector2.Zero) { Stack = int.MaxValue });
                stock.Add(new Object(Vector2.Zero, 200, false) { Stack = int.MaxValue });
                stock.Add(new Object(Vector2.Zero, 35, false) { Stack = int.MaxValue });
                stock.Add(new Object(Vector2.Zero, 46, false) { Stack = int.MaxValue });
                stock.Add(new Furniture(1792, Vector2.Zero) { Stack = int.MaxValue });
                stock.Add(new Furniture(1794, Vector2.Zero) { Stack = int.MaxValue });
                stock.Add(new Furniture(1798, Vector2.Zero) { Stack = int.MaxValue });
            }

            if (!Game1.player.craftingRecipes.ContainsKey("Wooden Brazier"))
                stock.Add(new Torch(Vector2.Zero, 143, true) { IsRecipe = true });

            else if (!Game1.player.craftingRecipes.ContainsKey("Stone Brazier"))
                stock.Add(new Torch(Vector2.Zero, 144, true) { IsRecipe = true });

            else if (!Game1.player.craftingRecipes.ContainsKey("Barrel Brazier"))
                stock.Add(new Torch(Vector2.Zero, 150, true) { IsRecipe = true });

            else if (!Game1.player.craftingRecipes.ContainsKey("Stump Brazier"))
                stock.Add(new Torch(Vector2.Zero, 147, true) { IsRecipe = true });

            else if (!Game1.player.craftingRecipes.ContainsKey("Gold Brazier"))
                stock.Add(new Torch(Vector2.Zero, 145, true) { IsRecipe = true });

            else if (!Game1.player.craftingRecipes.ContainsKey("Carved Brazier"))
                stock.Add(new Torch(Vector2.Zero, 148, true) { IsRecipe = true });

            else if (!Game1.player.craftingRecipes.ContainsKey("Skull Brazier"))
                stock.Add(new Torch(Vector2.Zero, 149, true) { IsRecipe = true });

            else if (!Game1.player.craftingRecipes.ContainsKey("Marble Brazier"))
                stock.Add(new Torch(Vector2.Zero, 151, true) { IsRecipe = true });

            if (!Game1.player.craftingRecipes.ContainsKey("Wood Lamp-post"))
                stock.Add(new Object(Vector2.Zero, 152, true) { IsRecipe = true });

            if (!Game1.player.craftingRecipes.ContainsKey("Iron Lamp-post"))
                stock.Add(new Object(Vector2.Zero, 153, true) { IsRecipe = true });

            if (!Game1.player.craftingRecipes.ContainsKey("Wood Floor"))
                stock.Add(new Object(328, 1, true, 50));
            if (!Game1.player.craftingRecipes.ContainsKey("Stone Floor"))
                stock.Add(new Object(329, 1, true, 50));
            if (!Game1.player.craftingRecipes.ContainsKey("Brick Floor"))
                stock.Add(new Object(293, 1, true, 50));
            if (!Game1.player.craftingRecipes.ContainsKey("Stepping Stone Path"))
                stock.Add(new Object(415, 1, true, 50));
            if (!Game1.player.craftingRecipes.ContainsKey("Straw Floor"))
                stock.Add(new Object(401, 1, true, 100));
            if (!Game1.player.craftingRecipes.ContainsKey("Crystal Path"))
                stock.Add(new Object(409, 1, true, 100));
            return stock;
        }


        private bool IsFurnitureOffLimitsForSale(int index)
		{
			if (index <= 1545)
			{
				if (index <= 1375)
				{
					if (index <= 989)
					{
						if (index != 131 && (uint)(index - 984) > 2U && index != 989)
							goto label_22;
					}
					else if (index != 1226 && (uint)(index - 1298) > 11U)
					{
						switch (index)
						{
							case 1371:
							case 1373:
							case 1375:
								break;
							default:
								goto label_22;
						}
					}
				}
				else if (index <= 1468)
				{
					if (index != 1402 && index != 1466 && index != 1468)
						goto label_22;
				}
				else if (index != 1471 && index != 1541 && index != 1545)
					goto label_22;
			}
			else if (index <= 1764)
			{
				if (index <= 1671)
				{
					if (index != 1554 && index != 1669 && index != 1671)
						goto label_22;
				}
				else if (index != 1680 && index != 1733 && (uint)(index - 1760) > 4U)
					goto label_22;
			}
			else if (index <= 1900)
			{
				switch (index - 1796)
				{
					case 0:
					case 2:
					case 4:
					case 6:
						break;
					case 1:
					case 3:
					case 5:
						goto label_22;
					default:
						switch (index - 1838)
						{
							case 0:
							case 2:
							case 4:
							case 6:
							case 8:
							case 10:
							case 12:
							case 14:
							case 16:
								break;
							case 1:
							case 3:
							case 5:
							case 7:
							case 9:
							case 11:
							case 13:
							case 15:
								goto label_22;
							default:
								if (index == 1900)
									break;
								goto label_22;
						}
						break;
				}
			}
			else if (index <= 1918)
			{
				if (index != 1902)
				{
					switch (index - 1907)
					{
						case 0:
						case 2:
						case 7:
						case 8:
						case 9:
						case 10:
						case 11:
							break;
						default:
							goto label_22;
					}
				}
			}
			else if ((uint)(index - 1952) > 9U && index != 1971)
				goto label_22;
			return true;
			label_22:
			return false;
		}

		private List<Item> GetAllFurniture()
        {
            List<Item> list = new List<Item>();

            foreach (KeyValuePair<int, string> keyValuePair in this.Content.Load<Dictionary<int, string>>("Data\\Furniture", ContentSource.GameContent))
            {
                if (!this.IsFurnitureOffLimitsForSale(keyValuePair.Key))
                    list.Add(new Furniture(keyValuePair.Key, Vector2.Zero));
            }

            return list;
        }

        private Furniture GetRandomFurniture(Random r, List<ISalable> stock, int lowerIndexBound = 0, int upperIndexBound = 1462)
        {
            Dictionary<int, string> dictionary = this.Content.Load<Dictionary<int, string>>("Data\\Furniture", ContentSource.GameContent);
            int num;
            do
            {
                num = r.Next(lowerIndexBound, upperIndexBound);
                if (stock != null)
                {
                    foreach (Item obj in stock)
                    {
                        if (obj is Furniture && obj.ParentSheetIndex == num)
                            num = -1;
                    }
                }
            }
            while (this.IsFurnitureOffLimitsForSale(num) || !dictionary.ContainsKey(num));
            return new Furniture(num, Vector2.Zero) { Stack = int.MaxValue };
        }

        public Dictionary<ISalable, int[]> GetBlacksmithStock(bool unfiltered = false)
        {
            if (unfiltered)
            {
                return new Dictionary<ISalable, int[]>
                {
                    [new Object(Vector2.Zero, 378, int.MaxValue)] = new[] { 75, int.MaxValue },
                    [new Object(Vector2.Zero, 380, int.MaxValue)] = new[] { 150, int.MaxValue },
                    [new Object(Vector2.Zero, 382, int.MaxValue)] = new[] { 150, int.MaxValue },
                    [new Object(Vector2.Zero, 384, int.MaxValue)] = new[] { 400, int.MaxValue },
                    [new Object(Vector2.Zero, 386, int.MaxValue)] = new[] { 400, int.MaxValue },

                    [new Object(Vector2.Zero, 334, int.MaxValue)] = new[] { 150, int.MaxValue },
                    [new Object(Vector2.Zero, 335, int.MaxValue)] = new[] { 250, int.MaxValue },
                    [new Object(Vector2.Zero, 336, int.MaxValue)] = new[] { 750, int.MaxValue },
                    [new Object(Vector2.Zero, 337, int.MaxValue)] = new[] { 2000, int.MaxValue },
                    [new Object(Vector2.Zero, 338, int.MaxValue)] = new[] { 100, int.MaxValue }
                };
            }
            else
            {
                return new Dictionary<ISalable, int[]>
                {
                    [new Object(Vector2.Zero, 378, int.MaxValue)] = new[] { 75, int.MaxValue },
                    [new Object(Vector2.Zero, 380, int.MaxValue)] = new[] { 150, int.MaxValue },
                    [new Object(Vector2.Zero, 382, int.MaxValue)] = new[] { 150, int.MaxValue },
                    [new Object(Vector2.Zero, 384, int.MaxValue)] = new[] { 400, int.MaxValue }
                };
            }
        }

        public Dictionary<ISalable, int[]> GetFishShopStock(Farmer who, bool unfiltered = false)
        {
            Dictionary<ISalable, int[]> stock = new Dictionary<ISalable, int[]>
            {
                [new Object(219, 1)] = new[] { 250, int.MaxValue }
            };

            if (Game1.player.fishingLevel.Value >= 2 || unfiltered)
                stock.Add(new Object(685, 1), new[] { 5, int.MaxValue });
            if (Game1.player.fishingLevel.Value >= 3 || unfiltered)
                stock.Add(new Object(710, 1), new[] { 1500, int.MaxValue });
            if (Game1.player.fishingLevel.Value >= 6 || unfiltered)
            {
                stock.Add(new Object(686, 1), new[] { 500, int.MaxValue });
                stock.Add(new Object(694, 1), new[] { 500, int.MaxValue });
                stock.Add(new Object(692, 1), new[] { 200, int.MaxValue });
            }
            if (Game1.player.fishingLevel.Value >= 7 || unfiltered)
            {
                stock.Add(new Object(693, 1), new[] { 750, int.MaxValue });
                stock.Add(new Object(695, 1), new[] { 750, int.MaxValue });
            }
            if (Game1.player.fishingLevel.Value >= 8 || unfiltered)
            {
                stock.Add(new Object(691, 1), new[] { 1000, int.MaxValue });
                stock.Add(new Object(687, 1), new[] { 1000, int.MaxValue });
            }
            if (Game1.player.fishingLevel.Value >= 9 || unfiltered)
                stock.Add(new Object(703, 1), new[] { 1000, int.MaxValue });
            stock.Add(new FishingRod(0), new[] { 500, int.MaxValue });
            stock.Add(new FishingRod(1), new[] { 25, int.MaxValue });
            if (Game1.player.fishingLevel.Value >= 2 || unfiltered)
                stock.Add(new FishingRod(2), new[] { 1800, int.MaxValue });
            if (Game1.player.fishingLevel.Value >= 6 || unfiltered)
                stock.Add(new FishingRod(3), new[] { 7500, int.MaxValue });
            if (Game1.MasterPlayer.mailReceived.Contains("ccFishTank"))
                stock.Add(new Pan(), new[] { 2500, int.MaxValue });

            if (unfiltered)
            {
                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("Fish -4"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                        Item i = new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2);

                        stock.Add(i, new[] { Convert.ToInt32(strArray[1]) * 4, int.MaxValue });
                    }
                }
            }

            return stock;
        }

        public List<ISalable> GetSaloonStock(bool unfiltered = false)
        {
            List<ISalable> stock = new List<ISalable>
            {
                new Object(Vector2.Zero, 346, int.MaxValue),
                new Object(Vector2.Zero, 196, int.MaxValue),
                new Object(Vector2.Zero, 216, int.MaxValue),
                new Object(Vector2.Zero, 224, int.MaxValue),
                new Object(Vector2.Zero, 206, int.MaxValue),
                new Object(Vector2.Zero, 395, int.MaxValue)
            };

            if (Game1.dishOfTheDay.Stack > 0 && !unfiltered)
                stock.Add(Game1.dishOfTheDay.getOne() as Object);
            else if (unfiltered)
            {
                // 194 - 239
                for (int i = 194; i < 240; i++)
                {
                    int parentSheetIndex = i;
                    if (parentSheetIndex == 217)
                        parentSheetIndex = 216;
                    stock.Add(new Object(Vector2.Zero, parentSheetIndex, Game1.random.Next(1, 4 + (Game1.random.NextDouble() < 0.08 ? 10 : 0))));
                }
            }

            if (!Game1.player.cookingRecipes.ContainsKey("Hashbrowns"))
                stock.Add(new Object(210, 1, true, 25));
            if (!Game1.player.cookingRecipes.ContainsKey("Omelet"))
                stock.Add(new Object(195, 1, true, 50));
            if (!Game1.player.cookingRecipes.ContainsKey("Pancakes"))
                stock.Add(new Object(211, 1, true, 50));
            if (!Game1.player.cookingRecipes.ContainsKey("Bread"))
                stock.Add(new Object(216, 1, true, 50));
            if (!Game1.player.cookingRecipes.ContainsKey("Tortilla"))
                stock.Add(new Object(229, 1, true, 50));
            if (!Game1.player.cookingRecipes.ContainsKey("Pizza"))
                stock.Add(new Object(206, 1, true, 75));
            if (!Game1.player.cookingRecipes.ContainsKey("Maki Roll"))
                stock.Add(new Object(228, 1, true, 150));
            if (!Game1.player.cookingRecipes.ContainsKey("Cookies") && Game1.player.eventsSeen.Contains(19))
                stock.Add(new Object(223, 1, true, 150) { name = "Cookies" });
            if (!Game1.player.cookingRecipes.ContainsKey("Triple Shot Espresso"))
                stock.Add(new Object(253, 1, true, 500));
            if (Game1.player.activeDialogueEvents.ContainsKey("willyCrabs"))
                stock.Add(new Object(Vector2.Zero, 732, int.MaxValue));
            
            return stock;
        }

        public List<ISalable> GetLeahShopStock(bool unfiltered = false)
        {
            List<ISalable> stock = new List<ISalable>();

            foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
            {
                if (Game1.objectInformation[keyValuePair.Key].Contains("/Basic -81"))
                {
                    string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                    stock.Add(new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2));
                }
                else if (keyValuePair.Key == 406 || keyValuePair.Key == 414 || keyValuePair.Key == 396)
                {
                    string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                    stock.Add(new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2));
                }
            }

            return stock;
        }

        public List<ISalable> GetRecipesStock(bool unfiltered = false)
        {
            List<ISalable> stock = new List<ISalable>();

            if (unfiltered)
            {
                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("/Cooking -7"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                        Item item = new Object(keyValuePair.Key, int.MaxValue, true, Convert.ToInt32(strArray[1]) * 10);

                        if (!Game1.player.cookingRecipes.ContainsKey(strArray[0]))
                            stock.Add(item);
                    }
                }
            }

            return stock;
        }

        public List<ISalable> GetMineralsAndArtifactsStock(bool unfiltered = false)
        {
            List<ISalable> stock = new List<ISalable>();

            if (unfiltered)
            {
                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("/Minerals -"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');
                        stock.Add(new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 2));
                    }
                }

                foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
                {
                    if (Game1.objectInformation[keyValuePair.Key].Contains("/Arch/"))
                    {
                        string[] strArray = Game1.objectInformation[keyValuePair.Key].Split('/');

                        if (keyValuePair.Key == 114)
                            stock.Add(new Object(keyValuePair.Key, int.MaxValue, false, 20000));
                        else if (keyValuePair.Key == 96 || keyValuePair.Key == 97 || keyValuePair.Key == 98 || keyValuePair.Key == 99)
                            stock.Add(new Object(keyValuePair.Key, int.MaxValue, false, 5000));
                        else
                            stock.Add(new Object(keyValuePair.Key, int.MaxValue, false, Convert.ToInt32(strArray[1]) * 3));
                    }
                }
            }

            return stock;
        }

        public List<Object> GetPurchaseAnimalStock()
        {
            string locationName = ((AnimalHouse)Game1.currentLocation).getBuilding().buildingType.Value;

            return new List<Object>
            {
                new Object(100, 1, false, 400){ name = "Chicken", Type = locationName.Equals("Coop") || locationName.Equals("Deluxe Coop") || locationName.Equals("Big Coop") ? null : "You gotta be in a Coop" },
                new Object(100, 1, false, 750) { name = "Dairy Cow", Type = locationName.Equals("Barn") || locationName.Equals("Deluxe Barn") || locationName.Equals("Big Barn") ? null : "You gotta be in a Barn" },
                new Object(100, 1, false, 2000){ name = "Goat", Type = locationName.Equals("Big Barn") || locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Big Barn" },
                new Object(100, 1, false, 2000) { name = "Duck", Type = locationName.Equals("Big Coop") || locationName.Equals("Deluxe Coop") ? null : "You gotta be in a Big Coop" },
                new Object(100, 1, false, 4000) { name = "Sheep", Type = locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Deluxe Barn" },
                new Object(100, 1, false, 4000) { name = "Rabbit", Type = locationName.Equals("Deluxe Coop") ? null : "You gotta be in a Deluxe Coop" },
                new Object(100, 1, false, 8000){ name = "Pig", Type = locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Deluxe Barn" }
            };
        }

        public void FinishAllBundles()
        {
            foreach (KeyValuePair<int, bool[]> bundle in ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles.Pairs)
            {
                for (int index = 0; index < bundle.Value.Length; ++index)
                    bundle.Value[index] = true;
            }
        }

        public List<ISalable> GetJunimoStock()
        {
            List<ISalable> junimoItems = new List<ISalable>();
            Dictionary<int, string> junContent = this.Data.ReadJsonFile<Dictionary<int, string>>("assets\\bundles.json");
            KeyValuePair<int, bool[]>[] bundleInfo = ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles.Pairs.ToArray();
            this.BundleToAreaDictionary = new Dictionary<int, int>();

            foreach (KeyValuePair<int, string> kvp in junContent)
            {
                int id = kvp.Key;
                string[] parts = kvp.Value.Split('/');

                if (parts.Length >= 7)
                {
                    string name = parts[0];
                    int price = int.Parse(parts[1]);
                    int category = int.Parse(parts[3].Split(' ')[1]);
                    int bundleIndex = int.Parse(parts[4]);
                    string area = parts[5];

                    Object o = new Object(id, 1, true, price)
                    {
                        Category = category,
                        Name = name,
                        SpecialVariable = bundleIndex
                    };

                    this.BundleToAreaDictionary.Add(bundleIndex, this.GetAreaNumberFromName(area));

                    foreach (KeyValuePair<int, bool[]> bundle in bundleInfo)
                    {
                        if (bundle.Key == bundleIndex)
                        {
                            foreach (bool i in bundle.Value)
                            {
                                if (!i)
                                {
                                    junimoItems.Add(o);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                    this.Monitor.Log($"Parts not >= 6: {kvp.Key}: {kvp.Value}", LogLevel.Error);
            }

            return junimoItems;
        }

        public void AddBundle(int bundleId)
        {
            CommunityCenter c = (CommunityCenter)Game1.getLocationFromName("CommunityCenter");

            // Check if the player has access to the community center
            if (!Game1.player.mailReceived.Contains("ccDoorUnlock"))
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
            int area = this.BundleToAreaDictionary[bundleId];
            if (!c.isJunimoNoteAtArea(area))
                c.addJunimoNote(area);

            // Add the bundle.
            for (int index = 0; index < c.bundles[bundleId].Length; ++index)
                c.bundles[bundleId][index] = true;

            c.bundleRewards[bundleId] = true;
        }

        private int GetAreaNumberFromName(string name)
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
