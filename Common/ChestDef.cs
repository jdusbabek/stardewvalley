using Microsoft.Xna.Framework;

namespace StardewLib
{
    internal class ChestDef
    {
        public int x;
        public int y;
        public Vector2 vector;
        public string location;

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
    }

}
