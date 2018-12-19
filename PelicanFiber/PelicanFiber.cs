using System;
using Microsoft.Xna.Framework.Graphics;
using PelicanFiber.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace PelicanFiber
{
    public class PelicanFiber : Mod
    {
        /*********
        ** Properties
        *********/
        private SButton MenuKey = SButton.PageDown;
        private Texture2D Websites;
        private ModConfig Config;
        private bool Unfiltered = true;
        private ItemUtils ItemUtils;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // load config
            this.Config = this.Helper.ReadConfig<ModConfig>();
            if (!Enum.TryParse(this.Config.KeyBind, true, out this.MenuKey))
            {
                this.MenuKey = SButton.PageDown;
                this.Monitor.Log($"404 Not Found: Error parsing key binding; defaulted to {this.MenuKey}.");
            }
            this.Unfiltered = !this.Config.InternetFilter;

            // load textures
            try
            {
                this.Websites = helper.Content.Load<Texture2D>("assets/websites.png");
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"400 Bad Request: Could not load image content. {ex}", LogLevel.Error);
            }

            // load utils
            this.ItemUtils = new ItemUtils(helper.Content, this.Monitor);

            // hook events
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsPlayerFree)
                return;

            if (e.Button == this.MenuKey)
            {
                try
                {
                    float scale = 1.0f;
                    if (Game1.viewport.Height < 1325)
                        scale = Game1.viewport.Height / 1325f;

                    Game1.activeClickableMenu = new PelicanFiberMenu(this.Websites, this.ItemUtils, this.Config.GiveAchievements, this.Helper.Multiplayer.GetNewID, this.ShowMainMenu, scale, this.Unfiltered);
                }
                catch (Exception ex)
                {
                    this.Monitor.Log($"500 Internal Error: {ex}", LogLevel.Error);
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

                Game1.activeClickableMenu = new PelicanFiberMenu(this.Websites, this.ItemUtils, this.Config.GiveAchievements, this.Helper.Multiplayer.GetNewID, this.ShowMainMenu, scale, !this.Config.InternetFilter);
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"500 Internal Error: {ex}", LogLevel.Error);
            }
        }
    }
}
