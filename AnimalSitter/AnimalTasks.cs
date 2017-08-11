using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StardewLib;

namespace ExtremePetting
{
    internal class AnimalTasks : IStats
    {
        /*********
        ** Accessors
        *********/
        public int animalsPet { get; set; } = 0;
        public int trufflesHarvested { get; set; } = 0;
        public int productsHarvested { get; set; } = 0;
        public int aged { get; set; } = 0;
        public int fed { get; set; } = 0;
        public int maxHappiness { get; set; } = 0;
        public int maxFriendship { get; set; } = 0;

        public int numActions { get; set; } = 0;
        public int totalCost { get; set; } = 0;


        /*********
        ** Public methods
        *********/
        public int getTaskCount()
        {
            return animalsPet + trufflesHarvested + productsHarvested + aged + fed + maxHappiness + maxFriendship;
        }

        public bool justGathering()
        {
            return (((animalsPet + aged + fed + maxHappiness + maxFriendship) == 0) && getTaskCount() > 0);
        }

        public IDictionary<string, object> GetFields()
        {
            return typeof(AnimalTasks)
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(this));
        }
    }
}
