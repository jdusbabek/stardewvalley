namespace PointAndPlant
{
    internal class PAPConfig
    {
        public string plowKey { get; set; } = "Z";
        public string plantKey { get; set; } = "A";
        public string growKey { get; set; } = "S";
        public string harvestKey { get; set; } = "D";

        public int plantRadius { get; set; } = 4;
        public int growRadius { get; set; } = 4;
        public int harvestRadius { get; set; } = 4;
        public int plowWidth { get; set; } = 3;
        public int plowHeight { get; set; } = 6;

        public bool plowEnabled { get; set; } = true;
        public bool plantEnabled { get; set; } = true;
        public bool growEnabled { get; set; } = true;
        public bool harvestEnabled { get; set; } = true;

        public int fertilizer { get; set; }

        public bool loggingEnabled { get; set; }
    }
}
