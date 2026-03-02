namespace TCGDex;

/// <summary>
/// Supported API languages for TCGDex.
/// </summary>
public enum SupportedLanguages
{
    En,
    Fr,
    Es,
    It,
    Pt,
    De
}

/// <summary>
/// Extension methods for SupportedLanguages to get the API string value.
/// </summary>
public static class SupportedLanguagesExtensions
{
    private static readonly string[] Values = ["en", "fr", "es", "it", "pt", "de"];

    public static string ToApiString(this SupportedLanguages lang)
    {
        var i = (int)lang;
        return i >= 0 && i < Values.Length ? Values[i] : "en";
    }

    public static SupportedLanguages FromApiString(string? value)
    {
        return value?.ToLowerInvariant() switch
        {
            "en" => SupportedLanguages.En,
            "fr" => SupportedLanguages.Fr,
            "es" => SupportedLanguages.Es,
            "it" => SupportedLanguages.It,
            "pt" => SupportedLanguages.Pt,
            "de" => SupportedLanguages.De,
            _ => SupportedLanguages.En
        };
    }
}

/// <summary>
/// Valid TCGDex API endpoint names.
/// </summary>
public static class ApiEndpoints
{
    public static readonly IReadOnlyList<string> All = new[]
    {
        "cards",
        "categories",
        "dex-ids",
        "energy-types",
        "hp",
        "illustrators",
        "rarities",
        "regulation-marks",
        "retreats",
        "series",
        "sets",
        "stages",
        "suffixes",
        "trainer-types",
        "types",
        "variants",
        "random"
    };

    public static bool IsValid(string endpoint)
    {
        return All.Contains(endpoint, StringComparer.OrdinalIgnoreCase);
    }
}
