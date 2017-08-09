using PointAndPlant;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System.Collections.Generic;
using SFarmer = StardewValley.Farmer;

public class PAPSickle : MeleeWeapon
{
    int radius = 4;

    public PAPSickle(int spriteIndex, int radius) : base(spriteIndex)
    {
        this.radius = radius;
    }

    public void DoDamage(GameLocation location, int x, int y, int facingDirection, int power, SFarmer who)
    {
        this.isOnSpecial = false;

        if (this.type != 2)
            ((Tool)this).DoFunction(location, x, y, power, who);

        string sound = "";

        List<Vector2> newvec = new List<Vector2>();

        int min = radius * -1;
        int max = radius;

        for (int nx = min; nx <= max; nx++)
        {
            for (int ny = min; ny <= max; ny++)
            {
                newvec.Add( PointAndPlant.PointAndPlant.vector + new Vector2(nx, ny) );
            }
        }

        foreach (Vector2 key in newvec)
        {
            try
            {
                if (((Dictionary<Vector2, TerrainFeature>)location.terrainFeatures).ContainsKey(key) && ((Dictionary<Vector2, TerrainFeature>)location.terrainFeatures)[key].performToolAction((Tool)this, 0, key))
                    ((SerializableDictionary<Vector2, TerrainFeature>)location.terrainFeatures).Remove(key);
                if (((Dictionary<Vector2, Object>)location.objects).ContainsKey(key) && ((string)((Dictionary<Vector2, Object>)location.objects)[key].name).Contains("Weed") && ((Dictionary<Vector2, Object>)location.objects)[key].performToolAction((Tool)this))
                    ((SerializableDictionary<Vector2, Object>)location.objects).Remove(key);
                if (location.performToolAction((Tool)this, (int)key.X, (int)key.Y))
                    break;
            }
            catch (System.Exception exception)
            {
                
                //StardewModdingAPI.Log.Info((object)("[Point-and-Plant] Exception: " + exception.Message));
                //StardewModdingAPI.Log.Info((object)("[Point-and-Plant] Stack Trace: " + exception.StackTrace));
            }
            
        }
        if (!sound.Equals(""))
        {
            Game1.playSound(sound);
        }
            
        ((Tool)this).CurrentParentTileIndex = (int)((Tool)this).indexOfMenuItemView;

        if (who == null || !who.isRidingHorse())
        {
            return;
        }
            
        who.completelyStopAnimatingOrDoingAction();
    }
}
