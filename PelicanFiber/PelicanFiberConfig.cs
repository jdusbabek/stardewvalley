using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace PelicanFiber
{
    public class PelicanFiberConfig : Config
    {
        public string keybind { get; set; }

        public bool internetFilter { get; set; }

        public bool giveAchievements { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            keybind = "PageDown";
            internetFilter = false;
            giveAchievements = false;

            return this as T;
        }

    }
}
