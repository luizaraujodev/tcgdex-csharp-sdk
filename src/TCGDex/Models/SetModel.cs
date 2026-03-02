using System.Text.Json;
using TCGDex.Interfaces;

namespace TCGDex.Models;

/// <summary>
/// Full set model with serie, cards, legal, variants, etc.
/// </summary>
public class SetModel : SetResumeModel, ISet
{
    [System.Text.Json.Serialization.JsonPropertyName("serie")]
    public SerieResumeModel Serie { get; set; } = null!;

    [System.Text.Json.Serialization.JsonPropertyName("tcgOnline")]
    public string? TcgOnline { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("variants")]
    public VariantsModel? Variants { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("releaseDate")]
    public string ReleaseDate { get; set; } = "";

    [System.Text.Json.Serialization.JsonPropertyName("legal")]
    public LegalModel Legal { get; set; } = null!;

    [System.Text.Json.Serialization.JsonPropertyName("cards")]
    public IReadOnlyList<CardResumeModel> Cards { get; set; } = [];

    [System.Text.Json.Serialization.JsonPropertyName("boosters")]
    public IReadOnlyList<BoosterModel>? Boosters { get; set; }

    ISerieResume ISet.Serie => Serie;
    IVariants? ISet.Variants => Variants;
    ILegal ISet.Legal => Legal;
    IReadOnlyList<ICardResume> ISet.Cards => Cards;
    IReadOnlyList<IBooster>? ISet.Boosters => Boosters;

    public SetModel(TCGDexClient sdk) : base(sdk) { }

    /// <summary>
    /// Fetches the full series for this set.
    /// </summary>
    public async Task<SerieModel?> GetSerieAsync(CancellationToken cancellationToken = default)
    {
        return await Sdk.Series.GetAsync(Serie.Id, cancellationToken).ConfigureAwait(false);
    }

    protected override void Fill(JsonElement data)
    {
        base.Fill(data);
        if (data.TryGetProperty("serie", out var serieEl))
        {
            Serie = new SerieResumeModel(Sdk);
            ModelBase.Build(Serie, serieEl);
        }
        if (data.TryGetProperty("tcgOnline", out var to)) TcgOnline = to.GetString();
        if (data.TryGetProperty("variants", out var v)) Variants = JsonSerializer.Deserialize<VariantsModel>(v.GetRawText());
        if (data.TryGetProperty("releaseDate", out var rd)) ReleaseDate = rd.GetString() ?? "";
        if (data.TryGetProperty("legal", out var leg)) Legal = JsonSerializer.Deserialize<LegalModel>(leg.GetRawText()) ?? new LegalModel();
        if (data.TryGetProperty("cards", out var cardsEl) && cardsEl.ValueKind == JsonValueKind.Array)
        {
            var list = new List<CardResumeModel>();
            foreach (var item in cardsEl.EnumerateArray())
            {
                var m = new CardResumeModel(Sdk);
                ModelBase.Build(m, item);
                list.Add(m);
            }
            Cards = list;
        }
        if (data.TryGetProperty("boosters", out var boostEl)) Boosters = JsonSerializer.Deserialize<List<BoosterModel>>(boostEl.GetRawText());
    }
}

public class VariantsModel : IVariants
{
    [System.Text.Json.Serialization.JsonPropertyName("normal")]
    public bool? Normal { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("reverse")]
    public bool? Reverse { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("holo")]
    public bool? Holo { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("firstEdition")]
    public bool? FirstEdition { get; set; }
}

public class LegalModel : ILegal
{
    [System.Text.Json.Serialization.JsonPropertyName("standard")]
    public bool Standard { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("expanded")]
    public bool Expanded { get; set; }
}

public class BoosterModel : IBooster
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public string Id { get; set; } = "";
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [System.Text.Json.Serialization.JsonPropertyName("logo")]
    public string? Logo { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("artwork_front")]
    public string? ArtworkFront { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("artwork_back")]
    public string? ArtworkBack { get; set; }
}
