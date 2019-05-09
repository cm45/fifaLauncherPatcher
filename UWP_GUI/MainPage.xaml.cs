using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Library;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly Patcher patcher = new Patcher();

        public MainPage()
        {
            this.InitializeComponent();
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

        private void BtnPatch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleForceMetric_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSkipLanguageSelection_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSkipLauncher_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void TboxPath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
