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
        private string gamePath;
        public string GamePath { get => gamePath;
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

        private int i;
        public void Patch()
        {
            if (!Directory.Exists(tboxPath.Text))
            {
                MessageBox.Show("Invalid Path");
                return;
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Patch();
        }
    }
}
