using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PelicanFiber;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;

namespace StardewValley.Menus
{
    internal class ShopMenu2 : IClickableMenu
    {
        private string descriptionText = "";
        private string hoverText = "";
        private string boldTitleText = "";
        private List<Item> forSale = new List<Item>();
        private List<ClickableComponent> forSaleButtons = new List<ClickableComponent>();
        private List<int> categoriesToSellHere = new List<int>();
        private Dictionary<Item, int[]> itemPriceAndStock = new Dictionary<Item, int[]>();
        private float sellPercentage = 1f;
        private List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();
        private int hoverPrice = -1;
        public const int howManyRecipesFitOnPage = 28;
        public const int infiniteStock = 2147483647;
        public const int salePriceIndex = 0;
        public const int stockIndex = 1;
        public const int extraTradeItemIndex = 2;
        public const int itemsPerPage = 4;
        public const int numberRequiredForExtraItemTrade = 5;
        private InventoryMenu inventory;
        private Item heldItem;
        private Item hoveredItem;
        private Texture2D wallpapers;
        private Texture2D floors;
        private int lastWallpaperFloorPrice;
        private TemporaryAnimatedSprite poof;
        private Rectangle scrollBarRunner;
        private int currency;
        private int currentItemIndex;
        private ClickableTextureComponent upArrow;
        private ClickableTextureComponent downArrow;
        private ClickableTextureComponent scrollBar;
        private ClickableTextureComponent backButton;

        public NPC portraitPerson;
        public string potraitPersonDialogue;
        private bool scrolling;
        private String locationName;

        public ShopMenu2(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null, String locationName = "")
          : this(itemPriceAndStock.Keys.ToList(), currency, who, locationName)
        {
            this.locationName = locationName;
            this.itemPriceAndStock = itemPriceAndStock;
            if (this.potraitPersonDialogue != null)
                return;
            this.setUpShopOwner(who);
        }

