using System.Text.Json;
using TCGDex.Interfaces;

namespace TCGDex.Models;

/// <summary>
/// Full series model with sets list.
/// </summary>
public class SerieModel : SerieResumeModel, ISerie
{
    [System.Text.Json.Serialization.JsonPropertyName("sets")]
    public IReadOnlyList<SetResumeModel> Sets { get; set; } = [];

    IReadOnlyList<ISetResume> ISerie.Sets => Sets;

    public SerieModel(TCGDexClient sdk) : base(sdk) { }

    protected override void Fill(JsonElement data)
    {
        base.Fill(data);
        if (data.TryGetProperty("sets", out var setsEl) && setsEl.ValueKind == JsonValueKind.Array)
        {
            var list = new List<SetResumeModel>();
            foreach (var item in setsEl.EnumerateArray())
            {
                var m = new SetResumeModel(Sdk);
                ModelBase.Build(m, item);
                list.Add(m);
            }
            Sets = list;
        }
    }
}
