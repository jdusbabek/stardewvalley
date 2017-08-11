using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using SFarmer = StardewValley.Farmer;

namespace CrabNet
{
    public class CrabNet : Mod
    {
        /*********
        ** Properties
        *********/
        // Local variable for config setting "keybind"
        private Keys actionKey;

        // Local variable for config setting "loggingEnabled"
        private bool loggingEnabled;

        // Local variable for config setting "preferredBait"
        private int baitChoice = 685;

        // Local variable for config setting "free"
        private bool free;

        // Local variable for config setting "chargeForBait"
        private bool chargeForBait = true;

        // Local variable for config setting "costPerCheck"
        private int costPerCheck = 1;

        // Local variable for config setting "costPerEmpty"
        private int costPerEmpty = 10;

        // Local variable for config setting "whoChecks"
        private string checker = "CrabNet";

        // Local variable for config setting "enableMessages"
        private bool enableMessages = true;

        // Local variable for config setting "chestCoords"
        private Vector2 chestCoords = new Vector2(73f, 14f);

        // Local variable for config setting "bypassInventory"
        private bool bypassInventory;

        // Local variable for config setting "allowFreebies"
        private bool allowFreebies = true;

        // The cost per 1 bait, as determined from the user's bait preference
        private int baitCost;

        // Content manager for loading dialogs, etc.
        private LocalizedContentManager content;

        // An indexed list of all messages from the dialog.xna file
        private Dictionary<string, string> allmessages;

        // An indexed list of key dialog elements, these need to be indexed in the order in the file ie. cannot be randomized.
        private Dictionary<int, string> dialog;

        // An indexed list of greetings.
        private Dictionary<int, string> greetings;

        // An indexed list of all dialog entries relating to "unfinished"
        private Dictionary<int, string> unfinishedMessages;

        // An indexed list of all dialog entries related to "freebies"
        private Dictionary<int, string> freebieMessages;

        // An indexed list of all dialog entries related to "inventory full"
        private Dictionary<int, string> inventoryMessages;

        // An indexed list of all dialog entries related to "smalltalk".  This list is merged with a list of dialogs that are specific to your "checker"
        private Dictionary<int, string> smalltalk;

        // Random number generator, used primarily for selecting dialog messages.
        private Random random = new Random();

        // A flag for when an item could not be deposited into either the inventory or the chest.
        private bool inventoryAndChestFull;

        // The configuration object.  Not used per-se, only to populate the local variables.
        private CrabNetConfig Config;

        private DialogManager DialogueManager;


        /*********
        ** Public methods
        *********/
        /**
         * The SMAPI entry point.
         */
        public override void Entry(params object[] objects)
        {
            this.Config = this.Helper.ReadConfig<CrabNetConfig>();
            this.DialogueManager = new DialogManager(this.Config);

            PlayerEvents.LoadedGame += onLoaded;
            ControlEvents.KeyReleased += onKeyReleased;
        }


        /*********
        ** Private methods
        *********/
        private void onLoaded(object sender, EventArgs e)
        {
            if (!Enum.TryParse(this.Config.keybind, true, out this.actionKey))
            {
                this.actionKey = Keys.H;
                Log.Info("[CrabNet] Error parsing key binding. Defaulted to H");
            }

            loggingEnabled = this.Config.enableLogging;

            // 685, or 774
            if (this.Config.preferredBait == 685 || this.Config.preferredBait == 774)
                baitChoice = this.Config.preferredBait;
            else
                baitChoice = 685;

            baitCost = new StardewValley.Object(baitChoice, 1).Price;
            chargeForBait = this.Config.chargeForBait;
            costPerCheck = Math.Max(0, this.Config.costPerCheck);
            costPerEmpty = Math.Max(0, this.Config.costPerEmpty);
            free = this.Config.free;
            checker = this.Config.whoChecks;
            enableMessages = this.Config.enableMessages;
            chestCoords = this.Config.chestCoords;
            bypassInventory = this.Config.bypassInventory;
            allowFreebies = this.Config.allowFreebies;

            this.content = new LocalizedContentManager(Game1.content.ServiceProvider, this.PathOnDisk);
            readInMessages();
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

            if (e.KeyPressed == this.actionKey)
            {
                try
                {
                    iterateOverCrabPots();
                }
                catch (Exception ex)
                {
                    if (loggingEnabled)
                    {
                        Log.Info("[CrabNet] Exception onKeyReleased: " + ex.Message);
                        Log.Error("[CrabNet] Stacktrace: " + ex.ToString());
                    }
                }

            }
        }

