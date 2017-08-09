using StardewValley;
using StardewValley.Objects;

namespace SortMyStuff
{
    internal class ItemContainer
    {
        public Chest chest;
        public Item item;

        public ItemContainer() { }

        public ItemContainer(Chest c, Item i)
        {
            chest = c;
            item = i;
        }
    }
}
