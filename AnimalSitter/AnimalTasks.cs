using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewLib;
using System.Reflection;

namespace ExtremePetting
{
    public class AnimalTasks : IStats
    {
        public int animalsPet = 0;
        public int trufflesHarvested = 0;
        public int productsHarvested = 0;
        public int aged = 0;
        public int fed = 0;
        public int maxHappiness = 0;
        public int maxFriendship = 0;

        public int numActions = 0;
        public int totalCost = 0;

        public int getTaskCount()
        {
            return animalsPet + trufflesHarvested + productsHarvested + aged + fed + maxHappiness + maxFriendship;
        }

        public bool justGathering()
        {
            if (((animalsPet + aged + fed + maxHappiness + maxFriendship) == 0) && getTaskCount() > 0)
                return true;
            else
                return false;
            
        }

        FieldInfo[] IStats.getFieldList()
        {
            return typeof(AnimalTasks).GetFields();
        }
    }
}
