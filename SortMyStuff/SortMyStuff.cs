using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StardewLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using Object = StardewValley.Object;


namespace SortMyStuff
{
    public class SortMyStuff : Mod
    {
        /*********
        ** Properties
        *********/
        private SortMyStuffConfig Config;
        private Keys ActionKey;
        private ChestManager ChestManager;


        /*********
        ** Public methods
        *********/
        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += this.PlayerEvents_OnLoaded;
            ControlEvents.KeyReleased += this.ControlEvents_KeyReleased;
        }


        /*********
        ** Private methods
        *********/
        private void PlayerEvents_OnLoaded(object sender, EventArgs e)
        {
            try
            {
                this.Config = this.Helper.ReadConfig<SortMyStuffConfig>();
                this.ChestManager = new ChestManager(this.Monitor);

                if (!Enum.TryParse(this.Config.keybind, true, out this.ActionKey))
                {
                    this.ActionKey = Keys.G;
                    this.Monitor.Log("Error parsing key binding. Defaulted to G");
                }

                ChestManager.ParseChests(this.Config.chests);
            }
            catch (Exception ex)
            {
                this.Monitor.Log(ex.ToString(), LogLevel.Error);
            }
        }

        private void ControlEvents_KeyReleased(object sender, EventArgsKeyPressed e)
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

            if (e.KeyPressed == this.ActionKey)
            {
                this.Monitor.Log("Logging key stroke G!!!", LogLevel.Trace);
                List<ItemContainer> ic = new List<ItemContainer>();
                try
                {
                    foreach (Item item in Game1.player.Items)
                    {
                        if (item != null)
                        {
                            this.Monitor.Log($"{item.Name}/{item.parentSheetIndex}", LogLevel.Trace);
                            Object c = ChestManager.GetChest(item.parentSheetIndex);
                            if (c != null && c is Chest)
                            {
                                ic.Add(new ItemContainer((Chest)c, item));
                            }
                            else
                            {
                                c = (Chest)ChestManager.GetChest(item.category);
                                if (c != null && c is Chest)
                                {
                                    ic.Add(new ItemContainer((Chest)c, item));
                                }
                            }
                        }
                    }

                    foreach (ItemContainer i in ic)
                    {
                        Item o = i.Chest.addItem(i.Item);
                        if (o == null)
                            Game1.player.removeItemFromInventory(i.Item);
                        //else
                        //    Game1.player.addItemToInventory(i.item);
                    }


                    SortedDictionary<int, ChestDef> bestGuessChests = ChestManager.ParseAllChests();
                    string message = "";
                    foreach (KeyValuePair<int, ChestDef> chestDefs in bestGuessChests)
                    {
                        message += chestDefs.Key + ": " + chestDefs.Value + "\n";
                    }
                    this.Monitor.Log(message);
                }
                catch (Exception ex)
                {
                    this.Monitor.Log(ex.ToString(), LogLevel.Error);
                }
            }
        }
    }
}
