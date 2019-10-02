using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

using Library;

namespace FIFALauncherPatcher
{
    public partial class MainWindow : Window
    {
        readonly Patcher patcher = new Patcher();

        public MainWindow()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            AppConfig cfg = ConfigManager.AppConfig;

            tboxPath.Text = cfg.path;
            checkBoxSkipLauncher.IsChecked = cfg.skipGameLauncher;
            checkBoxSkipLanguageSelection.IsChecked = cfg.skipLanguageSelection;
            checkBoxForceMetric.IsChecked = cfg.forceMetricUnits;
        }

        #region Events

        private void TboxPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tboxPath.Text = patcher.OpenGameFolderDialog() ?? tboxPath.Text;
        }

        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            tboxPath.Text = patcher.OpenGameFolderDialog() ?? tboxPath.Text;
        }

        private void BtnPatch_Click(object sender, RoutedEventArgs e)
        {
            AppConfig config = new AppConfig()
            {
                path = tboxPath.Text,
                skipGameLauncher = (bool)checkBoxSkipLauncher.IsChecked,
                skipLanguageSelection = (bool)checkBoxSkipLanguageSelection.IsChecked,
                forceMetricUnits = (bool)checkBoxForceMetric.IsChecked
            };

            patcher.Patch(config, Patcher.MessageType.MESSAGEBOX);
        }

        #endregion

    }
}
