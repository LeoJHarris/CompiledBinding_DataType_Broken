namespace SampleApp.Models;

/// <summary>
/// Defines the <see cref="HomeTile"/>.
/// </summary>
public class HomeTile
{
    /// <summary>
    /// Gets or sets the Image.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the NotificationCounter.
    /// </summary>
    public int? NotificationCounter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether OpenUrlExt.
    /// </summary>
    public bool OpenUrlExt { get; set; }

    /// <summary>
    /// Gets or sets the Title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Uri.
    /// </summary>
    public Uri? Uri { get; set; }
}