        private void iterateOverCrabPots()
        {
            // reset this each time invoked, it is a flag to determine if uncompleted work is due to inventory or money.
            inventoryAndChestFull = false;
            CrabNetStats stats = new CrabNetStats();

            bool doesPlayerHaveEnoughCash = true;

            foreach (GameLocation location in Game1.locations)
            {
                if (location.isOutdoors)
                {
                    foreach (StardewValley.Object obj in location.Objects.Values)
                    {
                        if (obj.Name == "Crab Pot")
                        {
                            stats.numTotal++;

                            if (!free && !canAfford(Game1.player, this.costPerCheck, stats) && !allowFreebies)
                            {
                                Log.Info("[CrabNet] Couldn't afford to check.");
                                stats.notChecked++;
                                continue;
                            }

                            stats.numChecked++;
                            stats.runningTotal += costPerCheck;

                            CrabPot pot = (CrabPot)obj;

                            if (pot.heldObject != null && pot.heldObject.category != -21)
                            {
                                if (!free && !canAfford(Game1.player, this.costPerEmpty, stats) && !allowFreebies)
                                {
                                    Log.Info("[CrabNet] Couldn't afford to empty.");
                                    stats.notEmptied++;
                                    continue;
                                }

                                if (checkForAction(Game1.player, pot, stats))
                                {
                                    stats.numEmptied++;
                                    stats.runningTotal += costPerEmpty;
                                }
                            }
                            else
                            {
                                stats.nothingToRetrieve++;
                            }

                            if (pot.bait == null && pot.heldObject == null && !Game1.player.professions.Contains(11))
                            {
                                StardewValley.Object b = new StardewValley.Object(this.baitChoice, 1, false, -1, 0);

                                if (!free && !canAfford(Game1.player, this.baitCost, stats) && !allowFreebies && chargeForBait)
                                {
                                    Log.Info("[CrabNet] Couldn't afford to bait.");
                                    stats.notBaited++;
                                    continue;
                                }

                                if (performObjectDropInAction(b, Game1.player, pot))
                                {
                                    stats.numBaited++;
                                    if (chargeForBait)
                                        stats.runningTotal += baitCost;
                                }
                            }
                            else
                            {
                                stats.nothingToBait++;
                            }

                        }
                    }
                }
            }

            int totalCost = (stats.numChecked * costPerCheck);
            totalCost += (stats.numEmptied * costPerEmpty);
            if (this.chargeForBait)
            {
                totalCost += (baitCost * stats.numBaited);
            }
            if (free)
            {
                totalCost = 0;
            }

            if (loggingEnabled)
            {
                Log.Info("[CrabNet] CrabNet checked " + stats.numChecked + " pots.");
                Log.Info("[CrabNet] You used " + stats.numBaited + " bait to reset.");

                if (!free)
                    Log.Info("[CrabNet] Total cost was " + totalCost + "g. Checks: " + (stats.numChecked * this.costPerCheck) + ", Emptied: " + (stats.numEmptied * this.costPerEmpty) + ", Bait: " + (stats.numBaited * this.baitCost));
            }

            doesPlayerHaveEnoughCash = (Game1.player.Money >= totalCost);

            if (!free)
                Game1.player.Money = Math.Max(0, Game1.player.Money + (-1 * totalCost));

            if (enableMessages)
            {
                showMessage(stats, totalCost, doesPlayerHaveEnoughCash);
            }
        }



        private bool checkForAction(SFarmer farmer, CrabPot pot, CrabNetStats stats)
        {
            if (!canAfford(farmer, this.costPerCheck, stats))
                return false;

            if (pot.tileIndexToShow == 714)
            {
                if (farmer.IsMainPlayer && !addItemToInventory(pot.heldObject, farmer, Game1.getFarm()))
                {
                    Game1.addHUDMessage(new HUDMessage("Inventory Full", Color.Red, 3500f));
                    return false;
                }
                Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
                if (dictionary.ContainsKey(pot.heldObject.parentSheetIndex))
                {
                    string[] strArray = dictionary[pot.heldObject.parentSheetIndex].Split('/');
                    int minValue = strArray.Length > 5 ? Convert.ToInt32(strArray[5]) : 1;
                    int num = strArray.Length > 5 ? Convert.ToInt32(strArray[6]) : 10;
                    farmer.caughtFish(pot.heldObject.parentSheetIndex, Game1.random.Next(minValue, num + 1));
                }
                pot.readyForHarvest = false;
                pot.heldObject = null;
                pot.tileIndexToShow = 710;
                pot.bait = null;
                farmer.gainExperience(1, 5);

                return true;
            }
            return false;
        }


