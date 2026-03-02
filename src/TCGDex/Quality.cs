namespace TCGDex;

/// <summary>
/// Image quality for card/set images.
/// </summary>
public enum Quality
{
    Low,
    High
}

/// <summary>
/// Extension methods for Quality to get the API string value.
/// </summary>
public static class QualityExtensions
{
    public static string ToApiString(this Quality quality) => quality == Quality.Low ? "low" : "high";
}
