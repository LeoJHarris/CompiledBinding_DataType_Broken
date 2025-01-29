[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SampleApp;

public partial class App : Application
{
    public App(Lazy<IVersionTracking> lazyVersionTracking)
    {
        InitializeComponent();

        Initializer.Initialize(loggerEnable: false, false);

        lazyVersionTracking.Value.Track();

        AppDictionary.MergedDictionaries.Add(new Resources.Styles.Methsof.Colors());
        AppDictionary.MergedDictionaries.Add(new Resources.Styles.Methsof.Styles());
    }

    public static AvailabilityResult? AvailabilityResult { get; set; }

    public static IServiceProvider? ContainerProvider => IPlatformApplication.Current?.Services;

    /// <summary>
    /// Custom hardware pressed func, return true if default back button action should invoked
    /// </summary>
    public static Func<Task<bool?>>? HardwareBackPressed { private get; set; }

    public static bool HasNetworkAccess => ContainerProvider.GetService<IConnectivity>()?.NetworkAccess is NetworkAccess.Internet;

    public static bool IsCurrentPageLandingTabbedPage => Current?.Windows[0].Page is not null;

    /// <summary>
    /// Is the customer signed in for the current session.
    /// </summary>
    public static bool IsCustomerSignedIn { get; set; }

    public static bool PromptToConfirmExit
    {
        get
        {
            Page? currentPage = Current?.Windows[0].Page;

            if (currentPage?.Navigation.ModalStack.Count > 0)
            {
                return false;
            }

            if (currentPage is ContentPage)
            {
                return true;
            }

            if (currentPage is NavigationPage mainPage)
            {
                return mainPage.CurrentPage is NavigationPage { Navigation.NavigationStack.Count: <= 1 };
            }

            if (currentPage is TabbedPage tabbedPage)
            {
                NavigationPage? navigationPage = tabbedPage.CurrentPage as NavigationPage;
                return navigationPage?.Navigation.NavigationStack.Count <= 1;
            }

            return false;
        }
    }

    private const string _beginShoppingAppAction = "begin_shopping";
    private const string _sendMessageAppAction = "send_message";
    private SubscriptionToken? _appInactivityTimerSubscriptionToken;

    public static async Task<bool?> CallHardwareBackPressedAsync()
    {
        Func<Task<bool?>>? backPressed = HardwareBackPressed;

        return backPressed is not null ? await backPressed().ConfigureAwait(true) : true;
    }

    public static Page? GetCurrentPage(Page? page)
    {
        if (page is not null)
        {
            switch (page)
            {
                case TabbedPage:
                    return GetCurrentPage((page as TabbedPage)?.CurrentPage);

                case NavigationPage:
                    NavigationPage? navigationPage = page as NavigationPage;
                    return navigationPage?.RootPage is TabbedPage tabbedPage ? GetCurrentPage(tabbedPage.CurrentPage) : navigationPage?.CurrentPage;

                default: return page;
            }
        }

        return page;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Activated += async (s, e) =>
        {
            if (ContainerProvider?.GetService<IBiometricAuthentication>() is IBiometricAuthentication biometricAuthentication)
            {
                AvailabilityResult = await biometricAuthentication.CheckAvailabilityAsync().ConfigureAwait(true);
            }
        };

        return window;
    }

    protected override void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);
    }

    protected override void OnResume() =>

        base.OnResume();

    protected override void OnSleep()
    {
        base.OnSleep();
    }
}