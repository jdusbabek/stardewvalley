using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Object = StardewValley.Object;

namespace MailOrderPigs
{
    public class MailOrderPigs : Mod
    {
        /*********
        ** Properties
        *********/
        private Keys menuKey = Keys.PageUp;
        private MailOrderPigsConfig config;
        private bool allowOvercrowding = false;
        private bool enableLogging = false;


        /*********
        ** Public methods
        *********/
        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += onLoaded;
            ControlEvents.KeyReleased += onKeyReleased;
        }


        /*********
        ** Private methods
        *********/
        private void onLoaded(object sender, EventArgs e)
        {
            try
            {
                this.config = this.Helper.ReadConfig<MailOrderPigsConfig>();

                if (!Enum.TryParse(config.keybind, true, out this.menuKey))
                {
                    this.menuKey = Keys.PageUp;
                    this.Monitor.Log("Error parsing key binding. Defaulted to Page Up");
                }

                this.allowOvercrowding = config.allowOvercrowding;
                this.enableLogging = config.enableLogging;

                this.Monitor.Log("Mod loaded successfully.", LogLevel.Trace);
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Mod not loaded successfully: {ex}", LogLevel.Error);
            }

        }

        private void onKeyReleased(object sender, EventArgsKeyPressed e)
        {
            if (Game1.currentLocation == null
                || (Game1.player == null
                || Game1.hasLoadedGame == false)
                || ((Game1.player).UsingTool
                || !(Game1.player).CanMove
                || (Game1.activeClickableMenu != null
                || Game1.CurrentEvent != null))
                || Game1.gameMode != 3)
            {
                return;
            }

            if (e.KeyPressed == this.menuKey)
            {
                this.Monitor.Log("Attempting to bring up menu.", LogLevel.Trace);
                if (Game1.currentLocation is AnimalHouse)
                {
                    try
                    {
                        if (((AnimalHouse)Game1.currentLocation).isFull() && !allowOvercrowding)
                        {
                            this.Monitor.Log("Not bringing up menu: building is full.", LogLevel.Trace);
                            Game1.showRedMessage("This Building Is Full");
                        }
                        else
                        {
                            this.Monitor.Log("Bringing up menu.", LogLevel.Trace);
                            Game1.activeClickableMenu = new MailOrderPigMenu(this.getPurchaseAnimalStock());
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Monitor.Log($"Problem bringing up menu: {ex}", LogLevel.Error);
                    }
                }
                else
                {
                    this.Monitor.Log($"Problem bringing up menu: you are not in an animal house. Location name is: {Game1.currentLocation.Name}", LogLevel.Trace);
                }
            }
        }

        private List<Object> getPurchaseAnimalStock()
        {
            //string locationName = ((AnimalHouse)Game1.currentLocation).Name;
            string locationName = ((AnimalHouse)Game1.currentLocation).getBuilding().buildingType;
            this.Monitor.Log($"Returning stock for building: {locationName}", LogLevel.Trace);

            return new List<Object>
            {
                new Object(100, 1, false, 400) { name = "Chicken", type = locationName.Equals("Coop") || locationName.Equals("Deluxe Coop") || locationName.Equals("Big Coop") ? null : "You gotta be in a Coop" },
                new Object(100, 1, false, 750) { name = "Dairy Cow", type = locationName.Equals("Barn") || locationName.Equals("Deluxe Barn") || locationName.Equals("Big Barn") ? null : "You gotta be in a Barn" },
                new Object(100, 1, false, 2000) { name = "Goat", type = locationName.Equals("Big Barn") || locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Big Barn" },
                new Object(100, 1, false, 2000) { name = "Duck", type = locationName.Equals("Big Coop") || locationName.Equals("Deluxe Coop") ? null : "You gotta be in a Big Coop" },
                new Object(100, 1, false, 4000) { name = "Sheep", type = locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Deluxe Barn" },
                new Object(100, 1, false, 4000) { name = "Rabbit", type = locationName.Equals("Deluxe Coop") ? null : "You gotta be in a Deluxe Coop" },
                new Object(100, 1, false, 8000) { name = "Pig", type = locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Deluxe Barn" }
            };
        }
    }
}
