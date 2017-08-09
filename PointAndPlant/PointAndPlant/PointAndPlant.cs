using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile.Dimensions;
using SFarmer = StardewValley.Farmer;

namespace PointAndPlant
{
    public class PointAndPlant : Mod
    {
        private static Keys plowKey;
        private static Keys plantKey;
        private static Keys growKey;
        private static Keys harvestKey;
        private static Keys grassKey;

        private bool plowEnabled;
        private bool plantEnabled;
        private bool growEnabled;
        private bool harvestEnabled;

        private int plantRadius;
        private int growRadius;
        private int harvestRadius;

        private int fertilizer;

        private int plowWidth;
        private int plowHeight;

        private int mouseX;
        private int mouseY;
        private int tileX;
        private int tileY;

        internal static Vector2 vector;
        private Texture2D buildingTiles;

        private static PAPConfig config;

        private static Hoe papHoe;
        private static PAPSickle papSickle;
        private static Axe phantomAxe;
        private static Pickaxe phantomPick;

        private static int power = -1;
        private static int toolLevel = 4;

        private static bool loggingEnabled;

        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += onLoaded;
            ControlEvents.KeyReleased += onKeyReleased;
            GraphicsEvents.OnPreRenderHudEvent += drawPlowOverlay;
        }


        private void onLoaded(object sender, EventArgs e)
        {
            PointAndPlant.config = new PAPConfig().InitializeConfig(this.BaseConfigPath);

            this.plowEnabled = config.plowEnabled;
            this.plantEnabled = config.plantEnabled;
            this.growEnabled = config.growEnabled;
            this.harvestEnabled = config.harvestEnabled;

            this.plantRadius = config.plantRadius;
            this.growRadius = config.growRadius;
            this.harvestRadius = config.harvestRadius;

            this.plowWidth = config.plowWidth;
            this.plowHeight = config.plowHeight;

            this.fertilizer = config.fertilizer;

            PointAndPlant.loggingEnabled = config.loggingEnabled;

            if (config.plowWidth < 1)
            {
                this.plowWidth = 1;
                if (this.plowEnabled)
                {
                    Log.Info("[PAP] Plow width must be at least 1. Defaulted to 1");
                }
            }
            else
            {
                this.plowWidth = config.plowWidth;
            }

            if (config.plowHeight < 1)
            {
                this.plowHeight = 1;
                if (this.plowEnabled)
                {
                    Log.Info("[PAP] Plow width must be at least 1. Defaulted to 1");
                }
            }
            else
            {
                this.plowHeight = config.plowHeight;
            }


            if (config.plantRadius < 0)
            {
                this.plantRadius = 0;

                if (config.plantEnabled)
                {
                    Log.Info("[PAP] Plant Radius must be 0 or greater.  Defaulted to 0");
                }
            }
            else
            {
                this.plantRadius = config.plantRadius;
            }

            if (config.growRadius < 0)
            {
                this.growRadius = 0;

                if (config.growEnabled)
                {
                    Log.Info("[PAP] Grow Radius must be 0 or greater.  Defaulted to 0");
                }
            }
            else
            {
                this.growRadius = config.growRadius;
            }

            if (config.harvestRadius < 0)
            {
                this.harvestRadius = 0;

                if (config.plantEnabled)
                {
                    Log.Info("[PAP] Harvest Radius must be 0 or greater.  Defaulted to 0");
                }
            }
            else
            {
                this.harvestRadius = config.harvestRadius;
            }




            if (!Enum.TryParse(config.plowKey, true, out PointAndPlant.plowKey))
            {
                PointAndPlant.plowKey = Keys.Z;

                if (this.plowEnabled)
                {
                    Log.Info("[PAP] Error parsing plow key. Defaulted to Z");
                }
            }

            if (!Enum.TryParse(config.plantKey, true, out PointAndPlant.plantKey))
            {
                PointAndPlant.plantKey = Keys.A;

                if (this.plantEnabled)
                {
                    Log.Info("[PAP] Error parsing plant key. Defaulted to A");
                }

            }

            if (!Enum.TryParse(config.growKey, true, out PointAndPlant.growKey))
            {
                PointAndPlant.growKey = Keys.S;

                if (this.growEnabled)
                {
                    Log.Info("[PAP] Error parsing grow key. Defaulted to S");
                }
            }

            if (!Enum.TryParse(config.harvestKey, true, out PointAndPlant.harvestKey))
            {
                PointAndPlant.harvestKey = Keys.D;

                if (this.harvestEnabled)
                {
                    Log.Info("[PAP] Error parsing harvest key. Defaulted to D");
                }

            }

            PointAndPlant.grassKey = Keys.Q;

            PointAndPlant.papSickle = new PAPSickle(47, config.harvestRadius);
            PointAndPlant.papHoe = new Hoe();
            PointAndPlant.papHoe.upgradeLevel = PointAndPlant.toolLevel;
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


            if (Game1.hasLoadedGame)
            {
                if (e.KeyPressed == PointAndPlant.plowKey && config.plowEnabled)
                {
                    try
                    {
                        plow();
                    }
                    catch (Exception ex)
                    {
                        if (PointAndPlant.loggingEnabled)
                        {
                            Log.Info("[Point-and-Plant] Plow (key handler) Exception: " + ex.Message);
                            Log.Error("[Point-and-Plant] Plow (key handler) Stack Trace: " + ex);
                            //Log.Info((object)("[Point-and-Plant] Plow (key handler) Source: " + ex.Source));
                        }
                    }
                }
                else if (e.KeyPressed == PointAndPlant.plantKey && config.plantEnabled)
                {
                    try
                    {
                        plant();
                    }
                    catch (Exception ex)
                    {
                        if (PointAndPlant.loggingEnabled)
                        {
                            Log.Info("[Point-and-Plant] Plant (key handler) Exception: " + ex.Message);
                            Log.Error("[Point-and-Plant] Plant (key handler) Stack Trace: " + ex);
                            //Log.Info((object)("[Point-and-Plant] Plant (key handler) Source: " + ex.Source));
                        }
                    }

                }
                else if (e.KeyPressed == PointAndPlant.growKey && config.growEnabled)
                {
                    try
                    {
                        grow();
                    }
                    catch (Exception ex)
                    {
                        if (PointAndPlant.loggingEnabled)
                        {
                            Log.Info("[Point-and-Plant] Grow (key handler) Exception: " + ex.Message);
                            Log.Error("[Point-and-Plant] Grow (key handler) Stack Trace: " + ex);
                            //Log.Info((object)("[Point-and-Plant] Grow (key handler) Source: " + ex.Source));
                        }
                    }

                }
                else if (e.KeyPressed == PointAndPlant.harvestKey && config.harvestEnabled)
                {
                    try
                    {
                        harvest();
                    }
                    catch (Exception ex)
                    {
                        if (PointAndPlant.loggingEnabled)
                        {
                            Log.Info("[Point-and-Plant] Harvest (key handler) Exception: " + ex.Message);
                            Log.Error("[Point-and-Plant] Harvest (key handler) Stack Trace: " + ex);
                            //Log.Info((object)("[Point-and-Plant] Harvest (key handler) Source: " + ex.Source));
                        }
                    }
                }
                else if (e.KeyPressed == PointAndPlant.grassKey)
                {
                    try
                    {
                        if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon)
                        {
                            if (((MeleeWeapon)Game1.player.CurrentTool).initialParentTileIndex == 39)
                            {
                                Log.Info("You're holding Leah's Whittler! Chop away!!");
                                chopTrees();
                            }
                            else if (((MeleeWeapon)Game1.player.CurrentTool).initialParentTileIndex == 37)
                            {
                                Log.Info("You're holding Harvey's Mallet! Pound away!!");
                                breakRocks();
                            }
                            else
                            {
                                plantGrass();
                            }
                        }
                        else
                        {
                            plantGrass();
                        }

                    }
                    catch (Exception ex)
                    {
                        if (PointAndPlant.loggingEnabled)
                        {
                            Log.Info("[Point-and-Plant] Plant Grass (key handler) Exception: " + ex.Message);
                            Log.Error("[Point-and-Plant] Plant Grass (key handler) Stack Trace: " + ex);
                            //Log.Info((object)("[Point-and-Plant] Harvest (key handler) Source: " + ex.Source));
                        }
                    }
                }
            }

        }


        private void plow()
        {
            //Log.Info((object)("[Point-and-Plant] Plow "));

            SFarmer plr = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            if (plr.currentLocation.name.Equals("Farm") || plr.currentLocation.name.Contains("Greenhouse"))
            {
                for (int x = 0; x < this.plowWidth; x++)
                {
                    for (int y = 0; y < this.plowHeight; y++)
                    {
                        tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                    }
                }
            }
            else
            {
                for (int x = 0; x < 15; x++)
                {
                    for (int y = 0; y < 15; y++)
                    {
                        tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                    }
                }
            }


            GameLocation location = Game1.player.currentLocation;
            Vector2 vector2 = new Vector2(this.tileX, this.tileY);

            foreach (Vector2 index in tiles)
            {
                try
                {
                    //index.Equals(vector2);
                    if (location.terrainFeatures.ContainsKey(index))
                    {
                        if (location.terrainFeatures[index].performToolAction(PointAndPlant.papHoe, 0, index))
                            location.terrainFeatures.Remove(index);
                    }
                    else
                    {
                        if (location.objects.ContainsKey(index) && location.Objects[index].performToolAction(PointAndPlant.papHoe))
                        {
                            if (location.Objects[index].type.Equals("Crafting") && location.Objects[index].fragility != 2)
                                location.debris.Add(new Debris(location.Objects[index].bigCraftable ? -location.Objects[index].ParentSheetIndex : location.Objects[index].ParentSheetIndex, Game1.player.GetToolLocation(), new Vector2(Game1.player.GetBoundingBox().Center.X, Game1.player.GetBoundingBox().Center.Y)));
                            location.Objects[index].performRemoveAction(index, location);
                            location.Objects.Remove(index);
                        }
                        if (location.doesTileHaveProperty((int)index.X, (int)index.Y, "Diggable", "Back") != null)
                        {
                            if (location.Name.Equals("UndergroundMine") && !location.isTileOccupied(index))
                            {
                                if (Game1.mine.mineLevel < 40 || Game1.mine.mineLevel >= 80)
                                {
                                    location.terrainFeatures.Add(index, new HoeDirt());
                                    Game1.playSound("hoeHit");
                                }
                                else if (Game1.mine.mineLevel < 80)
                                {
                                    location.terrainFeatures.Add(index, new HoeDirt());
                                    Game1.playSound("hoeHit");
                                }
                                Game1.removeSquareDebrisFromTile((int)index.X, (int)index.Y);
                                location.checkForBuriedItem((int)index.X, (int)index.Y, false, false);
                                location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(vector2.X * Game1.tileSize, vector2.Y * Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f));
                                if (tiles.Count > 2)
                                    location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(index.X * Game1.tileSize, index.Y * Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(vector2, index) * 30f));
                            }
                            else if (!location.isTileOccupied(index) && location.isTilePassable(new Location((int)index.X, (int)index.Y), Game1.viewport))
                            {
                                location.makeHoeDirt(index);
                                Game1.playSound("hoeHit");
                                Game1.removeSquareDebrisFromTile((int)index.X, (int)index.Y);
                                location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(index.X * Game1.tileSize, index.Y * Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f));
                                if (tiles.Count > 2)
                                    location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(index.X * Game1.tileSize, index.Y * Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(vector2, index) * 30f));
                                location.checkForBuriedItem((int)index.X, (int)index.Y, false, false);
                            }
                            ++Game1.stats.DirtHoed;
                        }

                    }
                }
                catch (Exception exception)
                {
                    if (PointAndPlant.loggingEnabled)
                    {
                        Log.Info("[Point-and-Plant] Plow Exception: " + exception.Message);
                        Log.Error("[Point-and-Plant] Plow Stack Trace: " + exception);
                        //Log.Error((object)("[Point-and-Plant] Plow Source: " + exception.Source));
                    }

                }
            }
        }

        private void plant()
        {
            //Log.Info((object)("[Point-and-Plant] Plant "));

            SFarmer plr = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            int min = this.plantRadius * -1;

            for (int x = min; x <= this.plantRadius; x++)
            {
                for (int y = min; y <= this.plantRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            foreach (Vector2 index in tiles)
            {
                try
                {
                    if (plr.currentLocation.terrainFeatures.ContainsKey(index))
                    {
                        if (plr.currentLocation.terrainFeatures[index] is HoeDirt)
                        {
                            HoeDirt dirt = (HoeDirt)plr.currentLocation.terrainFeatures[index];
                            //dirt.crop?.growCompletely();

                            if (plr.currentLocation.isFarm || plr.currentLocation.name.Contains("Greenhouse"))
                            {
                                if (Game1.player.ActiveObject != null && dirt.crop == null && (Game1.player.ActiveObject.Category == -74 || Game1.player.ActiveObject.Category == -19))
                                {
                                    //Quality Fertilizer: 369
                                    //Quality Retaining Soil: 371
                                    //Basic Fertilizer: 368
                                    //Basic Retaining Soil: 370
                                    //Deluxe Speed-Gro: 466
                                    //Speed - Gro: 465
                                    dirt.fertilizer = this.fertilizer;

                                    if (dirt.plant(Game1.player.ActiveObject.ParentSheetIndex, this.mouseX, this.mouseY, Game1.player, Game1.player.ActiveObject.Category == -19) && Game1.player.IsMainPlayer)
                                        Game1.player.reduceActiveItemByOne();
                                    Game1.haltAfterCheck = false;
                                }

                            }

                        }
                    }
                }
                catch (Exception exception)
                {
                    if (PointAndPlant.loggingEnabled)
                    {
                        Log.Info("[Point-and-Plant] Planting Exception: " + exception.Message);
                        Log.Error("[Point-and-Plant] Planting Stack Trace: " + exception);
                        //Log.Info((object)("[Point-and-Plant] Planting Source: " + exception.Source));
                    }
                }
            }
        }


        private void plantGrass()
        {

            SFarmer plr = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            int min = this.plantRadius * -1;

            for (int x = min; x <= this.plantRadius; x++)
            {
                for (int y = min; y <= this.plantRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            foreach (Vector2 index in tiles)
            {
                try
                {
                    if (plr.currentLocation.objects.ContainsKey(index) || plr.currentLocation.terrainFeatures.ContainsKey(index))
                    {

                    }
                    else
                    {
                        Log.Info("Adding grass to: " + index);
                        plr.currentLocation.terrainFeatures.Add(index, new Grass(1, 4));
                        Game1.playSound("dirtyHit");
                    }

                    //if (!plr.currentLocation.objects.ContainsKey(index) && !plr.currentLocation.terrainFeatures.ContainsKey(index))
                    //{
                    //    plr.currentLocation.terrainFeatures.Add(index, (TerrainFeature)new Grass(1, 0));
                    //    Game1.playSound("dirtyHit");
                    //}
                }
                catch (Exception exception)
                {
                    if (PointAndPlant.loggingEnabled)
                    {
                        Log.Info("[Point-and-Plant] Planting Exception: " + exception.Message);
                        Log.Error("[Point-and-Plant] Planting Stack Trace: " + exception);
                        //Log.Info((object)("[Point-and-Plant] Planting Source: " + exception.Source));
                    }
                }
            }
        }


        private void chopTrees()
        {
            SFarmer plr = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            if (PointAndPlant.phantomAxe == null)
            {
                PointAndPlant.phantomAxe = new Axe();
                PointAndPlant.phantomAxe.upgradeLevel = 4;
            }

            int min = this.plantRadius * -1;

            for (int x = min; x <= this.plantRadius; x++)
            {
                for (int y = min; y <= this.plantRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            foreach (Vector2 index in tiles)
            {
                try
                {
                    if (plr.currentLocation.terrainFeatures.ContainsKey(index) && plr.currentLocation.terrainFeatures[index] is Tree)
                    {
                        PointAndPlant.phantomAxe.DoFunction(plr.currentLocation, (int)(index.X * Game1.tileSize), (int)(index.Y * Game1.tileSize), 4, plr);
                    }

                }
                catch (Exception exception)
                {
                    if (PointAndPlant.loggingEnabled)
                    {
                        Log.Info("[Point-and-Plant] Tree Chopping Exception: " + exception.Message);
                        Log.Error("[Point-and-Plant] Tree Chopping Trace: " + exception);
                        //Log.Info((object)("[Point-and-Plant] Planting Source: " + exception.Source));
                    }
                }
            }
        }


        private void breakRocks()
        {
            SFarmer plr = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            if (PointAndPlant.phantomPick == null)
            {
                PointAndPlant.phantomPick = new Pickaxe();
                PointAndPlant.phantomPick.upgradeLevel = 4;
            }

            int min = this.plantRadius * -1;

            for (int x = min; x <= this.plantRadius; x++)
            {
                for (int y = min; y <= this.plantRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            foreach (Vector2 index in tiles)
            {
                try
                {
                    if (plr.currentLocation.objects.ContainsKey(index))
                    {
                        StardewValley.Object o = Game1.player.currentLocation.Objects[index];
                        if (o != null && o.name.Equals("Stone"))
                        {
                            o.minutesUntilReady = 0;
                            PointAndPlant.phantomPick.DoFunction(plr.currentLocation, (int)(index.X * Game1.tileSize), (int)(index.Y * Game1.tileSize), 4, plr);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (PointAndPlant.loggingEnabled)
                    {
                        Log.Info("[Point-and-Plant] Rock Pounding Exception: " + exception.Message);
                        Log.Error("[Point-and-Plant] Rock Pounding  Trace: " + exception);
                    }
                }
            }
        }

        private void grow()
        {
            //Log.Info((object)("[Point-and-Plant] Grow "));

            SFarmer plr = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            int min = this.growRadius * -1;

            for (int x = min; x <= this.growRadius; x++)
            {
                for (int y = min; y <= this.growRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            foreach (Vector2 index in tiles)
            {
                try
                {
                    if (plr.currentLocation.terrainFeatures.ContainsKey(index))
                    {
                        if (plr.currentLocation.terrainFeatures[index] is HoeDirt)
                        {
                            HoeDirt dirt = (HoeDirt)plr.currentLocation.terrainFeatures[index];
                            dirt.crop?.growCompletely();
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (PointAndPlant.loggingEnabled)
                    {
                        Log.Info("[Point-and-Plant] Grow Exception: " + exception.Message);
                        Log.Error("[Point-and-Plant] Grow Stack Trace: " + exception);
                        //Log.Info((object)("[Point-and-Plant] Grow Source: " + exception.Source));
                    }
                }

            }
        }

        private void harvest()
        {
            //Log.Info((object)("[Point-and-Plant] Harvest "));

            SFarmer plr = Game1.player;
            GameLocation currentLocation = Game1.currentLocation;
            StardewValley.Object @object = null;
            TerrainFeature terrainFeature = null;
            currentLocation.Objects.TryGetValue(PointAndPlant.vector, out @object);
            currentLocation.terrainFeatures.TryGetValue(PointAndPlant.vector, out terrainFeature);

            //List<Vector2> tiles = new List<Vector2>();
            try
            {
                if (plr.currentLocation.isFarm || plr.currentLocation.name.Contains("Greenhouse"))
                {
                    if (((HoeDirt)terrainFeature).crop != null || ((HoeDirt)terrainFeature).fertilizer != 0)
                    {
                        if (((HoeDirt)terrainFeature).crop != null && (((HoeDirt)terrainFeature).crop.harvestMethod == 1 && ((HoeDirt)terrainFeature).readyForHarvest() || ((HoeDirt)terrainFeature).crop.dead))
                        {
                            PointAndPlant.papSickle.DoDamage(currentLocation, this.mouseX, this.mouseY, Game1.player.getFacingDirection(), 0, Game1.player);
                        }
                        else if (((HoeDirt)terrainFeature).crop != null && (((HoeDirt)terrainFeature).crop.harvestMethod != 1 && ((HoeDirt)terrainFeature).readyForHarvest()))
                        {
                            List<Vector2> tiles = new List<Vector2>();
                            int min = this.harvestRadius * -1;

                            for (int x = min; x <= this.harvestRadius; x++)
                            {
                                for (int y = min; y <= this.harvestRadius; y++)
                                {
                                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                                }
                            }

                            foreach (Vector2 index in tiles)
                            {
                                try
                                {
                                    if (plr.currentLocation.terrainFeatures.ContainsKey(index))
                                    {
                                        if (plr.currentLocation.terrainFeatures[index] is HoeDirt)
                                        {
                                            HoeDirt dirt = (HoeDirt)plr.currentLocation.terrainFeatures[index];
                                            if (dirt.crop?.harvestMethod == 0)
                                            {
                                                dirt.performUseAction(index);
                                            }

                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    if (PointAndPlant.loggingEnabled)
                                    {
                                        Log.Info("[Point-and-Plant] Harvest Inner Exception: " + exception.Message);
                                        Log.Error("[Point-and-Plant] Harvest Inner Stack Trace: " + exception);
                                        //Log.Info((object)("[Point-and-Plant] Harvest Inner Source: " + exception.Source));
                                    }
                                }

                            }

                        }

                    }
                }
            }
            catch (Exception exception)
            {
                if (PointAndPlant.loggingEnabled)
                {
                    Log.Info("[Point-and-Plant] Harvest Outer Exception: " + exception.Message);
                    Log.Error("[Point-and-Plant] Harvest Outer Stack Trace: " + exception);
                    //Log.Info((object)("[Point-and-Plant] Harvest Outer Source: " + exception.Source));
                }
            }


        }


        /**
         * Gets some values used by the plow.
         */
        private void drawPlowOverlay(object sender, EventArgs e)
        {
            try
            {
                if (this.buildingTiles == null)
                    this.buildingTiles = Game1.content.Load<Texture2D>("LooseSprites\\buildingPlacementTiles");

                KeyboardState keyboard = Keyboard.GetState();

                if ((!keyboard.IsKeyDown(PointAndPlant.grassKey) &&
                    !keyboard.IsKeyDown(PointAndPlant.plowKey) &&
                    !keyboard.IsKeyDown(PointAndPlant.plantKey) &&
                    !keyboard.IsKeyDown(PointAndPlant.growKey) &&
                    !keyboard.IsKeyDown(PointAndPlant.harvestKey)) ||
                    Game1.currentLocation == null ||
                    (Game1.player == null ||
                    Game1.hasLoadedGame == false) ||
                    ((Game1.player).UsingTool ||
                    !(Game1.player).CanMove ||
                    (Game1.activeClickableMenu != null ||
                    Game1.CurrentEvent != null)) ||
                    Game1.gameMode != 3)
                    return;

                this.mouseX = Game1.getMouseX() + @Game1.viewport.X;
                this.mouseY = Game1.getMouseY() + @Game1.viewport.Y;

                this.tileX = this.mouseX / Game1.tileSize;
                this.tileY = this.mouseY / Game1.tileSize;
                PointAndPlant.vector = new Vector2(this.tileX, this.tileY);

                if (keyboard.IsKeyDown(PointAndPlant.plowKey))
                {
                    foreach (Vector2 vector2 in this.tilesAffected(PointAndPlant.vector, 0, Game1.player))
                        Game1.spriteBatch.Draw(this.buildingTiles, Game1.GlobalToLocal(Game1.viewport, vector2 * Game1.tileSize), Game1.getSourceRectForStandardTileSheet(this.buildingTiles, 0), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
                }
                else if (keyboard.IsKeyDown(PointAndPlant.grassKey))
                {
                    foreach (Vector2 vector2 in this.tilesAffectedGrass(PointAndPlant.vector, Game1.player))
                        Game1.spriteBatch.Draw(this.buildingTiles, Game1.GlobalToLocal(Game1.viewport, vector2 * Game1.tileSize), Game1.getSourceRectForStandardTileSheet(this.buildingTiles, 0), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
                }
            }
            catch (Exception exception)
            {
                if (PointAndPlant.loggingEnabled)
                {
                    Log.Info("[Point-and-Plant] Drawing Plow Overlay: " + exception.Message);
                    Log.Error("[Point-and-Plant] Drawing Plow Overlay Stack Trace: " + exception);
                    //Log.Info((object)("[Point-and-Plant] Drawing Plow Overlay Source: " + exception.Source));
                }
            }
        }


        private List<Vector2> tilesAffected(Vector2 tileLocation, int power, SFarmer who)
        {
            List<Vector2> tiles = new List<Vector2>();

            if (who.currentLocation.name.Equals("Farm") || who.currentLocation.name.Contains("Greenhouse"))
            {
                for (int x = 0; x < this.plowWidth; x++)
                {
                    for (int y = 0; y < this.plowHeight; y++)
                    {
                        tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                    }
                }
            }
            else
            {
                for (int x = 0; x < 15; x++)
                {
                    for (int y = 0; y < 15; y++)
                    {
                        tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                    }
                }
            }

            return tiles;
        }

        private List<Vector2> tilesAffectedTree(Vector2 tileLocation, SFarmer who)
        {
            List<Vector2> tiles = new List<Vector2>();

            int min = this.plantRadius * -1;

            for (int x = min; x <= this.plantRadius; x++)
            {
                for (int y = min; y <= this.plantRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            return tiles;
        }

        private List<Vector2> tilesAffectedRock(Vector2 tileLocation, SFarmer who)
        {
            List<Vector2> tiles = new List<Vector2>();

            int min = this.plantRadius * -1;

            for (int x = min; x <= this.plantRadius; x++)
            {
                for (int y = min; y <= this.plantRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            return tiles;
        }

        private List<Vector2> tilesAffectedGrass(Vector2 tileLocation, SFarmer who)
        {
            List<Vector2> tiles = new List<Vector2>();

            int min = this.plantRadius * -1;

            for (int x = min; x <= this.plantRadius; x++)
            {
                for (int y = min; y <= this.plantRadius; y++)
                {
                    tiles.Add(new Vector2(this.tileX + x, this.tileY + y));
                }
            }

            return tiles;
        }

    }
}
