using System.Windows;

using VTDownloader.ViewModels;

namespace VTDownloader
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnDownloader_OnClick(object sender, RoutedEventArgs e)
        {
            var result = await ViewModel.DownloadFileAsync();

            if (result)
            {
                MessageBox.Show("Datei gespeichert");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveSettings();
        }
    }
}