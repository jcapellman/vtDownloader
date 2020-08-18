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

        private void BtnDownloader_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DownloadFileAsync();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveSettings();
        }
    }
}