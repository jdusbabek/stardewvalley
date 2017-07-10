using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SortMyStuff
{
    public class SortMyStuffConfig : Config
    {
        public string keybind = "G";
        public string chests;

        public override T GenerateDefaultConfig<T>()
        {
            keybind = "G";
            chests = "";

            return this as T;
        }
    }
}
