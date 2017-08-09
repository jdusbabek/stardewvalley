using Microsoft.Xna.Framework;

namespace Replanter
{
    internal class ChestDef
    {
        public int x;
        public int y;
        public Vector2 vector;
        public string location = "f";

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
