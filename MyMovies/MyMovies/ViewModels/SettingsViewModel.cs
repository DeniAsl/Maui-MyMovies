using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;
using MyMovies.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyMovies.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        private bool receiveNotifications;
        public bool ReceiveNotifications
        {
            get { return receiveNotifications; }
            set
            {
                SetProperty(ref receiveNotifications, value);
            }
        }

        private int notificationInterval;
        public int NotificationInterval
        {
            get { return notificationInterval; }
            set
            {
                SetProperty(ref notificationInterval, value);
            }
        }

        private int[] notificationIntervals = new[] { 1, 3, 6, 12, 24, 48 };
        public int[] NotificationIntervals
        {
            get { return notificationIntervals; }
        }

        public ICommand GetSettingsCommand => new Command(async () =>
        {
            Setting setting = await _settingsService.GetSettingAsync();
            if (setting != null)
            {
                ReceiveNotifications = setting.ReceiveNotifications;
                NotificationInterval = setting.NotificationInterval;
            }
        });

        public ICommand UpdateNotificationIntervalCommand => new Command(async () =>
        {
            if (await _settingsService.Update(ReceiveNotifications, NotificationInterval))
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Notification interval updated successfully.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update notification interval.", "OK");
            }
        });

        public ICommand ResetSettingsCommand => new Command(async () =>
        {
            ReceiveNotifications = false;
            NotificationInterval = 1;
            if (await _settingsService.Update(ReceiveNotifications, NotificationInterval))
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Settings reset successfully.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to reset settings.", "OK");
            }
        });
    }
}
