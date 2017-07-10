using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace StardewLib
{
    public class IConfig : Config
    {
        public string whoChecks { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            whoChecks = "spouse";

            return this as T;
        }
    }
}
