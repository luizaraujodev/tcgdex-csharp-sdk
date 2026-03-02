namespace TCGDex;

/// <summary>
/// Image file extension for card/set images.
/// </summary>
public enum ImageExtension
{
    Jpg,
    Webp,
    Png
}

/// <summary>
/// Extension methods for ImageExtension to get the file extension string.
/// </summary>
public static class ImageExtensionExtensions
{
    public static string ToApiString(this ImageExtension ext) => ext switch
    {
        ImageExtension.Jpg => "jpg",
        ImageExtension.Webp => "webp",
        ImageExtension.Png => "png",
        _ => "png"
    };
}
