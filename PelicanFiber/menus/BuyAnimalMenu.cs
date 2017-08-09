using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace StardewValley.Menus
{
    internal class BuyAnimalMenu : IClickableMenu
    {
        public string whereToGo = "";

        public static int menuHeight = Game1.tileSize * 5;
        public static int menuWidth = Game1.tileSize * 7;
        private List<ClickableTextureComponent> animalsToPurchase = new List<ClickableTextureComponent>();
        private ClickableTextureComponent okButton;
        private ClickableTextureComponent doneNamingButton;
        private ClickableTextureComponent randomButton;
        private ClickableTextureComponent hovered;
        private ClickableTextureComponent backButton;
        private bool onFarm;
        private bool namingAnimal;
        private bool freeze;
        private FarmAnimal animalBeingPurchased;
        private TextBox textBox;
        private TextBoxEvent e;
        private Building newAnimalHome;
        private int priceOfAnimal;

        public BuyAnimalMenu(List<Object> stock)
          : base(Game1.viewport.Width / 2 - BuyAnimalMenu.menuWidth / 2 - IClickableMenu.borderWidth * 2, Game1.viewport.Height / 2 - BuyAnimalMenu.menuHeight - IClickableMenu.borderWidth * 2, BuyAnimalMenu.menuWidth + IClickableMenu.borderWidth * 2, BuyAnimalMenu.menuHeight + IClickableMenu.borderWidth)
        {
            this.whereToGo = Game1.player.currentLocation.Name;

            this.height += Game1.tileSize;
            for (int index = 0; index < stock.Count; ++index)
            {
                List<ClickableTextureComponent> animalsToPurchase = this.animalsToPurchase;
                ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(string.Concat(stock[index].salePrice()), new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + index % 3 * Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + index / 3 * (Game1.tileSize + Game1.tileSize / 3), Game1.tileSize * 2, Game1.tileSize), null, stock[index].Name, Game1.mouseCursors, new Rectangle(index % 3 * 16 * 2, 448 + index / 3 * 16, 32, 16), 4f, stock[index].type == null);
                textureComponent1.item = stock[index];
                ClickableTextureComponent textureComponent2 = textureComponent1;
                animalsToPurchase.Add(textureComponent2);
            }
            this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47), 1f);
            this.randomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 4 / 5 + Game1.tileSize, Game1.viewport.Height / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), Game1.pixelZoom);
            BuyAnimalMenu.menuHeight = Game1.tileSize * 5;
            BuyAnimalMenu.menuWidth = Game1.tileSize * 7;
            this.textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
            this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
            this.textBox.Y = Game1.viewport.Height / 2;
            this.textBox.Width = Game1.tileSize * 4;
            this.textBox.Height = Game1.tileSize * 3;
            this.e = this.textBoxEnter;
            this.randomButton = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2, Game1.viewport.Height / 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), Game1.pixelZoom);
            this.doneNamingButton = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize / 2 + Game1.pixelZoom, Game1.viewport.Height / 2 - Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f);
            this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - 10, this.yPositionOnScreen + 10, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), Game1.pixelZoom);

        }

        private void textBoxEnter(TextBox sender)
        {
            if (!this.namingAnimal)
                return;
            if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is BuyAnimalMenu))
            {
                this.textBox.OnEnterPressed -= this.e;
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
                    this.textBox.OnEnterPressed -= this.e;
                    this.animalBeingPurchased.name = sender.Text;
                    this.animalBeingPurchased.home = this.newAnimalHome;
                    this.animalBeingPurchased.homeLocation = new Vector2(this.newAnimalHome.tileX, this.newAnimalHome.tileY);
                    this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
                    (this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
                    (this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
                    this.newAnimalHome = null;
                    this.namingAnimal = false;
                    Game1.player.money -= this.priceOfAnimal;
                    Game1.globalFadeToBlack(this.setUpForReturnAfterPurchasingAnimal);
                }
            }
        }

        private void setUpForReturnAfterPurchasingAnimal()
        {
            Game1.currentLocation.cleanupBeforePlayerExit();
            //Game1.currentLocation = Game1.getLocationFromName("AnimalShop");
            Game1.currentLocation = Game1.getLocationFromName(whereToGo);
            Game1.currentLocation.resetForPlayerEntry();
            Game1.globalFadeToClear();
            this.onFarm = false;
            this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
            Game1.displayHUD = true;
            Game1.displayFarmer = true;
            this.freeze = false;
            this.textBox.OnEnterPressed -= this.e;
            this.textBox.Selected = false;
            Game1.viewportFreeze = false;
            Game1.globalFadeToClear(this.marnieAnimalPurchaseMessage);
        }

        //public void marnieAnimalPurchaseMessage()
        //{
        //    this.exitThisMenu(true);
        //    //Game1.player.position = this.whereAt;
        //    Game1.player.forceCanMove();
        //    this.freeze = false;
        //    //Game1.drawDialogue(Game1.getCharacterFromName("Marnie"), "Great! I'll send little " + this.animalBeingPurchased.name + " to " + (this.animalBeingPurchased.isMale() ? "his" : "her") + " new home right away.");
        //}

        private void marnieAnimalPurchaseMessage()
        {
            this.exitThisMenu();
            Game1.player.forceCanMove();
            this.freeze = false;

            Game1.activeClickableMenu = PelicanFiber.PelicanFiber.getBuyAnimalMenu();
        }

        private void backButtonPressed()
        {
            if (this.readyToClose())
            {
                this.exitThisMenu();
                PelicanFiber.PelicanFiber.showTheMenu();
            }
        }

        private void setUpForAnimalPlacement()
        {
            Game1.displayFarmer = false;
            Game1.currentLocation = Game1.getLocationFromName("Farm");
            Game1.currentLocation.resetForPlayerEntry();
            Game1.globalFadeToClear();
            this.onFarm = true;
            this.freeze = false;
            this.okButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
            this.okButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
            Game1.displayHUD = false;
            Game1.viewportFreeze = true;
            Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
            Game1.panScreen(0, 0);
        }

        private void setUpForReturnToShopMenu()
        {
            this.freeze = false;
            Game1.displayFarmer = true;
            Game1.currentLocation.cleanupBeforePlayerExit();
            //Game1.currentLocation = Game1.getLocationFromName("AnimalShop");
            Game1.currentLocation.resetForPlayerEntry();
            Game1.globalFadeToClear();
            this.onFarm = false;
            this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
            this.okButton.bounds.Y = this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth;
            Game1.displayHUD = true;
            Game1.viewportFreeze = false;
            this.namingAnimal = false;
            this.textBox.OnEnterPressed -= this.e;
            this.textBox.Selected = false;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (Game1.globalFade || this.freeze)
                return;

            if (this.backButton != null && this.backButton.containsPoint(x, y))
            {
                this.backButton.scale = this.backButton.baseScale;
                backButtonPressed();
            }

            if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
            {
                if (this.onFarm)
                {
                    Game1.globalFadeToBlack(this.setUpForReturnToShopMenu);
                    Game1.playSound("smallSelect");
                }
                else
                {
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
            }
            if (this.onFarm)
            {
                Building buildingAt = (Game1.getLocationFromName("Farm") as Farm).getBuildingAt(new Vector2((x + Game1.viewport.X) / Game1.tileSize, (y + Game1.viewport.Y) / Game1.tileSize));
                if (buildingAt != null && !this.namingAnimal)
                {
                    if (buildingAt.buildingType.Contains(this.animalBeingPurchased.buildingTypeILiveIn))
                    {
                        if ((buildingAt.indoors as AnimalHouse).isFull())
                            Game1.showRedMessage("That Building Is Full");
                        else if (this.animalBeingPurchased.harvestType != 2)
                        {
                            this.namingAnimal = true;
                            this.newAnimalHome = buildingAt;
                            if (this.animalBeingPurchased.sound != null && Game1.soundBank != null)
                            {
                                Cue cue = Game1.soundBank.GetCue(this.animalBeingPurchased.sound);
                                cue.SetVariable("Pitch", 1200 + Game1.random.Next(-200, 201));
                                cue.Play();
                            }
                            this.textBox.OnEnterPressed += this.e;
                            Game1.keyboardDispatcher.Subscriber = this.textBox;
                            this.textBox.Text = this.animalBeingPurchased.name;
                            this.textBox.Selected = true;
                        }
                        else if (Game1.player.money >= this.priceOfAnimal)
                        {
                            this.newAnimalHome = buildingAt;
                            this.animalBeingPurchased.home = this.newAnimalHome;
                            this.animalBeingPurchased.homeLocation = new Vector2(this.newAnimalHome.tileX, this.newAnimalHome.tileY);
                            this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
                            (this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
                            (this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
                            this.newAnimalHome = null;
                            this.namingAnimal = false;
                            if (this.animalBeingPurchased.sound != null && Game1.soundBank != null)
                            {
                                Cue cue = Game1.soundBank.GetCue(this.animalBeingPurchased.sound);
                                cue.SetVariable("Pitch", 1200 + Game1.random.Next(-200, 201));
                                cue.Play();
                            }
                            Game1.player.money -= this.priceOfAnimal;
                            Game1.addHUDMessage(new HUDMessage("Purchased " + this.animalBeingPurchased.type, Color.LimeGreen, 3500f));
                            this.animalBeingPurchased = new FarmAnimal(this.animalBeingPurchased.type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
                        }
                        else if (Game1.player.money < this.priceOfAnimal)
                            Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
                    }
                    else
                        Game1.showRedMessage(this.animalBeingPurchased.type.Split(' ').Last<string>() + "s Can't Live There.");
                }
                if (this.namingAnimal && this.doneNamingButton.containsPoint(x, y))
                {
                    this.textBoxEnter(this.textBox);
                    Game1.playSound("smallSelect");
                }
                else
                {
                    if (!this.namingAnimal || !this.randomButton.containsPoint(x, y))
                        return;
                    this.animalBeingPurchased.name = Dialogue.randomName();
                    this.textBox.Text = this.animalBeingPurchased.name;
                    this.randomButton.scale = this.randomButton.baseScale;
                    Game1.playSound("drumkit6");
                }
            }
            else
            {
                foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
                {
                    if (textureComponent.containsPoint(x, y) && (textureComponent.item as Object).type == null)
                    {
                        int int32 = Convert.ToInt32(textureComponent.name);
                        if (Game1.player.money >= int32)
                        {
                            Game1.globalFadeToBlack(this.setUpForAnimalPlacement);
                            Game1.playSound("smallSelect");
                            this.onFarm = true;
                            this.animalBeingPurchased = new FarmAnimal(textureComponent.hoverText, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
                            this.priceOfAnimal = int32;
                        }
                        else
                            Game1.addHUDMessage(new HUDMessage("Not Enough Money", Color.Red, 3500f));
                    }
                }
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if (Game1.globalFade || this.freeze)
                return;
            if (!Game1.globalFade && this.onFarm)
            {
                if (this.namingAnimal)
                    return;
                if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
                    Game1.globalFadeToBlack(this.setUpForReturnToShopMenu);
                else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
                    Game1.panScreen(0, 4);
                else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
                    Game1.panScreen(4, 0);
                else if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
                {
                    Game1.panScreen(0, -4);
                }
                else
                {
                    if (!Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
                        return;
                    Game1.panScreen(-4, 0);
                }
            }
            else
            {
                if (!Game1.options.doesInputListContain(Game1.options.menuButton, key) || Game1.globalFade || !this.readyToClose())
                    return;
                Game1.player.forceCanMove();
                Game1.exitActiveMenu();
                Game1.playSound("bigDeSelect");
            }
        }

        public override void update(GameTime time)
        {
            base.update(time);
            if (!this.onFarm || this.namingAnimal)
                return;
            int num1 = Game1.getOldMouseX() + Game1.viewport.X;
            int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
            if (num1 - Game1.viewport.X < Game1.tileSize)
                Game1.panScreen(-8, 0);
            else if (num1 - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
                Game1.panScreen(8, 0);
            if (num2 - Game1.viewport.Y < Game1.tileSize)
                Game1.panScreen(0, -8);
            else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
                Game1.panScreen(0, 8);
            foreach (Keys pressedKey in Game1.oldKBState.GetPressedKeys())
                this.receiveKeyPress(pressedKey);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void performHoverAction(int x, int y)
        {
            this.hovered = null;
            if (Game1.globalFade || this.freeze)
                return;
            if (this.okButton != null)
            {
                if (this.okButton.containsPoint(x, y))
                    this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
                else
                    this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
            }
            if (this.onFarm)
            {
                Vector2 tile = new Vector2((x + Game1.viewport.X) / Game1.tileSize, (y + Game1.viewport.Y) / Game1.tileSize);
                Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
                foreach (Building building in locationFromName.buildings)
                    building.color = Color.White;
                Building buildingAt = locationFromName.getBuildingAt(tile);
                if (buildingAt != null)
                    buildingAt.color = !buildingAt.buildingType.Contains(this.animalBeingPurchased.buildingTypeILiveIn) || (buildingAt.indoors as AnimalHouse).isFull() ? Color.Red * 0.8f : Color.LightGreen * 0.8f;
                if (this.doneNamingButton != null)
                {
                    if (this.doneNamingButton.containsPoint(x, y))
                        this.doneNamingButton.scale = Math.Min(1.1f, this.doneNamingButton.scale + 0.05f);
                    else
                        this.doneNamingButton.scale = Math.Max(1f, this.doneNamingButton.scale - 0.05f);
                }
                this.randomButton.tryHover(x, y, 0.5f);
            }
            else
            {
                foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
                {
                    if (textureComponent.containsPoint(x, y))
                    {
                        textureComponent.scale = Math.Min(textureComponent.scale + 0.05f, 4.1f);
                        this.hovered = textureComponent;
                    }
                    else
                        textureComponent.scale = Math.Max(4f, textureComponent.scale - 0.025f);
                }
            }
        }

        private static string getAnimalDescription(string name)
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

        public override void draw(SpriteBatch b)
        {
            if (!this.onFarm && !Game1.dialogueUp && !Game1.globalFade)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
                SpriteText.drawStringWithScrollBackground(b, "Livestock:", this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen);
                Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
                Game1.dayTimeMoneyBox.drawMoneyBox(b);
                foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
                    textureComponent.draw(b, (textureComponent.item as Object).type != null ? Color.Black * 0.4f : Color.White, 0.87f);

                this.backButton.draw(b);
            }
            else if (!Game1.globalFade && this.onFarm)
            {
                string s = "Choose a " + this.animalBeingPurchased.buildingTypeILiveIn + " for your new " + this.animalBeingPurchased.type.Split(' ').Last<string>();
                SpriteText.drawStringWithScrollBackground(b, s, Game1.viewport.Width / 2 - SpriteText.getWidthOfString(s) / 2, Game1.tileSize / 4);
                if (this.namingAnimal)
                {
                    b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
                    Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 4, Game1.viewport.Height / 2 - Game1.tileSize * 3 - Game1.tileSize / 2, Game1.tileSize * 8, Game1.tileSize * 3, false, true);
                    Utility.drawTextWithShadow(b, "Name your new animal: ", Game1.dialogueFont, new Vector2(Game1.viewport.Width / 2 - Game1.tileSize * 4 + Game1.tileSize / 2 + 8, Game1.viewport.Height / 2 - Game1.tileSize * 2 + 8), Game1.textColor);
                    this.textBox.Draw(b);
                    this.doneNamingButton.draw(b);
                    this.randomButton.draw(b);
                }
            }
            if (!Game1.globalFade && this.okButton != null)
                this.okButton.draw(b);
            if (this.hovered != null)
            {
                if ((this.hovered.item as Object).type != null)
                {
                    IClickableMenu.drawHoverText(b, Game1.parseText((this.hovered.item as Object).type, Game1.dialogueFont, Game1.tileSize * 5), Game1.dialogueFont);
                }
                else
                {
                    SpriteText.drawStringWithScrollBackground(b, this.hovered.hoverText, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize, this.yPositionOnScreen + this.height + -Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder / 2 + 8, "Truffle Pig");
                    SpriteText.drawStringWithScrollBackground(b, "$" + this.hovered.name + "g", this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, this.yPositionOnScreen + this.height + Game1.tileSize + IClickableMenu.spaceToClearTopBorder / 2 + 8, "$99999g", Game1.player.Money >= Convert.ToInt32(this.hovered.name) ? 1f : 0.5f);
                    IClickableMenu.drawHoverText(b, Game1.parseText(BuyAnimalMenu.getAnimalDescription(this.hovered.hoverText), Game1.smallFont, Game1.tileSize * 5), Game1.smallFont, 0, 0, -1, this.hovered.hoverText);
                }
            }
            this.drawMouse(b);
        }
    }
}
