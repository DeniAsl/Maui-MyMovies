using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Models
{
    public class AuthenticationResult
    {
        public bool Authenticated { get; set; }
        public string ErrorMessage { get; set; }
    }
}
