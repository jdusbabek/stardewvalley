using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewLib;
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
        private Keys ActionKey;

        // Local variable for config setting "loggingEnabled"
        private bool LoggingEnabled;

        // Local variable for config setting "preferredBait"
        private int BaitChoice = 685;

        // Local variable for config setting "free"
        private bool Free;

        // Local variable for config setting "chargeForBait"
        private bool ChargeForBait = true;

        // Local variable for config setting "costPerCheck"
        private int CostPerCheck = 1;

        // Local variable for config setting "costPerEmpty"
        private int CostPerEmpty = 10;

        // Local variable for config setting "whoChecks"
        private string Checker = "CrabNet";

        // Local variable for config setting "enableMessages"
        private bool EnableMessages = true;

        // Local variable for config setting "chestCoords"
        private Vector2 ChestCoords = new Vector2(73f, 14f);

        // Local variable for config setting "bypassInventory"
        private bool BypassInventory;

        // Local variable for config setting "allowFreebies"
        private bool AllowFreebies = true;

        // The cost per 1 bait, as determined from the user's bait preference
        private int BaitCost;

        // Content manager for loading dialogs, etc.
        private LocalizedContentManager Content;

        // An indexed list of all messages from the dialog.xna file
        private Dictionary<string, string> AllMessages;

        // An indexed list of key dialog elements, these need to be indexed in the order in the file ie. cannot be randomized.
        private Dictionary<int, string> Dialog;

        // An indexed list of greetings.
        private Dictionary<int, string> Greetings;

        // An indexed list of all dialog entries relating to "unfinished"
        private Dictionary<int, string> UnfinishedMessages;

        // An indexed list of all dialog entries related to "freebies"
        private Dictionary<int, string> FreebieMessages;

        // An indexed list of all dialog entries related to "inventory full"
        private Dictionary<int, string> InventoryMessages;

        // An indexed list of all dialog entries related to "smalltalk".  This list is merged with a list of dialogs that are specific to your "checker"
        private Dictionary<int, string> Smalltalk;

        // Random number generator, used primarily for selecting dialog messages.
        private Random Random = new Random();

        // A flag for when an item could not be deposited into either the inventory or the chest.
        private bool InventoryAndChestFull;

        // The configuration object.  Not used per-se, only to populate the local variables.
        private CrabNetConfig Config;

        private DialogueManager DialogueManager;


        /*********
        ** Public methods
        *********/
        /**
         * The SMAPI entry point.
         */
        public override void Entry(params object[] objects)
        {
            this.Config = this.Helper.ReadConfig<CrabNetConfig>();
            this.DialogueManager = new DialogueManager(this.Config, Game1.content.ServiceProvider, Game1.content.RootDirectory, this.Monitor);

            PlayerEvents.LoadedGame += this.PlayerEvents_LoadedGame;
            ControlEvents.KeyReleased += this.ControlEvents_KeyReleased;
        }


        /*********
        ** Private methods
        *********/
        private void PlayerEvents_LoadedGame(object sender, EventArgs e)
        {
            if (!Enum.TryParse(this.Config.KeyBind, true, out this.ActionKey))
            {
                this.ActionKey = Keys.H;
                this.Monitor.Log("Error parsing key binding. Defaulted to H");
            }

            this.LoggingEnabled = this.Config.EnableLogging;

            // 685, or 774
            if (this.Config.PreferredBait == 685 || this.Config.PreferredBait == 774)
                this.BaitChoice = this.Config.PreferredBait;
            else
                this.BaitChoice = 685;

            this.BaitCost = new StardewValley.Object(this.BaitChoice, 1).Price;
            this.ChargeForBait = this.Config.ChargeForBait;
            this.CostPerCheck = Math.Max(0, this.Config.CostPerCheck);
            this.CostPerEmpty = Math.Max(0, this.Config.CostPerEmpty);
            this.Free = this.Config.Free;
            this.Checker = this.Config.WhoChecks;
            this.EnableMessages = this.Config.EnableMessages;
            this.ChestCoords = this.Config.ChestCoords;
            this.BypassInventory = this.Config.BypassInventory;
            this.AllowFreebies = this.Config.AllowFreebies;

            this.Content = new LocalizedContentManager(Game1.content.ServiceProvider, this.PathOnDisk);
            this.ReadInMessages();
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
            {

                return;
            }

            if (e.KeyPressed == this.ActionKey)
            {
                try
                {
                    this.IterateOverCrabPots();
                }
                catch (Exception ex)
                {
                    this.Monitor.Log($"Exception onKeyReleased: {ex}", LogLevel.Error);
                }

            }
        }

        private void IterateOverCrabPots()
        {
            // reset this each time invoked, it is a flag to determine if uncompleted work is due to inventory or money.
            this.InventoryAndChestFull = false;
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
                            stats.NumTotal++;

                            if (!this.Free && !this.CanAfford(Game1.player, this.CostPerCheck, stats) && !this.AllowFreebies)
                            {
                                this.Monitor.Log("Couldn't afford to check.", LogLevel.Trace);
                                stats.NotChecked++;
                                continue;
                            }

                            stats.NumChecked++;
                            stats.RunningTotal += this.CostPerCheck;

                            CrabPot pot = (CrabPot)obj;

                            if (pot.heldObject != null && pot.heldObject.category != -21)
                            {
                                if (!this.Free && !this.CanAfford(Game1.player, this.CostPerEmpty, stats) && !this.AllowFreebies)
                                {
                                    this.Monitor.Log("Couldn't afford to empty.", LogLevel.Trace);
                                    stats.NotEmptied++;
                                    continue;
                                }

                                if (this.CheckForAction(Game1.player, pot, stats))
                                {
                                    stats.NumEmptied++;
                                    stats.RunningTotal += this.CostPerEmpty;
                                }
                            }
                            else
                            {
                                stats.NothingToRetrieve++;
                            }

                            if (pot.bait == null && pot.heldObject == null && !Game1.player.professions.Contains(11))
                            {
                                StardewValley.Object b = new StardewValley.Object(this.BaitChoice, 1, false, -1, 0);

                                if (!this.Free && !this.CanAfford(Game1.player, this.BaitCost, stats) && !this.AllowFreebies && this.ChargeForBait)
                                {
                                    this.Monitor.Log("Couldn't afford to bait.", LogLevel.Trace);
                                    stats.NotBaited++;
                                    continue;
                                }

                                if (this.PerformObjectDropInAction(b, Game1.player, pot))
                                {
                                    stats.NumBaited++;
                                    if (this.ChargeForBait)
                                        stats.RunningTotal += this.BaitCost;
                                }
                            }
                            else
                            {
                                stats.NothingToBait++;
                            }

                        }
                    }
                }
            }

            int totalCost = (stats.NumChecked * this.CostPerCheck);
            totalCost += (stats.NumEmptied * this.CostPerEmpty);
            if (this.ChargeForBait)
            {
                totalCost += (this.BaitCost * stats.NumBaited);
            }
            if (this.Free)
            {
                totalCost = 0;
            }

            if (this.LoggingEnabled)
            {
                this.Monitor.Log($"CrabNet checked {stats.NumChecked} pots. You used {stats.NumBaited} bait to reset.", LogLevel.Trace);
                if (!this.Free)
                    this.Monitor.Log($"Total cost was {totalCost}g. Checks: {stats.NumChecked * this.CostPerCheck}, Emptied: {stats.NumEmptied * this.CostPerEmpty}, Bait: {stats.NumBaited * this.BaitCost}", LogLevel.Trace);
            }

            doesPlayerHaveEnoughCash = (Game1.player.Money >= totalCost);

            if (!this.Free)
                Game1.player.Money = Math.Max(0, Game1.player.Money + (-1 * totalCost));

            if (this.EnableMessages)
            {
                this.ShowMessage(stats, totalCost, doesPlayerHaveEnoughCash);
            }
        }

        private bool CheckForAction(SFarmer farmer, CrabPot pot, CrabNetStats stats)
        {
            if (!this.CanAfford(farmer, this.CostPerCheck, stats))
                return false;

            if (pot.tileIndexToShow == 714)
            {
                if (farmer.IsMainPlayer && !this.AddItemToInventory(pot.heldObject, farmer, Game1.getFarm()))
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

        private bool PerformObjectDropInAction(StardewValley.Object dropIn, SFarmer farmer, CrabPot pot)
        {
            if (pot.bait != null || farmer.professions.Contains(11))
                return false;

            pot.bait = dropIn;

            return true;
        }

        private bool AddItemToInventory(StardewValley.Object obj, SFarmer farmer, Farm farm)
        {
            bool wasAdded = false;

            if (farmer.couldInventoryAcceptThisItem(obj) && !this.BypassInventory)
            {
                farmer.addItemToInventory(obj);
                wasAdded = true;

                if (this.LoggingEnabled)
                    this.Monitor.Log("Was able to add item to inventory.", LogLevel.Trace);
            }
            else
            {
                StardewValley.Object chest = null;
                farm.objects.TryGetValue(this.ChestCoords, out chest);

                if (chest != null && chest is Chest)
                {
                    if (this.LoggingEnabled)
                        this.Monitor.Log($"Found a chest at {(int)this.ChestCoords.X},{(int)this.ChestCoords.Y}", LogLevel.Trace);

                    Item i = ((Chest)chest).addItem(obj);
                    if (i == null)
                    {
                        wasAdded = true;

                        if (this.LoggingEnabled)
                            this.Monitor.Log("Was able to add items to chest.", LogLevel.Trace);
                    }
                    else
                    {
                        this.InventoryAndChestFull = true;

                        if (this.LoggingEnabled)
                            this.Monitor.Log("Was NOT able to add items to chest.", LogLevel.Trace);
                    }

                }
                else
                {
                    if (this.LoggingEnabled)
                        this.Monitor.Log($"Did not find a chest at {(int)this.ChestCoords.X},{(int)this.ChestCoords.Y}", LogLevel.Trace);

                    // If bypassInventory is set to true, but there's no chest: try adding to the farmer's inventory.
                    if (this.BypassInventory)
                    {
                        if (this.LoggingEnabled)
                            this.Monitor.Log($"No chest at {(int)this.ChestCoords.X},{(int)this.ChestCoords.Y}, you should place a chest there, or set bypassInventory to \'false\'.", LogLevel.Trace);

                        if (farmer.couldInventoryAcceptThisItem(obj))
                        {
                            farmer.addItemToInventory(obj);
                            wasAdded = true;

                            if (this.LoggingEnabled)
                                this.Monitor.Log("Was able to add item to inventory. (No chest found, bypassInventory set to 'true')", LogLevel.Trace);
                        }
                        else
                        {
                            this.InventoryAndChestFull = true;

                            if (this.LoggingEnabled)
                                this.Monitor.Log("Was NOT able to add item to inventory or a chest.  (No chest found, bypassInventory set to 'true')", LogLevel.Trace);
                        }
                    }
                    else
                    {
                        this.InventoryAndChestFull = true;

                        if (this.LoggingEnabled)
                            this.Monitor.Log("Was NOT able to add item to inventory or a chest.  (No chest found, bypassInventory set to 'false')", LogLevel.Trace);
                    }
                }
            }

            return wasAdded;
        }

        private String GetGathererName()
        {
            if (this.Checker.ToLower() == "spouse")
            {
                if (Game1.player.isMarried())
                    return Game1.player.getSpouse().getName();
                else
                    return "The crab pot checker";
            }
            else
            {
                return this.Checker;
            }
        }

        private bool CanAfford(SFarmer farmer, int amount, CrabNetStats stats)
        {
            // Calculate the running cost (need config passed for that) and determine if additional puts you over.
            return (amount + stats.RunningTotal) <= farmer.Money;
        }


        private void ShowMessage(CrabNetStats stats, int totalCost, bool doesPlayerHaveEnoughCash)
        {
            string message = "";

            if (this.Checker.ToLower() == "spouse")
            {
                if (Game1.player.isMarried())
                {
                    message += this.DialogueManager.PerformReplacement(this.Dialog[1], stats, this.Config);
                    //message += Game1.player.getSpouse().getName() + " has emptied and baited " + stats.numChecked + " crab pots.";
                }
                else
                {
                    message += this.DialogueManager.PerformReplacement(this.Dialog[2], stats, this.Config);
                    //message += "Your fishing assistant emptied and baited " + stats.numChecked + " crab pots. ";
                }

                if (totalCost > 0 && !this.Free)
                {
                    message += this.DialogueManager.PerformReplacement(this.Dialog[3], stats, this.Config);
                    //message += "Cost was " + totalCost + "g.";
                }

                HUDMessage msg = new HUDMessage(message);
                Game1.addHUDMessage(msg);
            }
            else
            {
                NPC character = Game1.getCharacterFromName(this.Checker);
                if (character != null)
                {
                    //this.isCheckerCharacter = true;

                    message += this.DialogueManager.PerformReplacement(this.GetRandomMessage(this.Greetings), stats, this.Config);
                    message += " " + this.DialogueManager.PerformReplacement(this.Dialog[4], stats, this.Config);
                    //message += "Hi @. I serviced " + stats.numChecked + " crab pots.";

                    if (!this.Free)
                    {
                        //message += " The charge is " + totalCost + "g.";
                        this.DialogueManager.PerformReplacement(this.Dialog[5], stats, this.Config);

                        if (stats.HasUnfinishedBusiness())
                        {
                            if (this.InventoryAndChestFull)
                            {
                                message += this.DialogueManager.PerformReplacement(this.GetRandomMessage(this.InventoryMessages), stats, this.Config);
                            }
                            else
                            {
                                if (this.AllowFreebies)
                                {
                                    message += this.DialogueManager.PerformReplacement(this.GetRandomMessage(this.FreebieMessages), stats, this.Config);
                                }
                                else
                                {
                                    message += " " + this.DialogueManager.PerformReplacement(this.GetRandomMessage(this.UnfinishedMessages), stats, this.Config);
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

                        message += this.DialogueManager.PerformReplacement(this.GetRandomMessage(this.Smalltalk), stats, this.Config);
                        message += "#$e#";
                    }
                    else
                    {
                        message += this.DialogueManager.PerformReplacement(this.GetRandomMessage(this.Smalltalk), stats, this.Config);
                        message += "#$e#";
                    }

                    character.CurrentDialogue.Push(new Dialogue(message, character));
                    Game1.drawDialogue(character);
                }
                else
                {
                    message += this.DialogueManager.PerformReplacement(this.Dialog[6], stats, this.Config);
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

        //    this.Monitor.Log((object)("[CrabNet] condition met to return random unfinished message, returning:" + value));

        //    return value;
        //}

        private string GetRandomMessage(Dictionary<int, string> messageStore)
        {
            int rand = this.Random.Next(1, messageStore.Count + 1);

            string value = "...$h#$e#";

            messageStore.TryGetValue(rand, out value);

            if (this.LoggingEnabled)
                this.Monitor.Log($"condition met to return random unfinished message, returning:{value}", LogLevel.Trace);

            return value;
        }


        private void ReadInMessages()
        {
            //Dictionary<int, string> objects = Game1.content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
            try
            {
                this.AllMessages = this.Content.Load<Dictionary<string, string>>("dialog");

                this.Dialog = this.DialogueManager.GetDialog("Xdialog", this.AllMessages);
                this.Greetings = this.DialogueManager.GetDialog("greeting", this.AllMessages);
                this.UnfinishedMessages = this.DialogueManager.GetDialog("unfinishedmoney", this.AllMessages);
                this.FreebieMessages = this.DialogueManager.GetDialog("freebies", this.AllMessages);
                this.InventoryMessages = this.DialogueManager.GetDialog("unfinishedinventory", this.AllMessages);
                this.Smalltalk = this.DialogueManager.GetDialog("smalltalk", this.AllMessages);

                Dictionary<int, string> characterDialog = this.DialogueManager.GetDialog(this.Checker, this.AllMessages);

                if (characterDialog.Count > 0)
                {
                    int index = this.Smalltalk.Count + 1;
                    foreach (KeyValuePair<int, string> d in characterDialog)
                    {
                        this.Smalltalk.Add(index, d.Value);
                        index++;
                    }
                }

                //foreach (KeyValuePair<int, string> msg in (Dictionary<int, string>)unfinishedMessages)
                //{
                //   this.Monitor.Log((object)("[CrabNet] unfinished:" + msg.Key + ": " + msg.Value));
                //}
                //foreach (KeyValuePair<int, string> msg in (Dictionary<int, string>)smalltalk)
                //{
                //   this.Monitor.Log((object)("[CrabNet] smalltalk:" + msg.Key + ": " + msg.Value));
                //}
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Exception loading content:{ex}", LogLevel.Error);
            }
        }
    }
}
