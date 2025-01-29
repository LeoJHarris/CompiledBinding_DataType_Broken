namespace SampleApp.Resources.Styles.Methsof;

public partial class Colors : ResourceDictionary
{
    public Colors()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets the SharedInstance.
    /// </summary>
    public static Colors SharedInstance { get; } = [];
}