        private bool performObjectDropInAction(StardewValley.Object dropIn, SFarmer farmer, CrabPot pot)
        {
            if (pot.bait != null || farmer.professions.Contains(11))
                return false;

            pot.bait = dropIn;

            return true;
        }


        private bool addItemToInventory(StardewValley.Object obj, SFarmer farmer, Farm farm)
        {
            bool wasAdded = false;

            if (farmer.couldInventoryAcceptThisItem(obj) && !bypassInventory)
            {
                farmer.addItemToInventory(obj);
                wasAdded = true;

                if (loggingEnabled)
                    Log.Info("[CrabNet] Was able to add item to inventory.");
            }
            else
            {
                StardewValley.Object chest = null;
                farm.objects.TryGetValue(chestCoords, out chest);

                if (chest != null && chest is Chest)
                {
                    if (loggingEnabled)
                        Log.Info("[CrabNet] Found a chest at " + (int)this.chestCoords.X + "," + (int)this.chestCoords.Y);

                    Item i = ((Chest)chest).addItem(obj);
                    if (i == null)
                    {
                        wasAdded = true;

                        if (loggingEnabled)
                            Log.Info("[CrabNet] Was able to add items to chest.");
                    }
                    else
                    {
                        inventoryAndChestFull = true;

                        if (loggingEnabled)
                            Log.Info("[CrabNet] Was NOT able to add items to chest.");
                    }

                }
                else
                {
                    if (loggingEnabled)
                        Log.Info("[Animal-Sitter] Did not find a chest at " + (int)this.chestCoords.X + "," + (int)this.chestCoords.Y);

                    // If bypassInventory is set to true, but there's no chest: try adding to the farmer's inventory.
                    if (bypassInventory)
                    {
                        if (loggingEnabled)
                            Log.Info("[Animal-Sitter] No chest at " + (int)this.chestCoords.X + "," + (int)this.chestCoords.Y + ", you should place a chest there, or set bypassInventory to 'false'.");

                        if (farmer.couldInventoryAcceptThisItem(obj))
                        {
                            farmer.addItemToInventory(obj);
                            wasAdded = true;

                            if (loggingEnabled)
                                Log.Info("[CrabNet] Was able to add item to inventory. (No chest found, bypassInventory set to 'true')");
                        }
                        else
                        {
                            inventoryAndChestFull = true;

                            if (loggingEnabled)
                                Log.Info("[CrabNet] Was NOT able to add item to inventory or a chest.  (No chest found, bypassInventory set to 'true')");
                        }
                    }
                    else
                    {
                        inventoryAndChestFull = true;

                        if (loggingEnabled)
                            Log.Info("[CrabNet] Was NOT able to add item to inventory or a chest.  (No chest found, bypassInventory set to 'false')");
                    }
                }
            }

            return wasAdded;
        }


        private String getGathererName()
        {
            if (checker.ToLower() == "spouse")
            {
                if (Game1.player.isMarried())
                    return Game1.player.getSpouse().getName();
                else
                    return "The crab pot checker";
            }
            else
            {
                return checker;
            }

        }


        private bool canAfford(SFarmer farmer, int amount, CrabNetStats stats)
        {
            // Calculate the running cost (need config passed for that) and determine if additional puts you over.
            return (amount + stats.runningTotal) <= farmer.Money;
        }


