using StardewModdingAPI;

namespace MailOrderPigs
{
    internal class MailOrderPigsConfig : Config
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
