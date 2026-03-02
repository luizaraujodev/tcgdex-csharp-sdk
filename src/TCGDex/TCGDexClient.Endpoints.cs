using TCGDex.Endpoints;
using TCGDex.Models;

namespace TCGDex;

/// <summary>
/// TCGDex client endpoint properties and random/helpers (partial).
/// </summary>
public partial class TCGDexClient
{
    private Endpoint<CardModel, CardResumeModel>? _cards;
    private Endpoint<SetModel, SetResumeModel>? _sets;
    private Endpoint<SerieModel, SerieResumeModel>? _series;
    private SimpleEndpoint? _type;
    private SimpleEndpoint? _retreat;
    private SimpleEndpoint? _rarity;
    private SimpleEndpoint? _illustrator;
    private SimpleEndpoint? _hp;
    private SimpleEndpoint? _categorie;
    private SimpleEndpoint? _dexId;
    private SimpleEndpoint? _energyType;
    private SimpleEndpoint? _regulationMark;
    private SimpleEndpoint? _stage;
    private SimpleEndpoint? _suffixe;
    private SimpleEndpoint? _trainerType;
    private SimpleEndpoint? _variant;

    /// <summary>Cards endpoint (get by id, list with optional query).</summary>
    public Endpoint<CardModel, CardResumeModel> Cards =>
        _cards ??= new Endpoint<CardModel, CardResumeModel>(this, "cards", s => new CardModel(s), s => new CardResumeModel(s));

    /// <summary>Sets endpoint (get by id, list with optional query).</summary>
    public Endpoint<SetModel, SetResumeModel> Sets =>
        _sets ??= new Endpoint<SetModel, SetResumeModel>(this, "sets", s => new SetModel(s), s => new SetResumeModel(s));

    /// <summary>Series endpoint (get by id, list with optional query).</summary>
    public Endpoint<SerieModel, SerieResumeModel> Series =>
        _series ??= new Endpoint<SerieModel, SerieResumeModel>(this, "series", s => new SerieModel(s), s => new SerieResumeModel(s));

    /// <summary>Types endpoint (Pokemon types).</summary>
    public SimpleEndpoint Type => _type ??= new SimpleEndpoint(this, "types");
    /// <summary>Retreat costs endpoint.</summary>
    public SimpleEndpoint Retreat => _retreat ??= new SimpleEndpoint(this, "retreats");
    /// <summary>Rarities endpoint.</summary>
    public SimpleEndpoint Rarity => _rarity ??= new SimpleEndpoint(this, "rarities");
    /// <summary>Illustrators endpoint.</summary>
    public SimpleEndpoint Illustrator => _illustrator ??= new SimpleEndpoint(this, "illustrators");
    /// <summary>HP values endpoint.</summary>
    public SimpleEndpoint Hp => _hp ??= new SimpleEndpoint(this, "hp");
    /// <summary>Categories endpoint (Pokemon, Trainer, Energy).</summary>
    public SimpleEndpoint Categorie => _categorie ??= new SimpleEndpoint(this, "categories");
    /// <summary>Dex IDs endpoint.</summary>
    public SimpleEndpoint DexId => _dexId ??= new SimpleEndpoint(this, "dex-ids");
    /// <summary>Energy types endpoint.</summary>
    public SimpleEndpoint EnergyType => _energyType ??= new SimpleEndpoint(this, "energy-types");
    /// <summary>Regulation marks endpoint.</summary>
    public SimpleEndpoint RegulationMark => _regulationMark ??= new SimpleEndpoint(this, "regulation-marks");
    /// <summary>Stages endpoint (Basic, Stage1, etc.).</summary>
    public SimpleEndpoint Stage => _stage ??= new SimpleEndpoint(this, "stages");
    /// <summary>Suffixes endpoint (EX, GX, V, etc.).</summary>
    public SimpleEndpoint Suffixe => _suffixe ??= new SimpleEndpoint(this, "suffixes");
    /// <summary>Trainer types endpoint.</summary>
    public SimpleEndpoint TrainerType => _trainerType ??= new SimpleEndpoint(this, "trainer-types");
    /// <summary>Variants endpoint.</summary>
    public SimpleEndpoint Variant => _variant ??= new SimpleEndpoint(this, "variants");

    /// <summary>Random card/set/serie helpers.</summary>
    public TCGDexRandom Random { get; }
}

/// <summary>
/// Random card, set, or serie from the API.
/// </summary>
public class TCGDexRandom
{
    private TCGDexClient? _client;

    internal TCGDexRandom(TCGDexClient client) => _client = client;

    private TCGDexClient Client => _client ?? throw new InvalidOperationException("Random is not bound to a client.");

    /// <summary>Fetches a random card.</summary>
    public async Task<CardModel?> GetRandomCardAsync(CancellationToken cancellationToken = default)
    {
        var path = new[] { "random", "card" };
        var json = await Client.FetchAsync(path, null, cancellationToken).ConfigureAwait(false);
        if (json == null) return null;
        var model = new CardModel(Client);
        ModelBase.Build(model, json.Value);
        return model;
    }

    /// <summary>Fetches a random set.</summary>
    public async Task<SetModel?> GetRandomSetAsync(CancellationToken cancellationToken = default)
    {
        var path = new[] { "random", "set" };
        var json = await Client.FetchAsync(path, null, cancellationToken).ConfigureAwait(false);
        if (json == null) return null;
        var model = new SetModel(Client);
        ModelBase.Build(model, json.Value);
        return model;
    }

    /// <summary>Fetches a random series.</summary>
    public async Task<SerieModel?> GetRandomSerieAsync(CancellationToken cancellationToken = default)
    {
        var path = new[] { "random", "serie" };
        var json = await Client.FetchAsync(path, null, cancellationToken).ConfigureAwait(false);
        if (json == null) return null;
        var model = new SerieModel(Client);
        ModelBase.Build(model, json.Value);
        return model;
    }
}
