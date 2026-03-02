using System.Text.Json;
using System.Text.Json.Serialization;
using TCGDex.Interfaces;

namespace TCGDex.Models;

/// <summary>
/// Brief card model (id, localId, name, image) with GetCardAsync and GetImageUrl.
/// </summary>
public class CardResumeModel : ModelBase, ICardResume
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("localId")]
    public string LocalId { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    public CardResumeModel(TCGDexClient sdk) : base(sdk) { }

    /// <summary>
    /// Returns the full image URL for the card with the given quality and extension.
    /// </summary>
    public string GetImageUrl(Quality quality = Quality.High, ImageExtension extension = ImageExtension.Png)
    {
        if (string.IsNullOrEmpty(Image)) return "";
        return $"{Image}/{quality.ToApiString()}.{extension.ToApiString()}";
    }

    /// <summary>
    /// Fetches the full card details from the API.
    /// </summary>
    public async Task<CardModel?> GetCardAsync(CancellationToken cancellationToken = default)
    {
        return await Sdk.Cards.GetAsync(Id, cancellationToken).ConfigureAwait(false);
    }

    protected override void Fill(JsonElement data)
    {
        if (data.TryGetProperty("id", out var j)) Id = j.GetString() ?? "";
        if (data.TryGetProperty("localId", out var j2)) LocalId = j2.GetString() ?? "";
        if (data.TryGetProperty("name", out var j3)) Name = j3.GetString() ?? "";
        if (data.TryGetProperty("image", out var j4)) Image = j4.GetString();
    }
}
