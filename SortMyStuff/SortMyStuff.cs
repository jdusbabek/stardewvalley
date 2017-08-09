using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using ChestDef = SortMyStuff.chest.ChestDef;
using ChestManager = SortMyStuff.chest.ChestManager;
using Log = StardewLib.Log;
using Object = StardewValley.Object;


namespace SortMyStuff
{
    public class SortMyStuff : Mod
    {
        private static SortMyStuffConfig config;
        private static Keys actionKey;

        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += onLoaded;
            ControlEvents.KeyReleased += onKeyReleased;
        }


        private void onLoaded(object sender, EventArgs e)
        {
            try
            {
                SortMyStuff.config = new SortMyStuffConfig().InitializeConfig(this.BaseConfigPath);

                if (!Enum.TryParse(config.keybind, true, out SortMyStuff.actionKey))
                {
                    SortMyStuff.actionKey = Keys.G;
                    Log.force_INFO("[SMS] Error parsing key binding. Defaulted to G");
                }

                ChestManager.parseChests(config.chests);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex.ToString());
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
                return;

            if (e.KeyPressed == actionKey)
            {
                Log.INFO("Logging key stroke G!!!");
                List<ItemContainer> ic = new List<ItemContainer>();
                try
                {
                    foreach (Item item in Game1.player.Items)
                    {
                        if (item != null)
                        {
                            Log.INFO(item.Name + "/" + item.parentSheetIndex);
                            Object c = ChestManager.getChest(item.parentSheetIndex);
                            if (c != null && c is Chest)
                            {
                                ic.Add(new ItemContainer((Chest)c, item));
                            }
                            else
                            {
                                c = (Chest)ChestManager.getChest(item.category);
                                if (c != null && c is Chest)
                                {
                                    ic.Add(new ItemContainer((Chest)c, item));
                                }
                            }
                        }
                    }

                    foreach (ItemContainer i in ic)
                    {
                        Item o = i.chest.addItem(i.item);
                        if (o == null)
                            Game1.player.removeItemFromInventory(i.item);
                        //else
                        //    Game1.player.addItemToInventory(i.item);
                    }


                    SortedDictionary<int, ChestDef> bestGuessChests = ChestManager.parseAllChests();
                    string message = "";
                    foreach (KeyValuePair<int, ChestDef> chestDefs in bestGuessChests)
                    {
                        message += chestDefs.Key + ": " + chestDefs.Value + "\n";
                    }
                    Log.INFO(message);

                }
                catch (Exception ex)
                {
                    Log.ERROR(ex.ToString());
                }

            }
        }

    }
}
