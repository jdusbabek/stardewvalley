using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Log = StardewLib.Log;

namespace PelicanFiber
{
    public class PelicanFiber : Mod
    {
        /*********
        ** Properties
        *********/
        private Keys menuKey = Keys.PageDown;
        private LocalizedContentManager content;
        private Texture2D websites;
        private PelicanFiberConfig config;
        private bool unfiltered = true;
        private ItemUtils ItemUtils;
        private readonly Log Log = new Log(false);


        /*********
        ** Public methods
        *********/
        public override void Entry(params object[] objects)
        {
            // load config
            this.config = this.Helper.ReadConfig<PelicanFiberConfig>();
            if (!Enum.TryParse(config.keybind, true, out this.menuKey))
            {
                this.menuKey = Keys.PageDown;
                Log.force_ERROR("[PelicanFiber] 404 Not Found: Error parsing key binding. Defaulted to Page Down");
            }
            this.unfiltered = !config.internetFilter;

            // load textures
            try
            {
                this.content = new LocalizedContentManager(Game1.content.ServiceProvider, this.PathOnDisk);
                this.websites = this.content.Load<Texture2D>("websites");
            }
            catch (Exception ex)
            {
                Log.force_ERROR("[PelicanFiber] 400 Bad Request: Could not load image content. " + ex);
            }

            // load utils
            this.ItemUtils = new ItemUtils(this.content, this.Log);

            // hook events
            ControlEvents.KeyReleased += onKeyReleased;
        }


        /*********
        ** Private methods
        *********/
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
                try
                {
                    float scale = 1.0f;
                    if (Game1.viewport.Height < 1325)
                        scale = Game1.viewport.Height / 1325f;

                    Game1.activeClickableMenu = new PelicanFiberMenu(this.websites, this.ItemUtils, this.config.giveAchievements, this.ShowMainMenu, scale, unfiltered);
                }
                catch (Exception ex)
                {
                    Log.force_ERROR("[PelicanFiber] 500 Internal Error: " + ex);
                }

            }
        }

        private void ShowMainMenu()
        {
            try
            {
                float scale = 1.0f;
                if (Game1.viewport.Height < 1325)
                    scale = Game1.viewport.Height / 1325f;

                Game1.activeClickableMenu = new PelicanFiberMenu(this.websites, this.ItemUtils, this.config.giveAchievements, this.ShowMainMenu, scale, !this.config.internetFilter);
            }
            catch (Exception ex)
            {
                Log.force_ERROR("[PelicanFiber] 500 Internal Error: " + ex);
            }
        }
    }
}
