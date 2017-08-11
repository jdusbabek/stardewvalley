using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Log = StardewLib.Log;
using Object = StardewValley.Object;

namespace MailOrderPigs
{
    public class MailOrderPigs : Mod
    {
        private static Keys menuKey = Keys.PageUp;

        private static MailOrderPigsConfig config;
        private bool allowOvercrowding = false;
        private bool enableLogging = false;


        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += onLoaded;
            ControlEvents.KeyReleased += onKeyReleased;
        }

        private void onLoaded(object sender, EventArgs e)
        {
            try
            {
                MailOrderPigs.config = this.Helper.ReadConfig<MailOrderPigsConfig>();

                if (!Enum.TryParse(config.keybind, true, out MailOrderPigs.menuKey))
                {
                    MailOrderPigs.menuKey = Keys.PageUp;
                    Log.force_ERROR("[MailOrderPigs] Error parsing key binding. Defaulted to Page Up");
                }

                this.allowOvercrowding = config.allowOvercrowding;
                this.enableLogging = config.enableLogging;

                Log.enabled = config.enableLogging;

                Log.force_INFO("[MailOrderPigs] Mod loaded successfully.");
            }
            catch (Exception ex)
            {
                Log.force_ERROR("[MailOrderPigs] Mod not loaded successfully: " + ex);
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

            if (e.KeyPressed == MailOrderPigs.menuKey)
            {
                Log.INFO("[MailOrderPigs] Attempting to bring up menu.");
                if (Game1.currentLocation is AnimalHouse)
                {
                    try
                    {
                        if (((AnimalHouse)Game1.currentLocation).isFull() && !allowOvercrowding)
                        {
                            Log.INFO("[MailOrderPigs] Not bringing up menu: building is full.");
                            Game1.showRedMessage("This Building Is Full");
                        }
                        else
                        {
                            Log.INFO("[MailOrderPigs] Bringing up menu.");
                            Game1.activeClickableMenu = new MailOrderPigMenu(getPurchaseAnimalStock());
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.force_ERROR("[MailOrderPigs] Problem bringing up menu: " + ex);
                    }
                }
                else
                {
                    Log.INFO("[MailOrderPigs] Problem bringing up menu: you are not in an animal house. Location name is: " + Game1.currentLocation.Name);
                }
            }
        }

        private static List<Object> getPurchaseAnimalStock()
        {
            //string locationName = ((AnimalHouse)Game1.currentLocation).Name;
            string locationName = ((AnimalHouse)Game1.currentLocation).getBuilding().buildingType;
            Log.INFO("[MailOrderPigs] Returning stock for building: " + locationName);

            return new List<Object>() {
                new Object(100, 1, false, 400, 0){ name = "Chicken", type = locationName.Equals("Coop") || locationName.Equals("Deluxe Coop") || locationName.Equals("Big Coop") ? null : "You gotta be in a Coop" },
                new Object(100, 1, false, 750, 0) { name = "Dairy Cow", type = locationName.Equals("Barn") || locationName.Equals("Deluxe Barn") || locationName.Equals("Big Barn") ? null : "You gotta be in a Barn" },
                new Object(100, 1, false, 2000, 0){ name = "Goat", type = locationName.Equals("Big Barn") || locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Big Barn" },
                new Object(100, 1, false, 2000, 0) { name = "Duck", type = locationName.Equals("Big Coop") || locationName.Equals("Deluxe Coop") ? null : "You gotta be in a Big Coop" },
                new Object(100, 1, false, 4000, 0) { name = "Sheep", type = locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Deluxe Barn" },
                new Object(100, 1, false, 4000, 0) { name = "Rabbit", type = locationName.Equals("Deluxe Coop") ? null : "You gotta be in a Deluxe Coop" },
                new Object(100, 1, false, 8000, 0){ name = "Pig", type = locationName.Equals("Deluxe Barn") ? null : "You gotta be in a Deluxe Barn" } };
        }
    }
}
