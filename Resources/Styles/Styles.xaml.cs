namespace SampleApp.Resources.Styles;

public partial class Styles : ResourceDictionary
{
    public Styles()
    {
        InitializeComponent();
    }

    public static Styles SharedInstance { get; } = [];
}