using Microsoft.Xna.Framework;
using StardewValley.Objects;

namespace StardewLib
{
    internal class ChestDef
    {
        /*********
        ** Accessors
        *********/
        public int x { get; set; }
        public int y { get; set; }
        public Vector2 vector { get; set; }
        public string location { get; set; }
        public int count { get; set; }
        public Chest chest { get; set; }


        /*********
        ** Public methods
        *********/
        public ChestDef()
        {
            this.x = -1;
            this.y = -1;
        }

        public ChestDef(int x, int y)
        {
            this.x = x;
            this.y = y;

            this.vector = new Vector2(x, y);

            this.location = "Farm";
        }

        public ChestDef(int x, int y, string location)
        {
            this.x = x;
            this.y = y;

            this.vector = new Vector2(x, y);

            this.location = location;
        }

        public ChestDef(int x, int y, string location, int count)
        {
            this.x = x;
            this.y = y;
            this.vector = new Vector2(x, y);
            this.location = location;
            this.count = count;
        }

        public ChestDef(int x, int y, string location, int count, Chest chest)
        {
            this.x = x;
            this.y = y;
            this.vector = new Vector2(x, y);
            this.location = location;
            this.count = count;
            this.chest = chest;
        }

        public override string ToString()
        {
            return location + " " + this.vector + " #" + count;
        }
    }

}
