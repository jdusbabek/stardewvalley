using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PelicanFiber;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace StardewValley.Menus
{
    internal class MailOrderPigMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        private static int MenuHeight = Game1.tileSize * 5;
        private static int MenuWidth = Game1.tileSize * 7;
        private List<ClickableTextureComponent> AnimalsToPurchase = new List<ClickableTextureComponent>();
        private ClickableTextureComponent OkButton;
        private ClickableTextureComponent DoneNamingButton;
        private ClickableTextureComponent RandomButton;
        private ClickableTextureComponent Hovered;
        private ClickableTextureComponent BackButton;
        //private bool onFarm;
        private bool NamingAnimal;
        private bool Freeze;
        private FarmAnimal AnimalBeingPurchased;
        private TextBox TextBox;
        private TextBoxEvent TextBoxEvent;
        private Building NewAnimalHome;
        private int PriceOfAnimal;
        private bool Condition = false;
        private ItemUtils ItemUtils;
        private Action ShowMainMenu;


        /*********
        ** Public methods
        *********/
        public MailOrderPigMenu(List<Object> stock, ItemUtils itemUtils, Action showMainMenu)
          : base(Game1.viewport.Width / 2 - MailOrderPigMenu.MenuWidth / 2 - IClickableMenu.borderWidth * 2, Game1.viewport.Height / 2 - MailOrderPigMenu.MenuHeight - IClickableMenu.borderWidth * 2, MailOrderPigMenu.MenuWidth + IClickableMenu.borderWidth * 2, MailOrderPigMenu.MenuHeight + IClickableMenu.borderWidth)
        {
            this.ItemUtils = itemUtils;
            this.ShowMainMenu = showMainMenu;

            this.height += Game1.tileSize;
            for (int index = 0; index < stock.Count; ++index)
            {
                List<ClickableTextureComponent> animalsToPurchase = this.AnimalsToPurchase;
                ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(string.Concat(stock[index].salePrice()), new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + index % 3 * Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + index / 3 * (Game1.tileSize + Game1.tileSize / 3), Game1.tileSize * 2, Game1.tileSize), null, stock[index].Name, Game1.mouseCursors, new Rectangle(index % 3 * 16 * 2, 448 + index / 3 * 16, 32, 16), 4f, stock[index].type == null);
                textureComponent1.item = stock[index];
                ClickableTextureComponent textureComponent2 = textureComponent1;
                animalsToPurchase.Add(textureComponent2);
            }
            this.OkButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47), 1f);
            this.RandomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 4 / 5 + Game1.tileSize, Game1.viewport.Height / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), Game1.pixelZoom);
            MailOrderPigMenu.MenuHeight = Game1.tileSize * 5;
            MailOrderPigMenu.MenuWidth = Game1.tileSize * 7;
            this.TextBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
            this.TextBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
            this.TextBox.Y = Game1.viewport.Height / 2;
            this.TextBox.Width = Game1.tileSize * 4;
            this.TextBox.Height = Game1.tileSize * 3;
            this.TextBoxEvent = this.TextBoxEnter;
            this.RandomButton = new ClickableTextureComponent(new Rectangle(this.TextBox.X + this.TextBox.Width + Game1.tileSize + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2, Game1.viewport.Height / 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), Game1.pixelZoom);
            this.DoneNamingButton = new ClickableTextureComponent(new Rectangle(this.TextBox.X + this.TextBox.Width + Game1.tileSize / 2 + Game1.pixelZoom, Game1.viewport.Height / 2 - Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f);
            this.BackButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - 10, this.yPositionOnScreen + 10, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), Game1.pixelZoom);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (Game1.globalFade || this.Freeze)
                return;

            if (this.BackButton.containsPoint(x, y))
            {
                this.BackButton.scale = this.BackButton.baseScale;
                this.BackButtonPressed();
            }

            if (this.OkButton != null && this.OkButton.containsPoint(x, y) && this.readyToClose())
            {
                if (this.NamingAnimal)
                {
                    Game1.globalFadeToBlack(this.SetUpForReturnToShopMenu);
                    Game1.playSound("smallSelect");
                }
                else
                {
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
            }

            if (this.NamingAnimal)
            {
                this.TextBox.OnEnterPressed += this.TextBoxEvent;
                Game1.keyboardDispatcher.Subscriber = this.TextBox;
                //this.textBox.Text = this.animalBeingPurchased.name;
                this.TextBox.Selected = true;

                if (this.DoneNamingButton.containsPoint(x, y))
                {
                    this.AnimalBeingPurchased.name = this.TextBox.Text;
                    this.TextBoxEnter(this.TextBox);
                    Game1.playSound("smallSelect");
                }
                else
                {
                    if (this.RandomButton.containsPoint(x, y))
                    {
                        this.AnimalBeingPurchased.name = Dialogue.randomName();
                        this.TextBox.Text = this.AnimalBeingPurchased.name;
                        this.RandomButton.scale = this.RandomButton.baseScale;
                        Game1.playSound("drumkit6");
                    }
                }
            }

            foreach (ClickableTextureComponent textureComponent in this.AnimalsToPurchase)
            {
                if (textureComponent.containsPoint(x, y) && (textureComponent.item as Object).type == null)
                {
                    int int32 = Convert.ToInt32(textureComponent.name);
                    if (Game1.player.money >= int32)
                    {
                        //Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForAnimalPlacement), 0.02f);
                        //Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForAnimalPlacement), 0.02f);
                        Game1.playSound("smallSelect");
                        //this.onFarm = true;
                        this.AnimalBeingPurchased = new FarmAnimal(textureComponent.hoverText, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
                        this.PriceOfAnimal = int32;

                        //this.newAnimalHome = ((AnimalHouse)Game1.player.currentLocation).getBuilding();
                        //this.animalBeingPurchased.name = "John" + new Random().NextDouble();
                        //this.animalBeingPurchased.home = this.newAnimalHome;
                        //this.animalBeingPurchased.homeLocation = new Vector2((float)this.newAnimalHome.tileX, (float)this.newAnimalHome.tileY);
                        //this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
                        //(this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
                        //(this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
                        //this.newAnimalHome = (Building)null;
                        //this.namingAnimal = false;
                        //Game1.player.money -= this.priceOfAnimal;

                        //Game1.exitActiveMenu();
                        this.NamingAnimal = true;
                    }
                    else
                        Game1.addHUDMessage(new HUDMessage("Not Enough Money", Color.Red, 3500f));
                }
            }

        }

        public override void receiveKeyPress(Keys key)
        {
            if (Game1.globalFade || this.Freeze)
                return;

            if (!Game1.options.doesInputListContain(Game1.options.menuButton, key) || Game1.globalFade || !this.readyToClose())
                return;

            if (this.NamingAnimal)
                return;

            Game1.player.forceCanMove();
            Game1.exitActiveMenu();
            Game1.playSound("bigDeSelect");
        }

        public override void update(GameTime time)
        {
            base.update(time);
            if (this.NamingAnimal)
                return;

            //int num1 = Game1.getOldMouseX() + Game1.viewport.X;
            //int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
            //if (num1 - Game1.viewport.X < Game1.tileSize)
            //    Game1.panScreen(-8, 0);
            //else if (num1 - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
            //    Game1.panScreen(8, 0);
            //if (num2 - Game1.viewport.Y < Game1.tileSize)
            //    Game1.panScreen(0, -8);
            //else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
            //    Game1.panScreen(0, 8);
            //foreach (Keys pressedKey in Game1.oldKBState.GetPressedKeys())
            //    this.receiveKeyPress(pressedKey);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void performHoverAction(int x, int y)
        {
            this.Hovered = null;
            if (Game1.globalFade || this.Freeze)
                return;
            if (this.OkButton != null)
            {
                if (this.OkButton.containsPoint(x, y))
                    this.OkButton.scale = Math.Min(1.1f, this.OkButton.scale + 0.05f);
                else
                    this.OkButton.scale = Math.Max(1f, this.OkButton.scale - 0.05f);
            }

            if (this.NamingAnimal)
            {
                if (this.DoneNamingButton != null)
                {
                    if (this.DoneNamingButton.containsPoint(x, y))
                        this.DoneNamingButton.scale = Math.Min(1.1f, this.DoneNamingButton.scale + 0.05f);
                    else
                        this.DoneNamingButton.scale = Math.Max(1f, this.DoneNamingButton.scale - 0.05f);
                }
                this.RandomButton.tryHover(x, y, 0.5f);
            }
            else
            {
                foreach (ClickableTextureComponent textureComponent in this.AnimalsToPurchase)
                {
                    if (textureComponent.containsPoint(x, y))
                    {
                        textureComponent.scale = Math.Min(textureComponent.scale + 0.05f, 4.1f);
                        this.Hovered = textureComponent;
                    }
                    else
                        textureComponent.scale = Math.Max(4f, textureComponent.scale - 0.025f);
                }
            }
        }

        public override void draw(SpriteBatch b)
        {
            if (!Game1.dialogueUp && !Game1.globalFade)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
                SpriteText.drawStringWithScrollBackground(b, "Livestock:", this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen);
                Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
                Game1.dayTimeMoneyBox.drawMoneyBox(b);
                foreach (ClickableTextureComponent textureComponent in this.AnimalsToPurchase)
                    textureComponent.draw(b, (textureComponent.item as Object).type != null ? Color.Black * 0.4f : Color.White, 0.87f);

                this.BackButton.draw(b);
            }
            if (!Game1.globalFade && this.OkButton != null)
                this.OkButton.draw(b);

            if (this.NamingAnimal)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
                Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 4, Game1.viewport.Height / 2 - Game1.tileSize * 3 - Game1.tileSize / 2, Game1.tileSize * 8, Game1.tileSize * 3, false, true);
                Utility.drawTextWithShadow(b, "Name your new animal: ", Game1.dialogueFont, new Vector2(Game1.viewport.Width / 2 - Game1.tileSize * 4 + Game1.tileSize / 2 + 8, Game1.viewport.Height / 2 - Game1.tileSize * 2 + 8), Game1.textColor);
                this.TextBox.Draw(b);
                this.DoneNamingButton.draw(b);
                this.RandomButton.draw(b);
            }

            if (this.Hovered != null)
            {
                if ((this.Hovered.item as Object).type != null)
                {
                    IClickableMenu.drawHoverText(b, Game1.parseText((this.Hovered.item as Object).type, Game1.dialogueFont, Game1.tileSize * 5), Game1.dialogueFont);
                }
                else
                {
                    SpriteText.drawStringWithScrollBackground(b, this.Hovered.hoverText, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize, this.yPositionOnScreen + this.height + -Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder / 2 + 8, "Truffle Pig");
                    SpriteText.drawStringWithScrollBackground(b, "$" + this.Hovered.name + "g", this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, this.yPositionOnScreen + this.height + Game1.tileSize + IClickableMenu.spaceToClearTopBorder / 2 + 8, "$99999g", Game1.player.Money >= Convert.ToInt32(this.Hovered.name) ? 1f : 0.5f);
                    IClickableMenu.drawHoverText(b, Game1.parseText(this.GetAnimalDescription(this.Hovered.hoverText), Game1.smallFont, Game1.tileSize * 5), Game1.smallFont, 0, 0, -1, this.Hovered.hoverText);
                }
            }
            this.drawMouse(b);
        }


        /*********
        ** Private methods
        *********/
        private void TextBoxEnter(TextBox sender)
        {
            if (!this.NamingAnimal)
                return;
            if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is MailOrderPigMenu))
            {
                this.TextBox.OnEnterPressed -= this.TextBoxEvent;
            }
            else
            {
                if (sender.Text.Length < 1)
                    return;
                if (Utility.areThereAnyOtherAnimalsWithThisName(sender.Text))
                {
                    Game1.showRedMessage("Name Unavailable");
                }
                else
                {
                    this.NewAnimalHome = ((AnimalHouse)Game1.player.currentLocation).getBuilding();
                    this.TextBox.OnEnterPressed -= this.TextBoxEvent;
                    this.AnimalBeingPurchased.name = sender.Text;
                    //StardewLib.Log.ERROR("Named Animal: " + sender.Text);
                    this.AnimalBeingPurchased.home = ((AnimalHouse)Game1.player.currentLocation).getBuilding();
                    this.AnimalBeingPurchased.homeLocation = new Vector2(this.NewAnimalHome.tileX, this.NewAnimalHome.tileY);
                    this.AnimalBeingPurchased.setRandomPosition(this.AnimalBeingPurchased.home.indoors);
                    (this.NewAnimalHome.indoors as AnimalHouse).animals.Add(this.AnimalBeingPurchased.myID, this.AnimalBeingPurchased);
                    (this.NewAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.AnimalBeingPurchased.myID);
                    this.NewAnimalHome = null;
                    Game1.player.money -= this.PriceOfAnimal;
                    this.NamingAnimal = false;

                    //Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnAfterPurchasingAnimal), 0.02f);
                    Game1.globalFadeToClear();
                    this.OkButton.bounds.X = this.xPositionOnScreen + this.width + 4;
                    Game1.displayHUD = true;
                    Game1.displayFarmer = true;
                    this.Freeze = false;
                    this.TextBox.OnEnterPressed -= this.TextBoxEvent;
                    this.TextBox.Selected = false;
                    Game1.viewportFreeze = false;
                    Game1.globalFadeToClear(this.MarnieAnimalPurchaseMessage);
                }
            }
        }

        private void SetUpForReturnToShopMenu()
        {
            Game1.globalFadeToClear();
            this.OkButton.bounds.X = this.xPositionOnScreen + this.width + 4;
            this.OkButton.bounds.Y = this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth;
            Game1.displayHUD = true;
            Game1.viewportFreeze = false;
            this.NamingAnimal = false;
            this.TextBox.OnEnterPressed -= this.TextBoxEvent;
            this.TextBox.Selected = false;
        }

        private void SetUpForReturnAfterPurchasingAnimal()
        {
            Game1.globalFadeToClear();
            this.OkButton.bounds.X = this.xPositionOnScreen + this.width + 4;
            Game1.displayHUD = true;
            Game1.displayFarmer = true;
            this.Freeze = false;
            this.TextBox.OnEnterPressed -= this.TextBoxEvent;
            this.TextBox.Selected = false;
            Game1.viewportFreeze = false;
            Game1.globalFadeToClear(this.MarnieAnimalPurchaseMessage);
        }

        private void MarnieAnimalPurchaseMessage()
        {
            this.exitThisMenu();
            Game1.player.forceCanMove();
            this.Freeze = false;

            Game1.activeClickableMenu = new MailOrderPigMenu(this.ItemUtils.GetPurchaseAnimalStock(), this.ItemUtils, this.ShowMainMenu);
        }

        private void BackButtonPressed()
        {
            if (this.readyToClose())
            {
                this.exitThisMenu();
                this.ShowMainMenu();
            }
        }

        private string GetAnimalDescription(string name)
        {
            switch (name)
            {
                case "Chicken":
                    return "Well cared-for adult chickens lay eggs every day." + Environment.NewLine + "Lives in the coop.";
                case "Duck":
                    return "Happy adults lay duck eggs every other day." + Environment.NewLine + "Lives in the coop.";
                case "Rabbit":
                    return "These are wooly rabbits! They shed precious wool every few days." + Environment.NewLine + "Lives in the coop.";
                case "Dairy Cow":
                    return "Adults can be milked daily. A milk pail is required to harvest the milk." + Environment.NewLine + "Lives in the barn.";
                case "Pig":
                    return "These pigs are trained to find truffles!" + Environment.NewLine + "Lives in the barn.";
                case "Goat":
                    return "Happy adults provide goat milk every other day. A milk pail is required to harvest the milk." + Environment.NewLine + "Lives in the barn.";
                case "Sheep":
                    return "Adults can be shorn for wool. Sheep who form a close bond with their owners can grow wool faster. A pair of shears is required to harvest the wool." + Environment.NewLine + "Lives in the barn.";
                default:
                    return "";
            }
        }
    }
}
