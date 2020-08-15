using System;
using System.IO;
using System.Text.Json;

namespace VTDownloader.Objects
{
    public class SettingsItem
    {
        private const string FILENAME_SETTINGS = "settings.json";

        public string VTKey { get; set; }

        public static SettingsItem Load(string fileName = FILENAME_SETTINGS)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, FILENAME_SETTINGS);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException();
            }

            var json = File.ReadAllText(fullPath);

            return JsonSerializer.Deserialize<SettingsItem>(json);
        }

        public static async void SaveAsync(string vtKey, string fileName = FILENAME_SETTINGS)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, FILENAME_SETTINGS);

            var settings = new SettingsItem
            {
                VTKey = vtKey
            };

            var json = JsonSerializer.Serialize(settings);

            await File.WriteAllTextAsync(fullPath, json);
        }
    }
}