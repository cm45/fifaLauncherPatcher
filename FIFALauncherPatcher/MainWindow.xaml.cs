using IniParser;
using IniParser.Model;
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
        FileIniDataParser iniParser = new FileIniDataParser();

        #region SETTINGS

        private bool useMetric = true;
        

        #endregion

        private string gamePath;
        public string GamePath
        {
            get => gamePath;
            set
            {
                gamePath = value;
                tboxPath.Text = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Patch()
        {
            GamePath = tboxPath.Text;
            if (!Directory.Exists(GamePath))
            {
                System.Windows.MessageBox.Show("Invalid Path");
                return;
            }

            string configPath = Path.Combine(GamePath, "FIFASetup", "config.ini");
            string localePath = Path.Combine(GamePath, "Data", "locale.ini");

            // FIFA uses // to start a comment instead of ;
            iniParser.Parser.Configuration.CommentString = "//";

            #region CONFIG

            // Set: Bypass Launcher Settings
            IniData configData = iniParser.ReadFile(configPath);
            configData.Global["AUTO_LAUNCH"] = "1";
            iniParser.WriteFile(configPath, configData);

            #endregion



            #region LOCALE-CONFIG

            // Set: Bypass Language Selection
            IniData localeData = iniParser.ReadFile(localePath);
            localeData["LOCALE"]["USE_LANGUAGE_SELECT"] = "0";

            // Set: Use Metric units for weight and length
            localeData["REGIONALIZATION_eng_us"]["LENGTH_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            localeData["REGIONALIZATION_eng_us"]["WEIGHT_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            iniParser.WriteFile(localePath, localeData);

            #endregion

            MessageBox.Show("Done Patching!");
        }

        public void OpenGameFolderDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = @"Select Folder (Example: C:\Program Files (x86)\Origin Games\",
                ShowNewFolderButton = false
            };

            DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                GamePath = dialog.SelectedPath;
            }
        }

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
    }
}
