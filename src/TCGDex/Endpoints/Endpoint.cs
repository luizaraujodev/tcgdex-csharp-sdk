using TCGDex.Models;

namespace TCGDex.Endpoints;

/// <summary>
/// Generic endpoint for entities with full and resume models (cards, sets, series).
/// </summary>
/// <typeparam name="TItem">Full model type (e.g. CardModel, SetModel, SerieModel).</typeparam>
/// <typeparam name="TList">Resume model type (e.g. CardResumeModel, SetResumeModel, SerieResumeModel).</typeparam>
public class Endpoint<TItem, TList>
    where TItem : ModelBase
    where TList : ModelBase
{
    private readonly TCGDexClient _client;
    private readonly string _endpoint;
    private readonly Func<TCGDexClient, TItem> _createItem;
    private readonly Func<TCGDexClient, TList> _createList;

    internal Endpoint(
        TCGDexClient client,
        string endpoint,
        Func<TCGDexClient, TItem> createItem,
        Func<TCGDexClient, TList> createList)
    {
        _client = client;
        _endpoint = endpoint;
        _createItem = createItem;
        _createList = createList;
    }

    /// <summary>
    /// Gets a single item by id.
    /// </summary>
    public async Task<TItem?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var path = new[] { _endpoint, id };
        var json = await _client.FetchAsync(path, null, cancellationToken).ConfigureAwait(false);
        if (json == null) return null;
        var model = _createItem(_client);
        ModelBase.Build(model, json.Value);
        return model;
    }

    /// <summary>
    /// Lists items, optionally with query filters.
    /// </summary>
    public async Task<IReadOnlyList<TList>> ListAsync(Query? query = null, CancellationToken cancellationToken = default)
    {
        var path = new[] { _endpoint };
        var q = query?.Params ?? (IReadOnlyList<(string Key, object Value)>?)[];
        var json = await _client.FetchAsync(path, q, cancellationToken).ConfigureAwait(false);
        if (json == null || json.Value.ValueKind != System.Text.Json.JsonValueKind.Array)
            return [];
        var list = new List<TList>();
        foreach (var item in json.Value.EnumerateArray())
        {
            var model = _createList(_client);
            ModelBase.Build(model, item);
            list.Add(model);
        }
        return list;
    }
}
