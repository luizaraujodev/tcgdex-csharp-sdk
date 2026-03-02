using System.Text.Json;
using System.Text.Json.Serialization;
using TCGDex.Interfaces;

namespace TCGDex.Models;

/// <summary>
/// Brief series model with GetSerieAsync and GetImageUrl.
/// </summary>
public class SerieResumeModel : ModelBase, ISerieResume
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    public SerieResumeModel(TCGDexClient sdk) : base(sdk) { }

    /// <summary>
    /// Returns the full logo URL with the given extension.
    /// </summary>
    public string GetImageUrl(ImageExtension extension = ImageExtension.Png)
    {
        if (string.IsNullOrEmpty(Logo)) return "";
        return $"{Logo}.{extension.ToApiString()}";
    }

    /// <summary>
    /// Fetches the full series details from the API.
    /// </summary>
    public async Task<SerieModel?> GetSerieAsync(CancellationToken cancellationToken = default)
    {
        return await Sdk.Series.GetAsync(Id, cancellationToken).ConfigureAwait(false);
    }

    protected override void Fill(JsonElement data)
    {
        if (data.TryGetProperty("id", out var j)) Id = j.GetString() ?? "";
        if (data.TryGetProperty("name", out var j2)) Name = j2.GetString() ?? "";
        if (data.TryGetProperty("logo", out var j3)) Logo = j3.GetString();
    }
}
