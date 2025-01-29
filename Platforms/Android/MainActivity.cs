using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Permission = Android.Content.PM.Permission;

namespace SampleApp;
 
[Activity(
    Theme = "@style/Maui.SplashTheme",
    LaunchMode = LaunchMode.SingleTop,
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private long backPressed;

    public override void OnBackPressed()
    {
        bool? result = false;

        _ = (App.ContainerProvider?.GetService<IDispatcher>()?.Dispatch(action: async () => result = await App.CallHardwareBackPressedAsync().ConfigureAwait(true)));

        // If back button does not have a custom back pressed func, call default.
        if (result.Value && Looper.MainLooper is Looper looper)
        {
            if (App.PromptToConfirmExit)
            {
                const int delay = 2000;
                if (backPressed + delay > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                {
                    FinishAndRemoveTask();
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                }
                else
                {
                    Android.Widget.Toast.MakeText(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity, "Press back again to close the app", ToastLength.Short)?.Show();
                    backPressed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        Microsoft.Maui.ApplicationModel.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    public override void OnUserInteraction()
    {
        base.OnUserInteraction();
    }

    protected override void OnCreate(Bundle? bundle)
    {
        base.OnCreate(bundle);

        Microsoft.Maui.ApplicationModel.Platform.Init(this, bundle);
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);
        Microsoft.Maui.ApplicationModel.Platform.OnNewIntent(intent);
    }

    protected override void OnPause()
    {
        base.OnPause();

        Window?.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);
    }

    protected override void OnResume()
    {
        base.OnResume();

        Window?.ClearFlags(WindowManagerFlags.Secure);
    }
}