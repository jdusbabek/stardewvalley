using Microsoft.Xna.Framework;

namespace Replanter
{
    internal class ChestDef
    {
        /*********
        ** Accessors
        *********/
        public int x { get; set; }
        public int y { get; set; }
        public Vector2 vector { get; set; }
        public string location { get; set; } = "f";


        /*********
        ** Public methods
        *********/
        public ChestDef()
        {
            x = 0;
            y = 0;
        }

        public ChestDef(int x, int y)
        {
            this.x = x;
            this.y = y;

            this.vector = new Vector2((float)x, (float)y);
        }
    }
}