        public ShopMenu2(List<Item> itemsForSale, int currency = 0, string who = null, string locationName = "")
          : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 1000 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            this.locationName = locationName;
            this.currency = currency;
            if (Game1.viewport.Width < 1500)
                this.xPositionOnScreen = Game1.tileSize / 2;
            Game1.player.forceCanMove();
            Game1.playSound("dwop");
            this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, null, this.highlightItemToSell)
            {
                showGrayedOutSlots = true
            };
            this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
            this.currency = currency;
            int positionOnScreen1 = this.xPositionOnScreen;
            int borderWidth1 = IClickableMenu.borderWidth;
            int toClearSideBorder = IClickableMenu.spaceToClearSideBorder;
            int positionOnScreen2 = this.yPositionOnScreen;
            int borderWidth2 = IClickableMenu.borderWidth;
            int toClearTopBorder = IClickableMenu.spaceToClearTopBorder;
            this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), Game1.pixelZoom);
            this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
            for (int index = 0; index < 4; ++index)
                this.forSaleButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat(index)));
            foreach (Item key in itemsForSale)
            {
                //if (key is StardewValley.Object && (key as StardewValley.Object).isRecipe || (key as StardewValley.Object).category == -425)
                if (key is Object && (key as Object).isRecipe || locationName.Equals("Junimo"))
                {
                    if (!Game1.player.knowsRecipe(key.Name))
                        key.Stack = 1;
                    else
                        continue;
                }
                this.forSale.Add(key);
                this.itemPriceAndStock.Add(key, new[]
                {
          key.salePrice(),
          key.Stack
                });
            }
            if (this.itemPriceAndStock.Count >= 2)
                this.setUpShopOwner(who);
            //switch (Game1.currentLocation.name)
            switch (this.locationName)
            {
                case "SeedShop":
                    this.categoriesToSellHere.AddRange(new[]
                    {
                        -81,
                        -75,
                        -79,
                        -80,
                        -74,
                        -17,
                        -18,
                        -6,
                        -26,
                        -5,
                        -14,
                        -19,
                        -7,
                        -25
                    });
                    break;
                case "Blacksmith":
                    this.categoriesToSellHere.AddRange(new[] { -12, -2, -15 });
                    break;
                case "ScienceHouse":
                    this.categoriesToSellHere.AddRange(new[] { -16 });
                    break;
                case "AnimalShop":
                    this.categoriesToSellHere.AddRange(new[] { -18, -6, -5, -14 });
                    break;
                case "FishShop":
                    this.categoriesToSellHere.AddRange(new[]
                    {
                        -4,
                        -23,
                        -21,
                        -22
                    });
                    break;
                case "AdventureGuild":
                    this.categoriesToSellHere.AddRange(new[]
                    {
                        -28,
                        -98,
                        -97,
                        -96
                    });
                    break;
            }
            Game1.currentLocation.Name.Equals("SeedShop");
        }

        private void setUpShopOwner(string who)
        {
            if (who == null)
                return;
            Random random = new Random((int)((long)Game1.uniqueIDForThisGame + Game1.stats.DaysPlayed));
            string text = "Have a look at my wares.";
            switch (who)
            {
                case "Leah":
                    this.portraitPerson = Game1.getCharacterFromName("Leah");
                    switch (Game1.random.Next(3))
                    {
                        case 0:
                            text = "Much of my selection comes from right here in the valley.";
                            break;
                        case 1:
                            text = "Don't have time to forage? 'Foraged By Leah' has you covered!";
                            break;
                        case 2:
                            text = "You should try our Dandelions.";
                            break;
                    }
                    break;
                case "Robin":
                    this.portraitPerson = Game1.getCharacterFromName("Robin");
                    switch (Game1.random.Next(5))
                    {
                        case 0:
                            text = "Need some construction supplies? Or are you looking to re-decorate?";
                            break;
                        case 1:
                            text = "I have a rotating selection of hand-made furniture.";
                            break;
                        case 2:
                            text = "I've got some great furniture for sale.";
                            break;
                        case 3:
                            text = "Got any spare construction material to sell?";
                            break;
                        case 4:
                            text = "I've got " + Lexicon.appendArticle(this.itemPriceAndStock.ElementAt(Game1.random.Next(2, this.itemPriceAndStock.Count)).Key.Name) + " that would look just " + Lexicon.getRandomPositiveAdjectiveForEventOrPerson() + " in your house.";
                            break;
                    }
                    break;
                case "Clint":
                    this.portraitPerson = Game1.getCharacterFromName("Clint");
                    switch (Game1.random.Next(3))
                    {
                        case 0:
                            text = "Too lazy to mine your own ore? No problem.";
                            break;
                        case 1:
                            text = "I've got lumps of raw metal for sale. Knock yourself out.";
                            break;
                        case 2:
                            text = "Looking to sell any metals or minerals?";
                            break;
                    }
                    break;
                case "ClintUpgrade":
                    this.portraitPerson = Game1.getCharacterFromName("Clint");
                    text = "I can upgrade your tools with more power. You'll have to leave them with me for a few days, though.";
                    break;
                case "Willy":
                    this.portraitPerson = Game1.getCharacterFromName("Willy");
                    text = "Need fishing supplies? You've come to the right place.";
                    if (Game1.random.NextDouble() < 0.05)
                    {
                        text = "Sorry about the smell.";
                        break;
                    }
                    break;
                case "Pierre":
                    this.portraitPerson = Game1.getCharacterFromName("Pierre");
                    switch (Game1.dayOfMonth % 7)
                    {
                        case 0:
                            text = "Got anything you want to sell?";
                            break;
                        case 1:
                            text = "Need some supplies?";
                            break;
                        case 2:
                            text = "Don't forget to check out my daily wallpaper and flooring selection!";
                            break;
                        case 3:
                            text = "What can I get for you?";
                            break;
                        case 4:
                            text = "I carry only the finest goods.";
                            break;
                        case 5:
                            text = "I've got quality goods for sale.";
                            break;
                        case 6:
                            text = "Looking to buy something?";
                            break;
                    }
                    text = "Welcome to Pierre's! " + text;
                    if (Game1.dayOfMonth == 28)
                    {
                        text = "The season's almost over. I'll be changing stock tomorrow.";
                        break;
                    }
                    break;
                case "Dwarf":
                    this.portraitPerson = Game1.getCharacterFromName("Dwarf");
                    text = "Buy something?";
                    break;
                case "HatMouse":
                    text = "Hiyo, poke. Did you bring coins? Gud. Me sell hats.";
                    break;
                case "Krobus":
                    this.portraitPerson = Game1.getCharacterFromName("Krobus");
                    text = "Rare Goods";
                    break;
                case "Traveler":
                    switch (random.Next(5))
                    {
                        case 0:
                            text = "I've got a little bit of everything. Take a look!";
                            break;
                        case 1:
                            text = "I smuggled these goods out of the Gotoro Empire. Why do you think they're so expensive?";
                            break;
                        case 2:
                            text = "I'll have new items every week, so make sure to come back!";
                            break;
                        case 3:
                            text = "Let me see... Oh! I've got just what you need: " + Lexicon.appendArticle(this.itemPriceAndStock.ElementAt(random.Next(this.itemPriceAndStock.Count)).Key.Name) + "!";
                            break;
                        case 4:
                            text = "Beautiful country you have here. One of my favorite stops. The pig likes it, too.";
                            break;
                    }
                    break;
                case "Marnie":
                    this.portraitPerson = Game1.getCharacterFromName("Marnie");
                    text = "Animal supplies for sale!";
                    if (random.NextDouble() < 0.0001)
                    {
                        text = "*sigh*... When the door opened I thought it might be Lewis.";
                        break;
                    }
                    break;
                case "Gus":
                    this.portraitPerson = Game1.getCharacterFromName("Gus");
                    switch (Game1.random.Next(4))
                    {
                        case 0:
                            text = "What'll you have?";
                            break;
                        case 1:
                            text = "Can you smell that? It's the " + this.itemPriceAndStock.ElementAt(random.Next(this.itemPriceAndStock.Count)).Key.Name;
                            break;
                        case 2:
                            text = "Hungry? Thirsty? I've got just the thing.";
                            break;
                        case 3:
                            text = "Welcome to the Stardrop Saloon! What can I get ya?";
                            break;
                    }
                    break;
                case "Marlon":
                    this.portraitPerson = Game1.getCharacterFromName("Marlon");
                    switch (random.Next(4))
                    {
                        case 0:
                            text = "The caves can be dangerous. Make sure you're prepared.";
                            break;
                        case 1:
                            text = "In the market for a new sword?";
                            break;
                        case 2:
                            text = "Welcome to the adventurer's guild.";
                            break;
                        case 3:
                            text = "Slay any monsters? I'll buy the loot.";
                            break;
                    }
                    if (random.NextDouble() < 0.001)
                    {
                        text = "The caves can be dangerous. How do you think I lost this eye?";
                        break;
                    }
                    break;
                case "Sandy":
                    this.portraitPerson = Game1.getCharacterFromName("Sandy");
                    text = "You won't find these goods anywhere else!";
                    if (random.NextDouble() < 0.0001)
                    {
                        text = "I've got just what you need.";
                        break;
                    }
                    break;
            }
            this.potraitPersonDialogue = Game1.parseText(text, Game1.dialogueFont, Game1.tileSize * 5 - Game1.pixelZoom * 4);
        }

        private bool highlightItemToSell(Item i)
        {
            return this.categoriesToSellHere.Contains(i.category);
        }

        private static int getPlayerCurrencyAmount(Farmer who, int currencyType)
        {
            switch (currencyType)
            {
                case 0:
                    return who.Money;
                case 1:
                    return who.festivalScore;
                case 2:
                    return who.clubCoins;
                default:
                    return 0;
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
            if (!this.scrolling)
                return;
            int y1 = this.scrollBar.bounds.Y;
            this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
            this.currentItemIndex = Math.Min(this.forSale.Count - 4, Math.Max(0, (int)(this.forSale.Count * (double)((y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height))));
            this.setScrollBarToCurrentIndex();
            if (y1 == this.scrollBar.bounds.Y)
                return;
            Game1.playSound("shiny4");
        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);
            this.scrolling = false;
        }

        private void setScrollBarToCurrentIndex()
        {
            if (this.forSale.Count <= 0)
                return;
            this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.forSale.Count - 4 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
            if (this.currentItemIndex != this.forSale.Count - 4)
                return;
            this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            if (direction > 0 && this.currentItemIndex > 0)
            {
                this.upArrowPressed();
                Game1.playSound("shiny4");
            }
            else
            {
                if (direction >= 0 || this.currentItemIndex >= Math.Max(0, this.forSale.Count - 4))
                    return;
                this.downArrowPressed();
                Game1.playSound("shiny4");
            }
        }

        private void downArrowPressed()
        {
            this.downArrow.scale = this.downArrow.baseScale;
            ++this.currentItemIndex;
            this.setScrollBarToCurrentIndex();
        }

        private void upArrowPressed()
        {
            this.upArrow.scale = this.upArrow.baseScale;
            --this.currentItemIndex;
            this.setScrollBarToCurrentIndex();
        }

        private void backButtonPressed()
        {
            if (this.readyToClose())
            {
                this.exitThisMenu();
                PelicanFiber.PelicanFiber.showTheMenu();
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y);
            if (Game1.activeClickableMenu == null)
                return;
            Vector2 clickableComponent = this.inventory.snapToClickableComponent(x, y);
            if (this.backButton.containsPoint(x, y))
            {
                this.backButton.scale = this.backButton.baseScale;
                backButtonPressed();
            }
            else if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
            {
                this.downArrowPressed();
                Game1.playSound("shwip");
            }
            else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
            {
                this.upArrowPressed();
                Game1.playSound("shwip");
            }
            else if (this.scrollBar.containsPoint(x, y))
                this.scrolling = true;
            else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
            {
                this.scrolling = true;
                this.leftClickHeld(x, y);
                this.releaseLeftClick(x, y);
            }
            this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
            if (this.heldItem == null)
            {
                Item obj = this.inventory.leftClick(x, y, null, false);
                if (obj != null)
                {
                    ShopMenu2.chargePlayer(Game1.player, this.currency, -((obj is Object ? (int)((obj as Object).sellToStorePrice() * (double)this.sellPercentage) : (int)(obj.salePrice() / 2 * (double)this.sellPercentage)) * obj.Stack));
                    int num = obj.Stack / 8 + 2;
                    for (int index = 0; index < num; ++index)
                    {
                        this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, clickableComponent + new Vector2(32f, 32f), false, false)
                        {
                            alphaFade = 0.025f,
                            motion = new Vector2(Game1.random.Next(-3, 4), -4f),
                            acceleration = new Vector2(0.0f, 0.5f),
                            delayBeforeAnimationStart = index * 25,
                            scale = Game1.pixelZoom * 0.5f
                        });
                        this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, clickableComponent + new Vector2(32f, 32f), false, false)
                        {
                            scale = Game1.pixelZoom,
                            alphaFade = 0.025f,
                            delayBeforeAnimationStart = index * 50,
                            motion = Utility.getVelocityTowardPoint(new Point((int)clickableComponent.X + 32, (int)clickableComponent.Y + 32), new Vector2(this.xPositionOnScreen - Game1.pixelZoom * 9, this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4), 8f),
                            acceleration = Utility.getVelocityTowardPoint(new Point((int)clickableComponent.X + 32, (int)clickableComponent.Y + 32), new Vector2(this.xPositionOnScreen - Game1.pixelZoom * 9, this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4), 0.5f)
                        });
                    }
                    if (obj is Object && (obj as Object).edibility != -300)
                    {
                        for (int index = 0; index < obj.Stack; ++index)
                        {
                            if (Game1.random.NextDouble() < 0.04)
                                (Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(obj.getOne());
                        }
                    }
                    Game1.playSound("sell");
                    Game1.playSound("purchase");
                    if (this.inventory.getItemAt(x, y) == null)
                        this.animations.Add(new TemporaryAnimatedSprite(5, clickableComponent + new Vector2(32f, 32f), Color.White)
                        {
                            motion = new Vector2(0.0f, -0.5f)
                        });
                }
            }
            else
                this.heldItem = this.inventory.leftClick(x, y, this.heldItem);
            for (int index1 = 0; index1 < this.forSaleButtons.Count; ++index1)
            {

                if (this.currentItemIndex + index1 < this.forSale.Count && this.forSaleButtons[index1].containsPoint(x, y))
                {
                    int index2 = this.currentItemIndex + index1;
                    if (this.forSale[index2] != null)
                    {
                        int numberToBuy = Math.Min(Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu2.getPlayerCurrencyAmount(Game1.player, this.currency) / Math.Max(1, this.itemPriceAndStock[this.forSale[index2]][0])), Math.Max(1, this.itemPriceAndStock[this.forSale[index2]][1])) : 1, this.forSale[index2].maximumStackSize());
                        if (numberToBuy == -1)
                            numberToBuy = 1;
                        if (numberToBuy > 0 && this.tryToPurchaseItem(this.forSale[index2], this.heldItem, numberToBuy, x, y, index2))
                        {
                            this.itemPriceAndStock.Remove(this.forSale[index2]);
                            this.forSale.RemoveAt(index2);
                        }
                        else if (numberToBuy <= 0)
                        {
                            Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
                            Game1.playSound("cancel");
                        }
                    }
                    this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
                    return;
                }
            }
            if (!this.readyToClose() || x >= this.xPositionOnScreen - Game1.tileSize && y >= this.yPositionOnScreen - Game1.tileSize && (x <= this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y <= this.yPositionOnScreen + this.height + Game1.tileSize))
                return;
            this.exitThisMenu();
        }

        public override bool readyToClose()
        {
            if (this.heldItem == null)
                return this.animations.Count == 0;
            return false;
        }

        public override void emergencyShutDown()
        {
            base.emergencyShutDown();
            if (this.heldItem == null)
                return;
            Game1.player.addItemToInventoryBool(this.heldItem);
            Game1.playSound("coin");
        }

        public static void chargePlayer(Farmer who, int currencyType, int amount)
        {
            switch (currencyType)
            {
                case 0:
                    who.Money -= amount;
                    break;
                case 1:
                    who.festivalScore -= amount;
                    break;
                case 2:
                    who.clubCoins -= amount;
                    break;
            }
        }

        private bool tryToPurchaseItem(Item item, Item heldItem, int numberToBuy, int x, int y, int indexInForSaleList)
        {
            if (heldItem == null)
            {
                int amount = this.itemPriceAndStock[item][0] * numberToBuy;
                int num = -1;
                if (this.itemPriceAndStock[item].Length > 2)
                    num = this.itemPriceAndStock[item][2];
                if (ShopMenu2.getPlayerCurrencyAmount(Game1.player, this.currency) >= amount && (num == -1 || Game1.player.hasItemInInventory(num, 5)))
                {
                    this.heldItem = item.getOne();
                    this.heldItem.Stack = numberToBuy;
                    if (!Game1.player.couldInventoryAcceptThisItem(this.heldItem))
                    {
                        Game1.playSound("smallSelect");
                        this.heldItem = null;
                        return false;
                    }
                    if (this.itemPriceAndStock[item][1] != int.MaxValue)
                    {
                        this.itemPriceAndStock[item][1] -= numberToBuy;
                        this.forSale[indexInForSaleList].Stack -= numberToBuy;
                    }
                    ShopMenu2.chargePlayer(Game1.player, this.currency, amount);
                    if (num != -1)
                        Game1.player.removeItemsFromInventory(num, 5);
                    if (item.actionWhenPurchased())
                    {
                        if (this.heldItem is Object && (this.heldItem as Object).isRecipe)
                        {
                            if ((this.heldItem as Object).name.Contains("Bundle"))
                            {
                                ItemUtils.addBundle((this.heldItem as Object).specialVariable);
                                Game1.playSound("newRecipe");
                                heldItem = null;
                                this.heldItem = null;
                            }
                            else
                            {
                                string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
                                try
                                {
                                    if ((this.heldItem as Object).category == -7)
                                    {
                                        Game1.player.cookingRecipes.Add(key, 0);

                                        if (this.locationName.Equals("Recipe") && PelicanFiber.PelicanFiber.giveAchievements)
                                        {
                                            Game1.player.cookedRecipe(item.parentSheetIndex);
                                            Game1.stats.checkForCookingAchievements();
                                        }
                                    }
                                    else
                                        Game1.player.craftingRecipes.Add(key, 0);
                                    Game1.playSound("newRecipe");
                                }
                                catch (Exception ex)
                                {
                                }
                                heldItem = null;
                                this.heldItem = null;
                            }
                        }
                    }
                    else if (Game1.mouseClickPolling > 300)
                        Game1.playSound("purchaseRepeat");
                    else
                        Game1.playSound("purchaseClick");
                }
                else
                {
                    Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
                    Game1.playSound("cancel");
                }
            }
            else if (heldItem.Name.Equals(item.Name))
            {
                numberToBuy = Math.Min(numberToBuy, heldItem.maximumStackSize() - heldItem.Stack);
                if (numberToBuy > 0)
                {
                    int amount = this.itemPriceAndStock[item][0] * numberToBuy;
                    int index = -1;
                    if (this.itemPriceAndStock[item].Length > 2)
                        index = this.itemPriceAndStock[item][2];
                    if (ShopMenu2.getPlayerCurrencyAmount(Game1.player, this.currency) >= amount)
                    {
                        this.heldItem.Stack += numberToBuy;
                        if (this.itemPriceAndStock[item][1] != int.MaxValue)
                            this.itemPriceAndStock[item][1] -= numberToBuy;
                        ShopMenu2.chargePlayer(Game1.player, this.currency, amount);
                        if (Game1.mouseClickPolling > 300)
                            Game1.playSound("purchaseRepeat");
                        else
                            Game1.playSound("purchaseClick");
                        if (index != -1)
                            Game1.player.removeItemsFromInventory(index, 5);
                        if (item.actionWhenPurchased())
                            this.heldItem = null;
                    }
                    else
                    {
                        Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
                        Game1.playSound("cancel");
                    }
                }
            }
            if (this.itemPriceAndStock[item][1] > 0)
            {
                if (PelicanFiber.PelicanFiber.giveAchievements)
                {
                    if (this.locationName.Equals("FishShop"))
                    {
                        if (item.category == -4)
                            Game1.player.caughtFish(item.parentSheetIndex, 12);
                    }
                    else if (this.locationName.Equals("Artifact"))
                    {
                        if (item.category == -2)
                            Game1.player.foundMineral(item.parentSheetIndex);
                        else if (item.category == -12)
                            Game1.player.foundMineral(item.parentSheetIndex);
                        else if (item.category == 0)
                            Game1.player.foundArtifact(item.parentSheetIndex, numberToBuy);
                    }
                }

                return false;
            }
            this.hoveredItem = null;
            return true;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            Vector2 clickableComponent = this.inventory.snapToClickableComponent(x, y);
            if (this.heldItem == null)
            {
                Item obj1 = this.inventory.rightClick(x, y, null, false);
                if (obj1 != null)
                {
                    ShopMenu2.chargePlayer(Game1.player, this.currency, -((obj1 is Object ? (int)((obj1 as Object).sellToStorePrice() * (double)this.sellPercentage) : (int)(obj1.salePrice() / 2 * (double)this.sellPercentage)) * obj1.Stack));
                    Item obj2 = null;
                    if (Game1.mouseClickPolling > 300)
                        Game1.playSound("purchaseRepeat");
                    else
                        Game1.playSound("purchaseClick");
                    this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * Game1.tileSize, 256, Game1.tileSize, Game1.tileSize), 9999f, 1, 999, clickableComponent + new Vector2(32f, 32f), false, false)
                    {
                        alphaFade = 0.025f,
                        motion = Utility.getVelocityTowardPoint(new Point((int)clickableComponent.X + 32, (int)clickableComponent.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 12f),
                        acceleration = Utility.getVelocityTowardPoint(new Point((int)clickableComponent.X + 32, (int)clickableComponent.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 0.5f)
                    });
                    if (obj2 is Object && (obj2 as Object).edibility != -300 && Game1.random.NextDouble() < 0.04)
                        (Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(obj2.getOne());
                    if (this.inventory.getItemAt(x, y) == null)
                    {
                        Game1.playSound("sell");
                        this.animations.Add(new TemporaryAnimatedSprite(5, clickableComponent + new Vector2(32f, 32f), Color.White)
                        {
                            motion = new Vector2(0.0f, -0.5f)
                        });
                    }
                }
            }
            else
                this.heldItem = this.inventory.rightClick(x, y, this.heldItem);
            for (int index1 = 0; index1 < this.forSaleButtons.Count; ++index1)
            {
                if (this.currentItemIndex + index1 < this.forSale.Count && this.forSaleButtons[index1].containsPoint(x, y))
                {
                    int index2 = this.currentItemIndex + index1;
                    if (this.forSale[index2] == null)
                        break;
                    int numberToBuy = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu2.getPlayerCurrencyAmount(Game1.player, this.currency) / this.itemPriceAndStock[this.forSale[index2]][0]), this.itemPriceAndStock[this.forSale[index2]][1]) : 1;
                    if (numberToBuy <= 0 || !this.tryToPurchaseItem(this.forSale[index2], this.heldItem, numberToBuy, x, y, index2))
                        break;

                    this.itemPriceAndStock.Remove(this.forSale[index2]);
                    this.forSale.RemoveAt(index2);
                    break;
                }
            }
        }

        public override void performHoverAction(int x, int y)
        {
            this.backButton.tryHover(x, y, 1f);

            base.performHoverAction(x, y);
            this.descriptionText = "";
            this.hoverText = "";
            this.hoveredItem = null;
            this.hoverPrice = -1;
            this.boldTitleText = "";
            this.upArrow.tryHover(x, y);
            this.downArrow.tryHover(x, y);
            this.scrollBar.tryHover(x, y);
            if (this.scrolling)
                return;
            for (int index = 0; index < this.forSaleButtons.Count; ++index)
            {
                if (this.currentItemIndex + index < this.forSale.Count && this.forSaleButtons[index].containsPoint(x, y))
                {
                    Item key = this.forSale[this.currentItemIndex + index];
                    this.hoverText = key.getDescription();
                    if (key.category == -425)
                        this.boldTitleText = ((Object)key).name;
                    else
                        this.boldTitleText = key.Name;
                    this.hoverPrice = this.itemPriceAndStock == null || !this.itemPriceAndStock.ContainsKey(key) ? key.salePrice() : this.itemPriceAndStock[key][0];
                    this.hoveredItem = key;
                    this.forSaleButtons[index].scale = Math.Min(this.forSaleButtons[index].scale + 0.03f, 1.1f);
                }
                else
                    this.forSaleButtons[index].scale = Math.Max(1f, this.forSaleButtons[index].scale - 0.03f);
            }
            if (this.heldItem != null)
                return;
            foreach (ClickableComponent c in this.inventory.inventory)
            {
                if (c.containsPoint(x, y))
                {
                    Item clickableComponent = this.inventory.getItemFromClickableComponent(c);
                    if (clickableComponent != null && this.highlightItemToSell(clickableComponent))
                    {
                        if (clickableComponent.category == -425)
                            this.hoverText = ((Object)clickableComponent).name + " x " + clickableComponent.Stack;
                        else
                            this.hoverText = clickableComponent.Name + " x " + clickableComponent.Stack;
                        this.hoverPrice = (clickableComponent is Object ? (int)((clickableComponent as Object).sellToStorePrice() * (double)this.sellPercentage) : (int)(clickableComponent.salePrice() / 2 * (double)this.sellPercentage)) * clickableComponent.Stack;
                    }
                }
            }
        }

        public override void update(GameTime time)
        {
            base.update(time);
            if (this.poof == null || !this.poof.update(time))
                return;
            this.poof = null;
        }

        private void drawCurrency(SpriteBatch b)
        {
            switch (this.currency)
            {
                case 0:
                    Game1.dayTimeMoneyBox.drawMoneyBox(b, this.xPositionOnScreen - Game1.pixelZoom * 9, this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 3);
                    break;
            }
        }

        private int getHoveredItemExtraItemIndex()
        {
            if (this.itemPriceAndStock != null && this.hoveredItem != null && (this.itemPriceAndStock.ContainsKey(this.hoveredItem) && this.itemPriceAndStock[this.hoveredItem].Length > 2))
                return this.itemPriceAndStock[this.hoveredItem][2];
            return -1;
        }

        private int getHoveredItemExtraItemAmount()
        {
            return 5;
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.xPositionOnScreen = Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2;
            this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2;
            this.width = 1000 + IClickableMenu.borderWidth * 2;
            this.height = 600 + IClickableMenu.borderWidth * 2;
            this.initializeUpperRightCloseButton();
            if (Game1.viewport.Width < 1500)
                this.xPositionOnScreen = Game1.tileSize / 2;
            Game1.player.forceCanMove();
            this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, null, this.highlightItemToSell)
            {
                showGrayedOutSlots = true
            };
            this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
            int positionOnScreen1 = this.xPositionOnScreen;
            int borderWidth1 = IClickableMenu.borderWidth;
            int toClearSideBorder = IClickableMenu.spaceToClearSideBorder;
            int positionOnScreen2 = this.yPositionOnScreen;
            int borderWidth2 = IClickableMenu.borderWidth;
            int toClearTopBorder = IClickableMenu.spaceToClearTopBorder;
            this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
            this.forSaleButtons.Clear();
            for (int index = 0; index < 4; ++index)
                this.forSaleButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat(index)));
        }

        public override void draw(SpriteBatch b)
        {
            if (!Game1.options.showMenuBackground)
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen + this.width - this.inventory.width - Game1.tileSize / 2 - Game1.pixelZoom * 6, this.yPositionOnScreen + this.height - Game1.tileSize * 4 + Game1.pixelZoom * 10, this.inventory.width + Game1.pixelZoom * 14, this.height - Game1.tileSize * 7 + Game1.pixelZoom * 5, Color.White, Game1.pixelZoom);
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height - Game1.tileSize * 4 + Game1.tileSize / 2 + Game1.pixelZoom, Color.White, Game1.pixelZoom);
            this.drawCurrency(b);
            for (int index = 0; index < this.forSaleButtons.Count; ++index)
            {
                if (this.currentItemIndex + index < this.forSale.Count)
                {
                    IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.forSaleButtons[index].bounds.X, this.forSaleButtons[index].bounds.Y, this.forSaleButtons[index].bounds.Width, this.forSaleButtons[index].bounds.Height, !this.forSaleButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) || this.scrolling ? Color.White : Color.Wheat, Game1.pixelZoom, false);
                    b.Draw(Game1.mouseCursors, new Vector2(this.forSaleButtons[index].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 3, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 6 - Game1.pixelZoom), new Rectangle?(new Rectangle(296, 363, 18, 18)), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
                    this.forSale[this.currentItemIndex + index].drawInMenu(b, new Vector2(this.forSaleButtons[index].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 2, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 6), 1f);
                    if (this.forSale[this.currentItemIndex + index].category == -425)
                        SpriteText.drawString(b, ((Object)this.forSale[this.currentItemIndex + index]).name, this.forSaleButtons[index].bounds.X + Game1.tileSize * 3 / 2 + Game1.pixelZoom * 2, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 7);

                    else
                        SpriteText.drawString(b, this.forSale[this.currentItemIndex + index].Name, this.forSaleButtons[index].bounds.X + Game1.tileSize * 3 / 2 + Game1.pixelZoom * 2, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 7);
                    SpriteText.drawString(b, this.itemPriceAndStock[this.forSale[this.currentItemIndex + index]][0].ToString() + " ", this.forSaleButtons[index].bounds.Right - SpriteText.getWidthOfString(this.itemPriceAndStock[this.forSale[this.currentItemIndex + index]][0].ToString() + " ") - Game1.pixelZoom * 8, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 7, 999999, -1, 999999, ShopMenu2.getPlayerCurrencyAmount(Game1.player, this.currency) >= this.itemPriceAndStock[this.forSale[this.currentItemIndex + index]][0] ? 1f : 0.5f);
                    Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(this.forSaleButtons[index].bounds.Right - Game1.pixelZoom * 13, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 10 - Game1.pixelZoom), new Rectangle(193 + this.currency * 9, 373, 9, 10), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 1f);
                }
            }
            if (this.forSale.Count == 0)
                SpriteText.drawString(b, "Out of stock", this.xPositionOnScreen + this.width / 2 - SpriteText.getWidthOfString("Out of stock.") / 2, this.yPositionOnScreen + this.height / 2 - Game1.tileSize * 2);
            this.inventory.draw(b);
            for (int index = this.animations.Count - 1; index >= 0; --index)
            {
                if (this.animations[index].update(Game1.currentGameTime))
                    this.animations.RemoveAt(index);
                else
                    this.animations[index].draw(b, true);
            }
            if (this.poof != null)
                this.poof.draw(b);
            this.upArrow.draw(b);
            this.downArrow.draw(b);
            if (this.forSale.Count > 4)
            {
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, Game1.pixelZoom);
                this.scrollBar.draw(b);
            }
            if (!this.hoverText.Equals(""))
                IClickableMenu.drawToolTip(b, this.hoverText, this.boldTitleText, this.hoveredItem, this.heldItem != null, -1, this.currency, this.getHoveredItemExtraItemIndex(), this.getHoveredItemExtraItemAmount(), null, this.hoverPrice);
            if (this.heldItem != null)
                this.heldItem.drawInMenu(b, new Vector2(Game1.getOldMouseX() + 8, Game1.getOldMouseY() + 8), 1f);
            base.draw(b);
            if (Game1.viewport.Width > 1800 && Game1.options.showMerchantPortraits)
            {
                if (this.portraitPerson != null)
                {
                    Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(this.xPositionOnScreen - 80 * Game1.pixelZoom, this.yPositionOnScreen), new Rectangle(603, 414, 74, 74), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.91f);
                    if (this.portraitPerson.Portrait != null)
                        b.Draw(this.portraitPerson.Portrait, new Vector2(this.xPositionOnScreen - 80 * Game1.pixelZoom + Game1.pixelZoom * 5, this.yPositionOnScreen + Game1.pixelZoom * 5), new Rectangle?(new Rectangle(0, 0, 64, 64)), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.92f);
                }
                if (this.potraitPersonDialogue != null)
                    IClickableMenu.drawHoverText(b, this.potraitPersonDialogue, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, this.xPositionOnScreen - (int)Game1.dialogueFont.MeasureString(this.potraitPersonDialogue).X - Game1.tileSize, this.yPositionOnScreen + (this.portraitPerson != null ? 78 * Game1.pixelZoom : 0));
            }

            this.backButton.draw(b);

            this.drawMouse(b);
        }
    }
}
