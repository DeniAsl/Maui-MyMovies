using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyMovies.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly string targetFile = $"{FileSystem.AppDataDirectory}, MyMoviesSettings.json";

        public async Task<Setting> GetSettingAsync()
        {
            EnsureFileExists();

            string savedSerialized = await File.ReadAllTextAsync(targetFile);
            Setting setting = JsonSerializer.Deserialize<Setting>(savedSerialized);
            return setting;
        }

        public async Task<bool> Update(bool receiveNotifications, int notificationInterval)
        {
            Setting setting = new Setting
            {
                ReceiveNotifications = receiveNotifications,
                NotificationInterval = notificationInterval
            };
            try
            {
                await WriteSettings(setting);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing settings: {ex.Message}");
                return false;
            }
            return true;
        }

        private async Task WriteSettings(Setting setting)
        {
            EnsureFileExists();

            string serializedSetting = JsonSerializer.Serialize(setting);
            await File.WriteAllTextAsync(targetFile, serializedSetting);
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(targetFile))
            {
                File.WriteAllText(targetFile, JsonSerializer.Serialize(new Setting()));
            }
        }
    }
}
