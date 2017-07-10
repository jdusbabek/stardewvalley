using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrabNet
{
    public class CrabNetConfig : Config
    {

        // The hot key that performs this action.
        public string keybind { get; set; }

        // Whether or not logging is enabled.  If set to true, then debugging log entries will be output to the SMAPI console.
        public bool enableLogging { get; set; }

        // This overrides all cost settings.  If this is set to true, there will be no costs associated with using the command.
        public bool free { get; set; }

        // The cost associated with checking (visiting) a crab pot.  This will be assessed regardless of whether the pot was emptied or baited.
        public int costPerCheck { get; set; }

        // The cost associated with emptying the pot.  This is only assessed if there is something to remove.
        public int costPerEmpty { get; set; }

        // Whether the user wants to be charged for bait.
        public bool chargeForBait { get; set; }

        // The ID of the users preferred bait (regular, or wild)
        public int preferredBait { get; set; }

        // The name of the person who is performing the checks.  'spouse' and character names wil result in interaction.  Setting it to anything else will display that sting in all messages.
        public string whoChecks { get; set; }

        // Whether to display HUD messages and dialog.  Not to be confused with the logging setting.
        public bool enableMessages { get; set; }

        // The X, Y coordinates of a chest, into which surplus items can be deposited.  The farmers inventory will be tried first.
        public Microsoft.Xna.Framework.Vector2 chestCoords { get; set; }

        // Whether to bypass the user's inventory and try depositing to the chest first.  Will fall back to the inventory if no chest is present.
        public bool bypassInventory { get; set; }

        // Whether the mod will be lenient about not having enough cash to complete the transaction.  If set to false, the worker will not do work the farmer cannot afford.
        public bool allowFreebies { get; set; }



        public override T GenerateDefaultConfig<T>()
        {
            keybind = "H";
            enableLogging = false;
            free = false;
            costPerCheck = 1;
            costPerEmpty = 10;
            chargeForBait = true;
            preferredBait = 685;
            whoChecks = "spouse";
            enableMessages = true;
            chestCoords = new Microsoft.Xna.Framework.Vector2(73f, 14f);
            bypassInventory = false;
            allowFreebies = true;
            
            return this as T;
        }

    }
}
