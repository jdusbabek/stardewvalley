using Microsoft.Xna.Framework;
using StardewValley.Objects;

namespace SortMyStuff.chest
{
    internal class ChestDef
    {
        public int x;
        public int y;
        public Vector2 vector;
        public string location;
        public int count;
        public Chest chest;

        public ChestDef()
        {
            x = -1;
            y = -1;
        }

        public ChestDef(int x, int y)
        {
            this.x = x;
            this.y = y;

            this.vector = new Vector2(x, y);

            location = "Farm";
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

        override public string ToString()
        {
            return location + " " + this.vector + " #" + count;
        }
    }

}
