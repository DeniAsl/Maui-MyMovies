using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Platforms.Services
{
    public class NativeAuthentication : INativeAuthentication
    {
        public bool IsSupported()
        {
            return false; // this stub is not ready, so feature is not supported
        }
        public async Task<AuthenticationResult> PromptLoginAsync(string prompt)
        {
            throw new NotImplementedException();
        }
    }
}
