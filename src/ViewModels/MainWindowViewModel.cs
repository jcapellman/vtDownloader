using Microsoft.Win32;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

using VTDownloader.Enums;
using VTDownloader.Helpers;
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

        private bool _enabledWindow;

        public bool EnabledWindow
        {
            get => _enabledWindow;

            set
            {
                _enabledWindow = value;

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
            } finally
            {
                EnabledWindow = true;
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

        public async void DownloadFileAsync()
        {
            EnabledWindow = false;

            var downloadResult = await VTHTTPHandler.DownloadAsync(VTKey, FileHash);

            switch (downloadResult.Status)
            {
                case DownloadResponseStatus.SUCCESS:
                    var sfd = new SaveFileDialog
                    {
                        FileName = FileHash
                    };

                    var result = sfd.ShowDialog();

                    if (!result.HasValue || !result.Value)
                    {
                        EnabledWindow = true;

                        return;
                    }

                    File.WriteAllBytes(sfd.FileName, downloadResult.Data);

                    MessageBox.Show($"Successfully saved to {sfd.FileName}");
                    break;
                default:
                    if (downloadResult.DownloadException == null)
                    {
                        MessageBox.Show($"Error ({downloadResult.Status}) while attempting to download");
                    }
                    else
                    {
                        MessageBox.Show($"Error ({downloadResult.Status}) | Detail: {downloadResult.DownloadException}");
                    }
                    break;
            }

            EnabledWindow = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}