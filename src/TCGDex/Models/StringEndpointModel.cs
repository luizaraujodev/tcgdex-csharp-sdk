using System.Text.Json;
using System.Text.Json.Serialization;
using TCGDex.Interfaces;

namespace TCGDex.Models;

/// <summary>
/// String-filter endpoint result (e.g. types, rarities): name + list of cards.
/// </summary>
public class StringEndpointModel : ModelBase, IStringEndpoint
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("cards")]
    public IReadOnlyList<CardResumeModel> Cards { get; set; } = [];

    IReadOnlyList<ICardResume> IStringEndpoint.Cards => Cards;

    public StringEndpointModel(TCGDexClient sdk) : base(sdk) { }

    protected override void Fill(JsonElement data)
    {
        if (data.TryGetProperty("name", out var j)) Name = j.GetString() ?? "";
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
    }
}
