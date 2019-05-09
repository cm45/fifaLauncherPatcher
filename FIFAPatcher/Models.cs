using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
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
    }
}
