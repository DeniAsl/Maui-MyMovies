using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Biometric;
using AndroidX.Fragment.App;
using Java.Lang;
using Java.Util.Concurrent;
using MyMovies.Core.Models;
using MyMovies.Core.Interfaces;
using Application = Android.App.Application;

namespace MyMovies.Platforms.Services
{
    public class NativeAuthentication : INativeAuthentication
    {
        private readonly BiometricManager manager;

        public NativeAuthentication()
        {
            manager = BiometricManager.From(Application.Context);
        }

        public bool IsSupported()
        {
            //check if android version is at least 28
            if (Build.VERSION.SdkInt < BuildVersionCodes.P)
                return false;

            var context = Application.Context;

            //check if we have the necessary permissions
            if (context.CheckCallingOrSelfPermission(Manifest.Permission.UseBiometric) != Permission.Granted)
                return false;

            //check if authentication is supported on this device
            int result = manager.CanAuthenticate(BiometricManager.Authenticators.DeviceCredential | BiometricManager.Authenticators.BiometricWeak);
            return result == 0;
        }

        public async Task<AuthenticationResult> PromptLoginAsync(string prompt)
        {
            var taskCancellationSource = new CancellationTokenSource();

            //this object signals completion of the Task, it is used in the AuthenticationHandler
            var taskCompletionSource = new TaskCompletionSource<AuthenticationResult>();

            //initialize manager and handler
            var manager = BiometricManager.From(Application.Context);
            var handler = new AuthenticationHandler(taskCompletionSource);

            //build the prompt
            var builder = new BiometricPrompt.PromptInfo.Builder()
                .SetTitle("Verify it's you")
                .SetConfirmationRequired(true)
                .SetDescription(prompt);

            builder = builder.SetNegativeButtonText("Cancel");

            var info = builder.Build();

            //get the native Android activity which hosts our MAUI app
            var activity = (FragmentActivity)Platform.CurrentActivity;
            var executor = Executors.NewSingleThreadExecutor();

            //show the prompt
            using var dialog = new BiometricPrompt(activity, executor, handler);
            await using (var taskCancellation = taskCancellationSource.Token.Register(dialog.CancelAuthentication))
            {
                dialog.Authenticate(info);
            }

            return await taskCompletionSource.Task;
        }

        public class AuthenticationHandler : BiometricPrompt.AuthenticationCallback, IDialogInterfaceOnClickListener
        {
            private readonly TaskCompletionSource<AuthenticationResult> taskCompletionSource;

            public AuthenticationHandler(TaskCompletionSource<AuthenticationResult> taskCompletionSource)
            {
                this.taskCompletionSource = taskCompletionSource;
            }

            public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
            {
                base.OnAuthenticationSucceeded(result);
                SetResult(true, null);
            }

            public override void OnAuthenticationError(int errorCode, ICharSequence errorMessage)
            {
                base.OnAuthenticationError(errorCode, errorMessage);
                SetResult(false, errorMessage.ToString());
            }

            public override void OnAuthenticationFailed()
            {
                base.OnAuthenticationFailed();
                SetResult(false, "Authentication failed. Please try again.");
            }

            public void OnClick(IDialogInterface dialog, int which)
            {
                SetResult(false, "Authentication cancelled");
            }

            private void SetResult(bool success, string errorMessage)
            {
                if (!(taskCompletionSource.Task.IsCanceled || taskCompletionSource.Task.IsCompleted || taskCompletionSource.Task.IsFaulted))
                {
                    //signal completetion of the Task
                    taskCompletionSource.SetResult(new AuthenticationResult
                    {
                        Authenticated = success,
                        ErrorMessage = errorMessage
                    });
                }
            }
        }
    }
}
