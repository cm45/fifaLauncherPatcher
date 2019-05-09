using System;
using System.IO;
using Models;

using IniParser;
using Newtonsoft.Json;
using IniParser.Model;
using System.Windows.Forms;

namespace Tools
{
    public static class Paths
    {
        public static string DEFAULT_GAMEPATH = @"C:\Program Files (x86)\Origin Games\FIFA 19";
        public static string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory, "config.json");
    }
    public static class ConfigManager
    {
        public static AppConfig AppConfig
        {
            get
            {
                if (!File.Exists(Paths.CONFIG_PATH)) return Reset();
                try
                {
                    return JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(Paths.CONFIG_PATH));
                }
                catch
                {
                    //MessageBox.Show("Couldn't read configurations... Resetting config file...");
                    return Reset();
                }
            }
            set
            {
                try
                {
                    File.WriteAllText(Paths.CONFIG_PATH, JsonConvert.SerializeObject(value, Formatting.Indented));
                }
                catch
                {
                    throw new NotImplementedException();
                    //MessageBox.Show("Couldn't save configurations");
                }
            }
        }

        private static AppConfig Reset()
        {
            AppConfig = new AppConfig(Paths.CONFIG_PATH);
            return AppConfig;
        }

        /// <summary>
        /// Loads the saved application options
        /// </summary>
        public static void LoadConfig()
        {
            //AppConfig cfg = AppConfig;

            //GamePath = cfg.path;
            //checkBoxSkipLauncher.IsChecked = cfg.skipGameLauncher;
            //checkBoxSkipLanguageSelection.IsChecked = cfg.skipLanguageSelection;
            //checkBoxForceMetric.IsChecked = cfg.forceMetricUnits;
        }
    }
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
        public void Patch()
        {
            //GamePath = tboxPath.Text;

            // Check if Directory exists:
            if (!Directory.Exists(GamePath))
            {
                //System.Windows.MessageBox.Show("Invalid Path");
                return;
            }
            // Check if selected correct Directory:
            else if (!File.Exists(Path.Combine(GamePath, "FIFA19.exe")))
            {
                //System.Windows.MessageBox.Show("Invalid Path");
                return;
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

            //SaveConfig();

            MessageBox.Show("Done Patching!");
        }

        /// <summary>
        /// Opens folder dialog sets Gamepath after successful selection
        /// </summary>
        public void OpenGameFolderDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                //Description = $"Select Folder (Example: {DEFAULT_GAMEPATH})",
                ShowNewFolderButton = false,
                //SelectedPath = ApplicationConfig.path ?? DEFAULT_GAMEPATH
            };

            DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                GamePath = dialog.SelectedPath;
            }
        }
    }
}
