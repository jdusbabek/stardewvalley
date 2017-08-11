using StardewValley;
using StardewValley.Objects;

namespace SortMyStuff
{
    internal class ItemContainer
    {
        /*********
        ** Accessors
        *********/
        public Chest chest { get; set; }
        public Item item { get; set; }


        /*********
        ** Public methods
        *********/
        public ItemContainer() { }

        public ItemContainer(Chest c, Item i)
        {
            chest = c;
            item = i;
        }
    }
}
