using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

namespace FIFALauncherPatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string DEFAULT_GAMEPATH = @"C:\Program Files (x86)\Origin Games\FIFA 19";

        FileIniDataParser iniParser = new FileIniDataParser();

        private string configPath = Path.Combine(Environment.CurrentDirectory, "config.cfg");


        public Config ApplicationConfig
        {
            get
            {
                if (!File.Exists(configPath)) return ResetConfig();

                try
                {
                    return JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
                }
                catch
                {
                    MessageBox.Show("Couldn't read configurations... Resetting config file...");
                    return ResetConfig();
                }
            }
            set
            {
                try
                {
                    File.WriteAllText(configPath, JsonConvert.SerializeObject(value, Formatting.Indented));
                }
                catch
                {
                    MessageBox.Show("Couldn't save configurations");
                }
            }
        }

        private string gamePath;
        public string GamePath
        {
            get => gamePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                gamePath = value;
                tboxPath.Text = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadConfig();
        }

        /// <summary>
        /// Reads current configurations and save them in a file
        /// </summary>
        private void SaveConfig()
        {
            ApplicationConfig = new Config()
            {
                path = GamePath,
                skipGameLauncher = (bool)checkBoxSkipLauncher.IsChecked,
                skipLanguageSelection = (bool)checkBoxSkipLanguageSelection.IsChecked,
                forceMetricUnits = (bool)checkBoxForceMetric.IsChecked
            };
        }

        /// <summary>
        /// Resets Config
        /// </summary>
        /// <returns>Default Config</returns>
        Config ResetConfig()
        {
            Config defaultConfig = new Config(DEFAULT_GAMEPATH);
            ApplicationConfig = defaultConfig;
            return defaultConfig;
        }

        /// <summary>
        /// Loads the saved application options
        /// </summary>
        private void LoadConfig()
        {
            Config cfg = ApplicationConfig;

            GamePath = cfg.path;
            checkBoxSkipLauncher.IsChecked = cfg.skipGameLauncher;
            checkBoxSkipLanguageSelection.IsChecked = cfg.skipLanguageSelection;
            checkBoxForceMetric.IsChecked = cfg.forceMetricUnits;
        }

        /// <summary>
        /// Changes Application-Config files
        /// </summary>
        public void Patch()
        {
            GamePath = tboxPath.Text;

            // Check if Directory exists:
            if (!Directory.Exists(GamePath))
            {
                System.Windows.MessageBox.Show("Invalid Path");
                return;
            }
            // Check if selected correct Directory:
            else if (!File.Exists(Path.Combine(GamePath, "FIFA19.exe")))
            {
                System.Windows.MessageBox.Show("Invalid Path");
                return;
            }

            string configPath = Path.Combine(GamePath, "FIFASetup", "config.ini");
            string localePath = Path.Combine(GamePath, "Data", "locale.ini");

            #region CONFIG

            // FIFA uses // to start a comment instead of ;
            iniParser.Parser.Configuration.CommentString = "//";

            // Set: Bypass Launcher Settings
            IniData configData = iniParser.ReadFile(configPath);
            configData.Global["AUTO_LAUNCH"] = (bool)checkBoxSkipLauncher.IsChecked ? "1" : "0";
            iniParser.WriteFile(configPath, configData);

            // Set: Bypass Language Selection
            IniData localeData = iniParser.ReadFile(localePath);
            localeData["LOCALE"]["USE_LANGUAGE_SELECT"] = (bool)checkBoxSkipLanguageSelection.IsChecked ? "0" : "1";

            // Set: Use Metric units for weight and length
            bool useMetric = (bool)checkBoxForceMetric.IsChecked;
            localeData["REGIONALIZATION_eng_us"]["LENGTH_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            localeData["REGIONALIZATION_eng_us"]["WEIGHT_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            iniParser.WriteFile(localePath, localeData);

            #endregion

            SaveConfig();

            MessageBox.Show("Done Patching!");
        }

        /// <summary>
        /// Opens folder dialog sets Gamepath after successful selection
        /// </summary>
        public void OpenGameFolderDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = $"Select Folder (Example: {DEFAULT_GAMEPATH})",
                ShowNewFolderButton = false,
                SelectedPath = ApplicationConfig.path ?? DEFAULT_GAMEPATH
            };

            DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                GamePath = dialog.SelectedPath;
            }
        }

        #region Events

        private void TboxPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenGameFolderDialog();
        }

        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenGameFolderDialog();
        }

        private void BtnPatch_Click(object sender, RoutedEventArgs e)
        {
            Patch();
        }

        #endregion

    }
}
