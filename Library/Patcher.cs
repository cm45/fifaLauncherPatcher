using System.IO;

using IniParser;
using IniParser.Model;
using System.Windows.Forms;

using Library;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System;

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
        public bool Patch(AppConfig cfg = null, MessageType msgType = MessageType.NONE)
        {
            Regex fileNameRegEx = new Regex(@"FIFA..");

            if (cfg != null) Config = cfg;
            GamePath = Config.path;

            // Check if Directory exists:
            if (!Directory.Exists(GamePath))
            {
                Message("Invalid Path", msgType, MessageSuccess.ERROR);
                return false;
            }
            // Check if selected correct Directory:
            else if (Directory.GetFiles(GamePath, "*.exe").Where(GamePath => fileNameRegEx.IsMatch(GamePath)).Count() <= 0)
            {
                Message("Invalid Path", msgType, MessageSuccess.ERROR);
                return false;
            }

            string configPath = Path.Combine(GamePath, "FIFASetup", "config.ini");
            string localePath = Path.Combine(GamePath, "Data", "locale.ini");

            #region CONFIG

            // FIFA uses // to start a comment instead of ;
            iniParser.Parser.Configuration.CommentString = "//";

            // Set: Bypass Launcher Settings
            if (!File.Exists(configPath))
            {
                Message(configPath + " does not exist!", msgType, MessageSuccess.ERROR);
                return false;
            }
            try
            {
                IniData configData = iniParser.ReadFile(configPath);
                configData.Global["AUTO_LAUNCH"] = Config.skipGameLauncher ? "1" : "0";
                iniParser.WriteFile(configPath, configData);
            }
            catch (Exception)
            {
                Message("Can not edit: " + configPath + "\nCorrupted File or insufficient write permissions!", msgType, MessageSuccess.ERROR);
                return false;
            }



            // Set: Bypass Language Selection
            IniData localeData = iniParser.ReadFile(localePath);
            localeData["LOCALE"]["USE_LANGUAGE_SELECT"] = Config.skipLanguageSelection ? "0" : "1";

            // Set: Use Metric units for weight and length
            bool useMetric = Config.forceMetricUnits;
            localeData["REGIONALIZATION_eng_us"]["LENGTH_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            localeData["REGIONALIZATION_eng_us"]["WEIGHT_UNIT_FORMAT"] = useMetric ? "METRIC" : "IMPERIAL_US";
            iniParser.WriteFile(localePath, localeData);

            #endregion

            Message("Done Patching!", msgType, MessageSuccess.SUCCESS);
            Config = Config;
            return true;
        }

        public enum MessageType
        {
            NONE,
            MESSAGEBOX,
            CLI
        }
        public enum MessageSuccess
        {
            INFO,
            SUCCESS,
            WARNING,
            ERROR
        }

        private void Message(string msg, MessageType msgType = MessageType.NONE, MessageSuccess success = MessageSuccess.INFO)
        {
            switch (msgType)
            {
                case MessageType.NONE:
                    break;
                case MessageType.MESSAGEBOX:
                    MessageBox.Show(msg);
                    break;
                case MessageType.CLI:
                    Console.ForegroundColor = success == MessageSuccess.INFO ? ConsoleColor.White :
                                                success == MessageSuccess.SUCCESS ? ConsoleColor.Green :
                                                success == MessageSuccess.WARNING ? ConsoleColor.Yellow : ConsoleColor.Red;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }
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
