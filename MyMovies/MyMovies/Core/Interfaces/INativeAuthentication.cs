using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Interfaces
{
    interface INativeAuthentication
    {
        // <summary>
        /// Checks whether the current platform supports native authentication
        /// </summary>
        bool IsSupported();
        /// <summary>
        /// Prompt the user to authenticate using the native authentication system
        /// </summary>
        Task<AuthenticationResult> PromptLoginAsync(string prompt);
    }
}
