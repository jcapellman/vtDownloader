using System.IO;
using System.Net.Http;
using System.Windows;

using Microsoft.Win32;

namespace VTDownloader
{
    public partial class MainWindow : Window
    {
        private const string KEY = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnDownloader_OnClick(object sender, RoutedEventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                var file = await httpClient.GetByteArrayAsync(
                    $"https://www.virustotal.com/vtapi/v2/file/download?apikey={KEY}&hash={txtBxHash.Text}");

                if (file == null)
                {
                    MessageBox.Show("Die Datei existiert nicht");

                    return;
                }

                var sfd = new SaveFileDialog
                {
                    FileName = txtBxHash.Text
                };

                var result = sfd.ShowDialog();

                if (!result.HasValue || !result.Value)
                {
                    return;
                }

                File.WriteAllBytes(sfd.FileName, file);

                MessageBox.Show($"Datei gespeichert in {sfd.FileName}");
            }
        }
    }
}