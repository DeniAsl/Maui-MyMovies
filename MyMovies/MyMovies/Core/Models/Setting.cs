using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Models
{
    public class Setting
    {
        public bool ReceiveNotifications { get; set; }
        public int NotificationInterval { get; set; } = 24;
    }
}
