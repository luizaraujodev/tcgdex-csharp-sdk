using System.Text.Json;
using System.Text.Json.Serialization;
using TCGDex.Interfaces;

namespace TCGDex.Models;

/// <summary>
/// Full card model with category, rarity, set, attacks, abilities, etc.
/// </summary>
public class CardModel : CardResumeModel, ICard
{
    [JsonPropertyName("illustrator")]
    public string? Illustrator { get; set; }
    [JsonPropertyName("rarity")]
    public string Rarity { get; set; } = "";
    [JsonPropertyName("category")]
    public string Category { get; set; } = "";
    [JsonPropertyName("variants")]
    public VariantsModel? Variants { get; set; }
    [JsonPropertyName("set")]
    public SetResumeModel Set { get; set; } = null!;
    [JsonPropertyName("dexId")]
    public IReadOnlyList<int>? DexId { get; set; }
    [JsonPropertyName("hp")]
    public int? Hp { get; set; }
    [JsonPropertyName("types")]
    public IReadOnlyList<string>? Types { get; set; }
    [JsonPropertyName("evolveFrom")]
    public string? EvolveFrom { get; set; }
    [JsonPropertyName("weight")]
    public string? Weight { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("level")]
    public JsonElement? LevelJson { get; set; }
    [JsonPropertyName("stage")]
    public string? Stage { get; set; }
    [JsonPropertyName("suffix")]
    public string? Suffix { get; set; }
    [JsonPropertyName("item")]
    public CardItemModel? Item { get; set; }
    [JsonPropertyName("abilities")]
    public IReadOnlyList<AbilityModel>? Abilities { get; set; }
    [JsonPropertyName("attacks")]
    public IReadOnlyList<AttackModel>? Attacks { get; set; }
    [JsonPropertyName("weaknesses")]
    public IReadOnlyList<WeaknessResistanceModel>? Weaknesses { get; set; }
    [JsonPropertyName("resistances")]
    public IReadOnlyList<WeaknessResistanceModel>? Resistances { get; set; }
    [JsonPropertyName("retreat")]
    public int? Retreat { get; set; }
    [JsonPropertyName("effect")]
    public string? Effect { get; set; }
    [JsonPropertyName("trainerType")]
    public string? TrainerType { get; set; }
    [JsonPropertyName("energyType")]
    public string? EnergyType { get; set; }
    [JsonPropertyName("regulationMark")]
    public string? RegulationMark { get; set; }
    [JsonPropertyName("legal")]
    public LegalModel Legal { get; set; } = null!;
    [JsonPropertyName("boosters")]
    public IReadOnlyList<BoosterModel>? Boosters { get; set; }

    object? ICard.Level => LevelJson switch { { } e when e.ValueKind == JsonValueKind.Number => e.TryGetInt32(out var i) ? i : e.GetDouble(), { } e => e.GetString(), _ => null };

    ISetResume ICard.Set => Set;
    IVariants? ICard.Variants => Variants;
    ICardItem? ICard.Item => Item;
    IReadOnlyList<IAbility>? ICard.Abilities => Abilities;
    IReadOnlyList<IAttack>? ICard.Attacks => Attacks;
    IReadOnlyList<IWeaknessResistance>? ICard.Weaknesses => Weaknesses;
    IReadOnlyList<IWeaknessResistance>? ICard.Resistances => Resistances;
    ILegal ICard.Legal => Legal;
    IReadOnlyList<IBooster>? ICard.Boosters => Boosters;

    public CardModel(TCGDexClient sdk) : base(sdk) { }

    /// <summary>
    /// Fetches the full set for this card.
    /// </summary>
    public async Task<SetModel?> GetSetAsync(CancellationToken cancellationToken = default)
    {
        return await Sdk.Sets.GetAsync(Set.Id, cancellationToken).ConfigureAwait(false);
    }

    protected override void Fill(JsonElement data)
    {
        base.Fill(data);
        if (data.TryGetProperty("illustrator", out var j)) Illustrator = j.GetString();
        if (data.TryGetProperty("rarity", out var j2)) Rarity = j2.GetString() ?? "";
        if (data.TryGetProperty("category", out var j3)) Category = j3.GetString() ?? "";
        if (data.TryGetProperty("variants", out var v)) Variants = JsonSerializer.Deserialize<VariantsModel>(v.GetRawText());
        if (data.TryGetProperty("set", out var setEl)) { Set = new SetResumeModel(Sdk); ModelBase.Build(Set, setEl); }
        if (data.TryGetProperty("dexId", out var dex)) DexId = JsonSerializer.Deserialize<List<int>>(dex.GetRawText());
        if (data.TryGetProperty("hp", out var hp)) Hp = hp.GetInt32();
        if (data.TryGetProperty("types", out var types)) Types = JsonSerializer.Deserialize<List<string>>(types.GetRawText());
        if (data.TryGetProperty("evolveFrom", out var ef)) EvolveFrom = ef.GetString();
        if (data.TryGetProperty("weight", out var w)) Weight = w.GetString();
        if (data.TryGetProperty("description", out var d)) Description = d.GetString();
        if (data.TryGetProperty("level", out var lvl)) LevelJson = lvl.Clone();
        if (data.TryGetProperty("stage", out var st)) Stage = st.GetString();
        if (data.TryGetProperty("suffix", out var suf)) Suffix = suf.GetString();
        if (data.TryGetProperty("item", out var itemEl)) Item = JsonSerializer.Deserialize<CardItemModel>(itemEl.GetRawText());
        if (data.TryGetProperty("abilities", out var ab)) Abilities = JsonSerializer.Deserialize<List<AbilityModel>>(ab.GetRawText());
        if (data.TryGetProperty("attacks", out var at)) Attacks = JsonSerializer.Deserialize<List<AttackModel>>(at.GetRawText());
        if (data.TryGetProperty("weaknesses", out var wk)) Weaknesses = JsonSerializer.Deserialize<List<WeaknessResistanceModel>>(wk.GetRawText());
        if (data.TryGetProperty("resistances", out var rs)) Resistances = JsonSerializer.Deserialize<List<WeaknessResistanceModel>>(rs.GetRawText());
        if (data.TryGetProperty("retreat", out var ret)) Retreat = ret.GetInt32();
        if (data.TryGetProperty("effect", out var eff)) Effect = eff.GetString();
        if (data.TryGetProperty("trainerType", out var tt)) TrainerType = tt.GetString();
        if (data.TryGetProperty("energyType", out var et)) EnergyType = et.GetString();
        if (data.TryGetProperty("regulationMark", out var rm)) RegulationMark = rm.GetString();
        if (data.TryGetProperty("legal", out var leg)) Legal = JsonSerializer.Deserialize<LegalModel>(leg.GetRawText()) ?? new LegalModel();
        if (data.TryGetProperty("boosters", out var boost)) Boosters = JsonSerializer.Deserialize<List<BoosterModel>>(boost.GetRawText());
    }
}

public class CardItemModel : ICardItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("effect")]
    public string Effect { get; set; } = "";
}

public class AbilityModel : IAbility
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("effect")]
    public string Effect { get; set; } = "";
}

public class AttackModel : IAttack
{
    [JsonPropertyName("cost")]
    public IReadOnlyList<string>? Cost { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("effect")]
    public string? Effect { get; set; }
    [JsonPropertyName("damage")]
    public JsonElement? DamageJson { get; set; }
    public object? Damage => DamageJson switch { { } e when e.ValueKind == JsonValueKind.Number => e.TryGetInt32(out var i) ? i : e.GetDouble(), { } e => e.GetString(), _ => null };
}

public class WeaknessResistanceModel : IWeaknessResistance
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
