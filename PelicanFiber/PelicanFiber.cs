using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewLib;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using Object = StardewValley.Object;
using Log = StardewLib.Log;
using Microsoft.Xna.Framework.Graphics;

namespace PelicanFiber
{
    public class PelicanFiber : Mod
    {
        private static Keys menuKey = Keys.PageDown;
        public static LocalizedContentManager content;
        public static Texture2D websites;
        public static PelicanFiberConfig config;

        public bool unfiltered = true;
        public static bool giveAchievements = false;


        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += onLoaded;
            ControlEvents.KeyReleased += onKeyReleased;
        }

        private void onLoaded(object sender, EventArgs e)
        {
            PelicanFiber.config = (PelicanFiberConfig)ConfigExtensions.InitializeConfig<PelicanFiberConfig>(new PelicanFiberConfig(), this.BaseConfigPath);

            if (!Enum.TryParse<Keys>(config.keybind, true, out PelicanFiber.menuKey))
            {
                PelicanFiber.menuKey = Keys.PageDown;
                Log.force_ERROR((object)"[PelicanFiber] 404 Not Found: Error parsing key binding. Defaulted to Page Down");
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
                Log.force_ERROR("[PelicanFiber] 400 Bad Request: Could not load image content. " + ex.ToString());
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
                        scale = (float)(Game1.viewport.Height) / 1325f;

                    Game1.activeClickableMenu = new PelicanFiberMenu(scale, unfiltered);
                }
                catch (Exception ex)
                {
                    Log.force_ERROR("[PelicanFiber] 500 Internal Error: " + ex.ToString());
                }
                
            }
        }

        public static void showTheMenu()
        {
            try
            {
                float scale = 1.0f;
                if (Game1.viewport.Height < 1325)
                    scale = (float)(Game1.viewport.Height) / 1325f;

                Game1.activeClickableMenu = new PelicanFiberMenu(scale, !config.internetFilter);
            }
            catch (Exception ex)
            {
                Log.force_ERROR("[PelicanFiber] 500 Internal Error: " + ex.ToString());
            }
        }

        public static MailOrderPigMenu getMailOrderPigMenu()
        {
            return new MailOrderPigMenu(ItemUtils.getPurchaseAnimalStock());
        }

        public static BuyAnimalMenu getBuyAnimalMenu()
        {
            return new BuyAnimalMenu(Utility.getPurchaseAnimalStock());
        }

        public static ConstructionMenu getConstructionMenu(bool magical)
        {
            return new ConstructionMenu(magical);
        }

    }
}
