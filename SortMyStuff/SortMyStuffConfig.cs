using StardewModdingAPI;

namespace SortMyStuff
{
    internal class SortMyStuffConfig : Config
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
