namespace SampleApp.Infrastructure;

public interface IHasView
{
    CollectionView? DiaryTab_DaysCollectionView { get; }
    CollectionView? DiaryTab_MonthsCollectionView { get; }
    CollectionView? DiaryTab_PatientCollectionView { get; }
    Editor? DispensedScriptDetailPage_OrderNotesEditor { get; }

    WebView? GenericWebView { get; }
    CollectionView? MessagesCollectionView { get; }
    CollectionView? Schedule_PatientCollectionView { get; }
    Editor? ScriptNotesEditor { get; }
    CarouselView? ShoppingCarouselView { get; }
    WebView? ShoppingWebView { get; }
    SearchBar? StoreLocatorMapTab_StoresSearchBar { get; }
}