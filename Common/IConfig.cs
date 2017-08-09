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
