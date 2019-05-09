using System.IO;

using IniParser;
using IniParser.Model;
using System.Windows.Forms;

using Library;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Library
{
    public class Patcher
    {
        readonly FileIniDataParser iniParser = new FileIniDataParser();

        private AppConfig Config
        {
            get => ConfigManager.AppConfig;
            set => ConfigManager.AppConfig = value;
        }

        private string gamePath;
        public string GamePath
        {
            get => gamePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                gamePath = value;
                //tboxPath.Text = value;
            }
        }

        /// <summary>
        /// Changes Application-Config files
        /// </summary>
        public bool Patch(AppConfig cfg = null)
        {
            Regex fileNameRegEx = new Regex(@"FIFA..");

            if (cfg != null) Config = cfg;
            GamePath = Config.path;

            // Check if Directory exists:
            if (!Directory.Exists(GamePath))
            {
                MessageBox.Show("Invalid Path");
                return false;
            }
            // Check if selected correct Directory:
            else if (Directory.GetFiles(GamePath, "*.exe").Where(GamePath => fileNameRegEx.IsMatch(GamePath)).Count() <= 0)
            {
                MessageBox.Show("Invalid Path");
                return false;
            }            

            string configPath = Path.Combine(GamePath, "FIFASetup", "config.ini");
            string localePath = Path.Combine(GamePath, "Data", "locale.ini");

            #region CONFIG

            // FIFA uses // to start a comment instead of ;
            iniParser.Parser.Configuration.CommentString = "//";

            // Set: Bypass Launcher Settings
            IniData configData = iniParser.ReadFile(configPath);
            configData.Global["AUTO_LAUNCH"] = Config.skipGameLauncher ? "1" : "0";
            iniParser.WriteFile(configPath, configData);

            // Set: Bypass Language Selection
            IniData localeData = iniParser.ReadFile(localePath);
            localeData["LOCALE"]["USE_LANGUAGE_SELECT"] = Config.skipLanguageSelection ? "0" : "1";

            // Set: Use Metric units for weight and length
            bool useMetric = Config.forceMetricUnits;
            localeData["REGIONALIZATION_eng_us"]["LENGTH_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            localeData["REGIONALIZATION_eng_us"]["WEIGHT_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            iniParser.WriteFile(localePath, localeData);

            #endregion

            Config = Config;
            MessageBox.Show("Done Patching!");
            return true;
        }

        /// <summary>
        /// Opens folder dialog sets Gamepath after successful selection
        /// </summary>
        public string OpenGameFolderDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = $"Select Folder (Example: {Paths.DEFAULT_GAMEPATH})",
                ShowNewFolderButton = false,
                SelectedPath = Paths.CONFIG_PATH ?? Paths.DEFAULT_GAMEPATH
            };

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                GamePath = dialog.SelectedPath;
                return dialog.SelectedPath;
            }
            else return null;
        }
    }
}
