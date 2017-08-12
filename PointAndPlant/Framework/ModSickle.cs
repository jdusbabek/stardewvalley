using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Tools;
using SFarmer = StardewValley.Farmer;

internal class ModSickle : MeleeWeapon
{
    /*********
    ** Properties
    *********/
    private readonly int Radius = 4;
    private readonly Vector2 Vector;


    /*********
    ** Public methods
    *********/
    public ModSickle(int spriteIndex, int radius, Vector2 vector)
        : base(spriteIndex)
    {
        this.Radius = radius;
        this.Vector = vector;
    }

    public void DoDamage(GameLocation location, int x, int y, int facingDirection, int power, SFarmer who)
    {
        this.isOnSpecial = false;

        if (this.type != 2)
            this.DoFunction(location, x, y, power, who);

        string sound = "";

        List<Vector2> newvec = new List<Vector2>();

        int min = this.Radius * -1;
        int max = this.Radius;

        for (int nx = min; nx <= max; nx++)
        {
            for (int ny = min; ny <= max; ny++)
            {
                newvec.Add(this.Vector + new Vector2(nx, ny));
            }
        }

        foreach (Vector2 key in newvec)
        {
            try
            {
                if (location.terrainFeatures.ContainsKey(key) && location.terrainFeatures[key].performToolAction(this, 0, key))
                    location.terrainFeatures.Remove(key);
                if (location.objects.ContainsKey(key) && location.objects[key].name.Contains("Weed") && location.objects[key].performToolAction(this))
                    location.objects.Remove(key);
                if (location.performToolAction(this, (int)key.X, (int)key.Y))
                    break;
            }
            catch
            {
                //StardewModdingAPI.Log.Info((object)("[Point-and-Plant] Exception: " + exception.Message));
                //StardewModdingAPI.Log.Info((object)("[Point-and-Plant] Stack Trace: " + exception.StackTrace));
            }

        }
        if (!sound.Equals(""))
            Game1.playSound(sound);

        this.CurrentParentTileIndex = this.indexOfMenuItemView;

        if (who == null || !who.isRidingHorse())
            return;

        who.completelyStopAnimatingOrDoingAction();
    }
}
