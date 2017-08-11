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
        private static Keys menuKey = Keys.PageDown;
        internal static LocalizedContentManager content;
        internal static Texture2D websites;
        private static PelicanFiberConfig config;

        private bool unfiltered = true;
        internal static bool giveAchievements;


        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += onLoaded;
            ControlEvents.KeyReleased += onKeyReleased;
        }

        private void onLoaded(object sender, EventArgs e)
        {
            PelicanFiber.config = this.Helper.ReadConfig<PelicanFiberConfig>();

            if (!Enum.TryParse(config.keybind, true, out PelicanFiber.menuKey))
            {
                PelicanFiber.menuKey = Keys.PageDown;
                Log.force_ERROR("[PelicanFiber] 404 Not Found: Error parsing key binding. Defaulted to Page Down");
            }

            this.unfiltered = !config.internetFilter;
            PelicanFiber.giveAchievements = config.giveAchievements;

            try
            {
                PelicanFiber.content = new LocalizedContentManager(Game1.content.ServiceProvider, this.PathOnDisk);
                PelicanFiber.websites = PelicanFiber.content.Load<Texture2D>("websites");
            }
            catch (Exception ex)
            {
                Log.force_ERROR("[PelicanFiber] 400 Bad Request: Could not load image content. " + ex);
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

            if (e.KeyPressed == PelicanFiber.menuKey)
            {
                try
                {
                    float scale = 1.0f;
                    if (Game1.viewport.Height < 1325)
                        scale = Game1.viewport.Height / 1325f;

                    Game1.activeClickableMenu = new PelicanFiberMenu(scale, unfiltered);
                }
                catch (Exception ex)
                {
                    Log.force_ERROR("[PelicanFiber] 500 Internal Error: " + ex);
                }

            }
        }

        internal static void showTheMenu()
        {
            try
            {
                float scale = 1.0f;
                if (Game1.viewport.Height < 1325)
                    scale = Game1.viewport.Height / 1325f;

                Game1.activeClickableMenu = new PelicanFiberMenu(scale, !config.internetFilter);
            }
            catch (Exception ex)
            {
                Log.force_ERROR("[PelicanFiber] 500 Internal Error: " + ex);
            }
        }

        internal static MailOrderPigMenu getMailOrderPigMenu()
        {
            return new MailOrderPigMenu(ItemUtils.getPurchaseAnimalStock());
        }

        internal static BuyAnimalMenu getBuyAnimalMenu()
        {
            return new BuyAnimalMenu(Utility.getPurchaseAnimalStock());
        }

        internal static ConstructionMenu getConstructionMenu(bool magical)
        {
            return new ConstructionMenu(magical);
        }

    }
}
