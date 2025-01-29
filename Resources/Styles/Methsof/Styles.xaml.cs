namespace SampleApp.Resources.Styles.Methsof;

public partial class Styles : ResourceDictionary
{
    public Styles()
    {
        InitializeComponent();
    }

    public static Styles SharedInstance { get; } = [];
}