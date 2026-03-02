using TCGDex.Models;

namespace TCGDex.Endpoints;

/// <summary>
/// Endpoint for string-filter resources (types, rarities, etc.) that return name + cards.
/// </summary>
public class SimpleEndpoint
{
    private readonly TCGDexClient _client;
    private readonly string _endpoint;

    internal SimpleEndpoint(TCGDexClient client, string endpoint)
    {
        _client = client;
        _endpoint = endpoint;
    }

    /// <summary>
    /// Gets a single string-endpoint by id (e.g. a specific type or rarity).
    /// </summary>
    public async Task<StringEndpointModel?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var path = new[] { _endpoint, id };
        var json = await _client.FetchAsync(path, null, cancellationToken).ConfigureAwait(false);
        if (json == null) return null;
        var model = new StringEndpointModel(_client);
        ModelBase.Build(model, json.Value);
        return model;
    }

    /// <summary>
    /// Lists all values for this endpoint (e.g. all types, all rarities).
    /// </summary>
    public async Task<IReadOnlyList<StringEndpointModel>> ListAsync(Query? query = null, CancellationToken cancellationToken = default)
    {
        var path = new[] { _endpoint };
        var q = query?.Params ?? (IReadOnlyList<(string Key, object Value)>?)[];
        var json = await _client.FetchAsync(path, q, cancellationToken).ConfigureAwait(false);
        if (json == null || json.Value.ValueKind != System.Text.Json.JsonValueKind.Array)
            return [];
        var list = new List<StringEndpointModel>();
        foreach (var item in json.Value.EnumerateArray())
        {
            var model = new StringEndpointModel(_client);
            ModelBase.Build(model, item);
            list.Add(model);
        }
        return list;
    }
}
