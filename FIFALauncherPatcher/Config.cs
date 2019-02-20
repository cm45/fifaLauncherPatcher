using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFALauncherPatcher
{
    public struct Config
    {
        public string path;
        public bool skipGameLauncher;
        public bool skipLanguageSelection;
        public bool forceMetricUnits;

        public Config(string path)
        {
            this.path = path;
            this.skipGameLauncher = true;
            this.skipLanguageSelection = true;
            this.forceMetricUnits = false;
        }
    }
}
