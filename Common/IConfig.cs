using StardewModdingAPI;

namespace StardewLib
{
    internal class IConfig : Config
    {
        public string whoChecks { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            whoChecks = "spouse";

            return this as T;
        }
    }
}
