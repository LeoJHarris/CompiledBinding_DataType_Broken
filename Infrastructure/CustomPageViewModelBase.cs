namespace SampleApp.Infrastructure;

public abstract partial class CustomPageViewModelBase(INavigationService navigationService) : ObservableObject, IInitialize, INavigationAware, IDestructible
{
    public INavigationService NavigationService { get; } = navigationService;

    private const string _rootUriPrependText = "/";

    [ObservableProperty]
    private string _currentPageState = string.Empty;

    [ObservableProperty]
    private string _exceptionMessage = string.Empty;

    [ObservableProperty]
    private bool _hasInternetAccess = true;

    [ObservableProperty]
    private bool _isEntryNoDotValidatorBehaviorAttached;

    [ObservableProperty]
    private bool _isLoggedIn = true;

    [ObservableProperty]
    private bool _isPageRefreshing;

    private SubscriptionToken? _loginChangedPubSubEventSubscriptionToken;

    public virtual void Destroy()
    {
    }

    public virtual void Initialize(INavigationParameters parameters)
    {
        HasInternetAccess = App.HasNetworkAccess;
    }

    public virtual void OnAppearing()
    {
        IsEntryNoDotValidatorBehaviorAttached = true;
    }

    public virtual void OnDisappearing() => IsEntryNoDotValidatorBehaviorAttached = false;

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public virtual void OnNavigatedTo(INavigationParameters parameters)
    {
    }

    protected virtual bool CanNavigateCommandExecute(string? uri) => !string.IsNullOrEmpty(uri);

    [RelayCommand(CanExecute = nameof(CanNavigateCommandExecute))]
    protected virtual async Task NavigateAbsoluteAsync(string? path)
    {
        if (!string.IsNullOrEmpty(path) && !path.StartsWith(_rootUriPrependText))
        {
            path = string.Concat(_rootUriPrependText, path);
        }

        INavigationResult navigationResult = await NavigationService.NavigateAsync(path).ConfigureAwait(false);

        if (!navigationResult.Success && navigationResult.Exception is not null)
        {
        }
    }

    [RelayCommand(CanExecute = nameof(CanNavigateCommandExecute))]
    protected virtual async Task NavigateAsync(string? path)
    {
        INavigationResult navigationResult = await NavigationService.NavigateAsync(path).ConfigureAwait(false);

        if (!navigationResult.Success && navigationResult.Exception is not null)
        {
        }
    }

    [RelayCommand(CanExecute = nameof(CanNavigateCommandExecute))]
    protected virtual async Task NavigateModalAsync(string? path)
    {
        INavigationResult navigationResult = await NavigationService.NavigateAsync(path, parameters: new NavigationParameters
        {
            { KnownNavigationParameters.UseModalNavigation, true }
        }).ConfigureAwait(false);

        if (!navigationResult.Success && navigationResult.Exception is not null)
        {
        }
    }

    [RelayCommand]
    protected virtual async Task NavigateToRootAsync()
    {
        INavigationResult navigationResult = await NavigationService.GoBackToRootAsync().ConfigureAwait(false);

        if (!navigationResult.Success && navigationResult.Exception is not null)
        {
        }
    }

    [RelayCommand(CanExecute = nameof(CanNavigateCommandExecute))]
    protected async Task OpenExtBrowserAsync(string? url)
    {
        if (!string.IsNullOrEmpty(url))
        {
        }
    }

    [RelayCommand]
    protected abstract Task PullToRefreshPageAsync();

    [RelayCommand]
    protected abstract Task RefreshPageAsync(INavigationParameters navigationParameters);

    [RelayCommand]
    protected virtual void UpdatePageStateOnNetworkAccess(NetworkAccess networkAccess)
    {
        HasInternetAccess = networkAccess is NetworkAccess.Internet;

        if (HasInternetAccess && CurrentPageState is not StateKey.Loading)
        {
            // If the page did have connection again and page was not loading, StateKey.Loading
            // prevents multiple refresh while in loading state
            RefreshPageCommand?.Execute(default);
        }
    }

    [RelayCommand]
    private Task NavigateBackAsync() => NavigationService.GoBackAsync();
}