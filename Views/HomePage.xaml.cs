namespace SampleApp.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    public HomePage()
    {
        InitializeComponent();
    }
}