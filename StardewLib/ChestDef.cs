using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace StardewLib
{
    public class ChestDef
    {
        public int x;
        public int y;
        public Vector2 vector;
        public string location;

        public ChestDef()
        {
            x = -1;
            y = -1;
        }

        public ChestDef(int x, int y)
        {
            this.x = x;
            this.y = y;

            this.vector = new Vector2((float)x, (float)y);

            location = "Farm";
        }

        public ChestDef(int x, int y, string location)
        {
            this.x = x;
            this.y = y;

            this.vector = new Vector2((float)x, (float)y);

            this.location = location;
        }
    }

}
