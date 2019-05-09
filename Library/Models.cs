using System;

namespace Library
{
    [Serializable]
    public class AppConfig
    {
        public string path;
        public bool skipGameLauncher;
        public bool skipLanguageSelection;
        public bool forceMetricUnits;

        public AppConfig(string path)
        {
            this.path = path;
            this.skipGameLauncher = true;
            this.skipLanguageSelection = true;
            this.forceMetricUnits = false;
        }
        public AppConfig() { }
    }
}
