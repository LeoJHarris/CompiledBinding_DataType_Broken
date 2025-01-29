namespace SampleApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        MauiAppBuilder prismAppBuilder = builder
            .UseMauiApp<App>()
#if DEBUG
            .UseMauiCommunityToolkit(options => options.SetShouldEnableSnackbarOnWindows(true))
#else
                                .UseMauiCommunityToolkit(options =>
                                {
                                    options.SetShouldEnableSnackbarOnWindows(true);
                                    options.SetShouldSuppressExceptionsInConverters(true);
                                    options.SetShouldSuppressExceptionsInBehaviors(true);
                                    options.SetShouldSuppressExceptionsInAnimations(true);
                                })
#endif
            .UseMauiCommunityToolkitMediaElement()
            .UseBiometricAuthentication()
            .UseSharpnadoTabs(loggerEnable: false)
            .UseAndroidInAppUpdates()
            .RegisterFirebase()
            .ConfigureFonts(fonts =>
            {
                _ = fonts.AddFont("outfitlight.ttf", "LightFont");
                _ = fonts.AddFont("outfitbold.ttf", "HeavyFont");
                _ = fonts.AddFont("outfitregular.ttf", "RegularFont");
                _ = fonts.AddFont("materialdesignicons.ttf", "MaterialDesignIcons");
                _ = fonts.AddFont("transpiredesignicons.ttf", "TranspireDesignIcons");
            })
            .ConfigureEssentials(essentials => essentials.UseVersionTracking())
            .UseBarcodeReader()
            .UsePrism(prismAppBuilder =>
            {
                prismAppBuilder.RegisterTypes(containerRegistry =>
                {
                    registerPages(containerRegistry);
                    registerCustomServices(containerRegistry);
                    registerMauiCoreServices(containerRegistry);
                    registerEssentials(containerRegistry);
                })
                .AddGlobalNavigationObserver(context => context.Subscribe(navigationRequestContext =>
                {
                    Exception? exception = navigationRequestContext.Result.Exception;

                    if (exception is not null)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }))
                .CreateWindow(async (containerProvider, navigationService) =>
                {
                    await navigationService.NavigateAsync($"/{PageNames.LandingTabbedPage}").ConfigureAwait(false);
                });
            })
            .ConfigureMauiHandlers(registerHandlerMappers);

        return builder.Build();
    }

    private static void registerHandlerMappers(IMauiHandlersCollection handlers)
    {
        LabelHandler.Mapper.AppendToMapping(nameof(Label.FormattedText), (handler, view) =>
        {
            if (view is not Label label || label.FormattedText == null)
            {
                return;
            }

            foreach (Span? span in label.FormattedText.Spans)
            {
                span.Style ??= label.Style;
                span.TextColor ??= label.TextColor;
            }
        });

        EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        DatePickerHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        TimePickerHandler.Mapper.AppendToMapping(nameof(TimePicker), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);

#endif
        });

        EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        SearchBarHandler.Mapper.AppendToMapping(nameof(SearchBar), (handler, view) =>
        {
#if ANDROID
            Android.Widget.LinearLayout? linearLayout = handler.PlatformView.GetChildAt(0) as Android.Widget.LinearLayout;

            if (linearLayout is not null)
            {
                linearLayout = linearLayout.GetChildAt(2) as Android.Widget.LinearLayout;
                if (linearLayout is not null)
                {
                    linearLayout = linearLayout.GetChildAt(1) as Android.Widget.LinearLayout; if (linearLayout is not null)
                    {
                        linearLayout.Background = null;
                    }
                }
            }

#endif
        });
    }

    private static void registerCustomServices(in IContainerRegistry containerRegistry)
    {
        _ = containerRegistry.RegisterSingleton<IBiometricAuthentication>(ctx => BiometricAuthentication.Current);
        _ = containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
    }

    private static void registerEssentials(in IContainerRegistry containerRegistry)
    {
        _ = containerRegistry.RegisterSingleton<IDeviceDisplay>(ctx => DeviceDisplay.Current);
        _ = containerRegistry.RegisterSingleton<IDeviceInfo>(ctx => DeviceInfo.Current);
        _ = containerRegistry.RegisterSingleton<IFileSaver>(ctx => FileSaver.Default);
        _ = containerRegistry.RegisterSingleton<IFileSystem>(ctx => FileSystem.Current);
        _ = containerRegistry.RegisterSingleton<IFolderPicker>(ctx => FolderPicker.Default);
        _ = containerRegistry.RegisterSingleton<IBadge>(ctx => Badge.Default);
        _ = containerRegistry.RegisterSingleton<ITextToSpeech>(ctx => TextToSpeech.Default);
        _ = containerRegistry.RegisterSingleton<IAppInfo>(ctx => AppInfo.Current);
        _ = containerRegistry.RegisterSingleton<IPreferences>(ctx => Preferences.Default);
        _ = containerRegistry.RegisterSingleton<ISecureStorage>(ctx => SecureStorage.Default);
        _ = containerRegistry.RegisterSingleton<IVersionTracking>(ctx => VersionTracking.Default);
        _ = containerRegistry.RegisterSingleton<IConnectivity>(ctx => Connectivity.Current);
        _ = containerRegistry.RegisterSingleton<IGeolocation>(ctx => Geolocation.Default);
        _ = containerRegistry.RegisterSingleton<IShare>(ctx => Share.Default);
        _ = containerRegistry.RegisterSingleton<IMediaPicker>(ctx => MediaPicker.Default);
        _ = containerRegistry.RegisterSingleton<IVibration>(ctx => Vibration.Default);
    }

    private static void registerMauiCoreServices(in IContainerRegistry containerRegistry)
    {
        _ = containerRegistry.RegisterSingleton<Microsoft.Maui.ApplicationModel.IMap>(ctx => Microsoft.Maui.ApplicationModel.Map.Default);
        _ = containerRegistry.RegisterSingleton<IBrowser>(ctx => Browser.Default);
    }

    private static void registerPages(in IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
        containerRegistry.RegisterForNavigation<LandingTabbedPage, LandingTabbedPageViewModel>();
    }

    private static MauiAppBuilder RegisterFirebase(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events => events.AddAndroid(android => android.OnCreate((activity, _) => Firebase.FirebaseApp.InitializeApp(activity))));
        return builder;
    }
}