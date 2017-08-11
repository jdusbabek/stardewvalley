namespace StardewLib
{
    internal class Log
    {
        /*********
        ** Properties
        *********/
        private readonly bool enabled;


        /*********
        ** Public methods
        *********/
        public Log(bool enabled)
        {
            this.enabled = enabled;
        }

        public void INFO(object o)
        {
            if (this.enabled)
                StardewModdingAPI.Log.Info(o);
        }

        public void ERROR(object o)
        {
            if (this.enabled)
                StardewModdingAPI.Log.Error(o);
        }

        public void DEBUG(object o)
        {
            if (this.enabled)
                StardewModdingAPI.Log.Debug(o);
        }

        public void force_INFO(object o)
        {
            StardewModdingAPI.Log.Info(o);
        }

        public void force_ERROR(object o)
        {
            StardewModdingAPI.Log.Error(o);
        }

        public void force_DEBUG(object o)
        {
            StardewModdingAPI.Log.Debug(o);
        }
    }
}
