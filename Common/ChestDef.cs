using Microsoft.Xna.Framework;

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
    }

}
