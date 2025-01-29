namespace SampleApp.ViewModels;

public partial class HomePageViewModel : CustomPageViewModelBase, IPageLifecycleAware
{
    public HomePageViewModel(INavigationService navigationService) : base(navigationService)
    {
        IsLoggedIn = true;

        CurrentPageState = StateKey.Loading;
    }

    [ObservableProperty]
    private string _customerFirstName = string.Empty;

    [ObservableProperty]
    private bool _displayMedicationOverview;

    [ObservableProperty]
    private int _messagesCounter;

    [ObservableProperty]
    private int _notificationCounter;

    [ObservableProperty]
    private PermissionStatus _notificationsPermissionStatus = PermissionStatus.Granted;

    [ObservableProperty]
    private int _sumLowSupplyCount;

    [ObservableProperty]
    private int _sumNoSupplyCount;

    [ObservableProperty]
    private int _sumNumberOfScripts;

    [ObservableProperty]
    private int _sumOnOrderCount;

    [ObservableProperty]
    private int _todaysDoseReminderCount;

    public override void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);

        _ = Task.Run(async () => await RefreshPageCommand.ExecuteAsync(parameters).ConfigureAwait(false));
    }

    public override void OnNavigatedTo(INavigationParameters navigationParameters)
    {
        base.OnNavigatedTo(navigationParameters);
    }

    protected override async Task PullToRefreshPageAsync()
    {
        IsPageRefreshing = false;
        CurrentPageState = StateKey.Success;
    }

    protected override async Task RefreshPageAsync(INavigationParameters? navigationParameters)
    {
        IsPageRefreshing = false;
        CurrentPageState = StateKey.Success;
    }
}