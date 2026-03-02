using System.Text.Json;
using System.Text.Json.Serialization;
using TCGDex.Interfaces;

namespace TCGDex.Models;

/// <summary>
/// Brief set model with GetSetAsync.
/// </summary>
public class SetResumeModel : ModelBase, ISetResume
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("cardCount")]
    public SetCardCountModel CardCount { get; set; } = new();

    ISetCardCount ISetResume.CardCount => CardCount;

    public SetResumeModel(TCGDexClient sdk) : base(sdk) { }

    /// <summary>
    /// Fetches the full set details from the API.
    /// </summary>
    public async Task<SetModel?> GetSetAsync(CancellationToken cancellationToken = default)
    {
        return await Sdk.Sets.GetAsync(Id, cancellationToken).ConfigureAwait(false);
    }

    protected override void Fill(JsonElement data)
    {
        if (data.TryGetProperty("id", out var j)) Id = j.GetString() ?? "";
        if (data.TryGetProperty("name", out var j2)) Name = j2.GetString() ?? "";
        if (data.TryGetProperty("logo", out var j3)) Logo = j3.GetString();
        if (data.TryGetProperty("symbol", out var j4)) Symbol = j4.GetString();
        if (data.TryGetProperty("cardCount", out var cc))
        {
            CardCount = new SetCardCountModel();
            if (cc.TryGetProperty("total", out var t)) CardCount.Total = t.GetInt32();
            if (cc.TryGetProperty("official", out var o)) CardCount.Official = o.GetInt32();
            if (cc.TryGetProperty("normal", out var n)) CardCount.Normal = n.GetInt32();
            if (cc.TryGetProperty("reverse", out var r)) CardCount.Reverse = r.GetInt32();
            if (cc.TryGetProperty("holo", out var h)) CardCount.Holo = h.GetInt32();
            if (cc.TryGetProperty("firstEd", out var fe)) CardCount.FirstEd = fe.GetInt32();
        }
    }
}

public class SetCardCountModel : ISetCardCount
{
    public int Total { get; set; }
    public int Official { get; set; }
    public int? Normal { get; set; }
    public int? Reverse { get; set; }
    public int? Holo { get; set; }
    public int? FirstEd { get; set; }
}
