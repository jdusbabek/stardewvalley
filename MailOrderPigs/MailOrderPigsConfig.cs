using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace MailOrderPigs
{
    public class MailOrderPigsConfig : Config
    {
        public string keybind { get; set; }
        public bool allowOvercrowding { get; set; }
        public bool enableLogging { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            keybind = "PageUp";
            allowOvercrowding = false;
            enableLogging = false;

            return this as T;
        }

    }
}