        private void showMessage(CrabNetStats stats, int totalCost, bool doesPlayerHaveEnoughCash)
        {
            string message = "";

            if (checker.ToLower() == "spouse")
            {
                if (Game1.player.isMarried())
                {
                    message += this.DialogueManager.PerformReplacement(dialog[1], stats);
                    //message += Game1.player.getSpouse().getName() + " has emptied and baited " + stats.numChecked + " crab pots.";
                }
                else
                {
                    message += this.DialogueManager.PerformReplacement(dialog[2], stats);
                    //message += "Your fishing assistant emptied and baited " + stats.numChecked + " crab pots. ";
                }

                if (totalCost > 0 && !free)
                {
                    message += this.DialogueManager.PerformReplacement(dialog[3], stats);
                    //message += "Cost was " + totalCost + "g.";
                }

                HUDMessage msg = new HUDMessage(message);
                Game1.addHUDMessage(msg);
            }
            else
            {
                NPC character = Game1.getCharacterFromName(checker);
                if (character != null)
                {
                    //this.isCheckerCharacter = true;

                    message += this.DialogueManager.PerformReplacement(getRandomMessage(greetings), stats);
                    message += " " + this.DialogueManager.PerformReplacement(dialog[4], stats);
                    //message += "Hi @. I serviced " + stats.numChecked + " crab pots.";

                    if (!free)
                    {
                        //message += " The charge is " + totalCost + "g.";
                        this.DialogueManager.PerformReplacement(dialog[5], stats);

                        if (stats.hasUnfinishedBusiness())
                        {
                            if (inventoryAndChestFull)
                            {
                                message += this.DialogueManager.PerformReplacement(getRandomMessage(inventoryMessages), stats);
                            }
                            else
                            {
                                if (allowFreebies)
                                {
                                    message += this.DialogueManager.PerformReplacement(getRandomMessage(freebieMessages), stats);
                                }
                                else
                                {
                                    message += " " + this.DialogueManager.PerformReplacement(getRandomMessage(unfinishedMessages), stats);
                                }
                            }
                        }

                        //if (allowFreebies && stats.hasUnfinishedBusiness() )
                        //{
                        //    message += getRandomMessage(freebieMessages);
                        //}
                        //else if (!allowFreebies && stats.hasUnfinishedBusiness())
                        //{
                        //    message += " " + getRandomMessage(unfinishedMessages);
                        //}

                        message += this.DialogueManager.PerformReplacement(getRandomMessage(smalltalk), stats);
                        message += "#$e#";
                    }
                    else
                    {
                        message += this.DialogueManager.PerformReplacement(getRandomMessage(smalltalk), stats);
                        message += "#$e#";
                    }

                    character.CurrentDialogue.Push(new Dialogue(message, character));
                    Game1.drawDialogue(character);
                }
                else
                {
                    message += this.DialogueManager.PerformReplacement(dialog[6], stats);
                    HUDMessage msg = new HUDMessage(message);
                    Game1.addHUDMessage(msg);
                }
            }

        }


        //public string getRandomUnfinishedMessage()
        //{
        //    int rand = random.Next(1, unfinishedMessages.Count + 1);

        //    string value = "You're broke, so I'm done for the day.$h#$e#";

        //    unfinishedMessages.TryGetValue(rand, out value);

        //    Log.Info((object)("[CrabNet] condition met to return random unfinished message, returning:" + value));

        //    return value;
        //}

        private string getRandomMessage(Dictionary<int, string> messageStore)
        {
            int rand = random.Next(1, messageStore.Count + 1);

            string value = "...$h#$e#";

            messageStore.TryGetValue(rand, out value);

            if (loggingEnabled)
                Log.Info("[CrabNet] condition met to return random unfinished message, returning:" + value);

            return value;
        }


        private void readInMessages()
        {
            //Dictionary<int, string> objects = Game1.content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
            try
            {
                allmessages = content.Load<Dictionary<string, string>>("dialog");

                dialog = this.DialogueManager.GetDialog("Xdialog", allmessages);
                greetings = this.DialogueManager.GetDialog("greeting", allmessages);
                unfinishedMessages = this.DialogueManager.GetDialog("unfinishedmoney", allmessages);
                freebieMessages = this.DialogueManager.GetDialog("freebies", allmessages);
                inventoryMessages = this.DialogueManager.GetDialog("unfinishedinventory", allmessages);
                smalltalk = this.DialogueManager.GetDialog("smalltalk", allmessages);

                Dictionary<int, string> characterDialog = this.DialogueManager.GetDialog(this.checker, allmessages);

                if (characterDialog.Count > 0)
                {
                    int index = smalltalk.Count + 1;
                    foreach (KeyValuePair<int, string> d in characterDialog)
                    {
                        smalltalk.Add(index, d.Value);
                        index++;
                    }
                }

                //foreach (KeyValuePair<int, string> msg in (Dictionary<int, string>)unfinishedMessages)
                //{
                //    Log.Info((object)("[CrabNet] unfinished:" + msg.Key + ": " + msg.Value));
                //}
                //foreach (KeyValuePair<int, string> msg in (Dictionary<int, string>)smalltalk)
                //{
                //    Log.Info((object)("[CrabNet] smalltalk:" + msg.Key + ": " + msg.Value));
                //}
            }
            catch (Exception ex)
            {
                Log.Error("[CrabNet] Exception loading content:" + ex.ToString());
            }
        }
    }
}
