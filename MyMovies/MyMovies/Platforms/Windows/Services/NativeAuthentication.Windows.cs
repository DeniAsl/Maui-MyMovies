using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;

namespace MyMovies.Platforms.Services
{
    public class NativeAuthentication : INativeAuthentication
    {
        public bool IsSupported()
        {
            var result = UserConsentVerifier.CheckAvailabilityAsync()
            .AsTask()
            .GetAwaiter() //run synchronously because interface method is not a Task<>
            .GetResult();

            return result == UserConsentVerifierAvailability.Available;
        }

        public async Task<AuthenticationResult> PromptLoginAsync(string prompt)
        {
            var result = await UserConsentVerifier.RequestVerificationAsync(prompt);
            if (result == UserConsentVerificationResult.Verified)
            {
                return new AuthenticationResult
                {
                    Authenticated = true
                };
            }
            else
            {
                return new AuthenticationResult
                {
                    Authenticated = false,
                    ErrorMessage = "Authentication failed"
                };
            }
        }
    }
}
