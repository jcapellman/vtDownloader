using Microsoft.Win32;

using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using VTDownloader.Objects;

namespace VTDownloader.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _vtKey;

        public string VTKey
        {
            get => _vtKey;

            set
            {
                _vtKey = value;

                OnPropertyChanged();

                UpdateButtons();
            }
        }


        private string _FileHash;

        public string FileHash
        {
            get => _FileHash;

            set
            {
                _FileHash = value;

                OnPropertyChanged();

                UpdateButtons();
            }
        }

        private bool _EnabledDownload;

        public bool EnabledDownload
        {
            get => _EnabledDownload;

            set
            {
                _EnabledDownload = value;

                OnPropertyChanged();
            }
        }

        private bool _EnabledVTSave;

        public bool EnabledVTSave
        {
            get => _EnabledVTSave;

            set
            {
                _EnabledVTSave = value;

                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            try
            {
                LoadSettings();
            }
            catch (FileNotFoundException fnfe)
            {
                // Handle exception
            }
        }

        private void LoadSettings()
        {
            var settingsItem = SettingsItem.Load();

            VTKey = settingsItem.VTKey;
        }

        public void SaveSettings()
        {
            SettingsItem.SaveAsync(VTKey);
        }

        private void UpdateButtons()
        {
            EnabledDownload = !string.IsNullOrEmpty(FileHash) && !string.IsNullOrEmpty(VTKey);

            EnabledVTSave = !string.IsNullOrEmpty(VTKey);
        }

        public async Task<bool> DownloadFileAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var file = await httpClient.GetByteArrayAsync(
                    $"https://www.virustotal.com/vtapi/v2/file/download?apikey={VTKey}&hash={FileHash}");

                if (file == null)
                {
                    return false;
                }

                var sfd = new SaveFileDialog
                {
                    FileName = FileHash
                };

                var result = sfd.ShowDialog();

                if (!result.HasValue || !result.Value)
                {
                    return false;
                }

                await File.WriteAllBytesAsync(sfd.FileName, file);

                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}