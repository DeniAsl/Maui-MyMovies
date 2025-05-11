using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;

namespace MyMovies.Platforms.Services
{
    public class NativeAuthentication : INativeAuthentication
    {
        public Task<bool> IsSupported()
        {
            return Task.FromResult(false);
        }

        public Task<AuthenticationResult> PromptLoginAsync(string prompt)
        {
            throw new NotImplementedException();
        }
    }
}
