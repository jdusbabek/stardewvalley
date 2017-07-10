using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework.Input;
using StardewValley.TerrainFeatures;
using Object = StardewValley.Object;
using Microsoft.Xna.Framework;
using StardewLib;
using Log = StardewLib.Log;
using xTile.ObjectModel;
using StardewValley.Objects;
using ChestManager = SortMyStuff.chest.ChestManager;
using ChestDef = SortMyStuff.chest.ChestDef;


namespace SortMyStuff
{
    public class SortMyStuff : Mod
    {
        public static SortMyStuffConfig config;
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
                SortMyStuff.config = (SortMyStuffConfig)ConfigExtensions.InitializeConfig<SortMyStuffConfig>(new SortMyStuffConfig(), this.BaseConfigPath);

                if (!Enum.TryParse<Keys>(config.keybind, true, out SortMyStuff.actionKey))
                {
                    SortMyStuff.actionKey = Keys.G;
                    Log.force_INFO((object)"[SMS] Error parsing key binding. Defaulted to G");
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
                || (((Farmer)Game1.player).UsingTool
                || !((Farmer)Game1.player).CanMove
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
                        message += chestDefs.Key + ": " + chestDefs.Value.ToString() + "\n";
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
