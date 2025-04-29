using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Interfaces
{
    public interface ISettingsService
    {
        Task<Setting> GetSettingAsync();
        Task<bool> Update(bool receiveNotifications, int notificationInterval);
    }
}
