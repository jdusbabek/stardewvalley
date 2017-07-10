using StardewValley.Objects;
using StardewValley;

namespace SortMyStuff
{
    public class ItemContainer
    {
        public Chest chest;
        public Item item;

        public ItemContainer()
        {

        }

        public ItemContainer(Chest c, Item i)
        {
            chest = c;
            item = i;
        }
    }
}
