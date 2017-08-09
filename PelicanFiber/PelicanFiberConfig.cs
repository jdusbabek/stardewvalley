using StardewModdingAPI;

namespace PelicanFiber
{
    internal class PelicanFiberConfig : Config
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
