using System.Net.Http.Headers;
using System.Text.Json;

namespace TCGDex;

/// <summary>
/// Main client for the TCGDex API (api.tcgdex.net/v2).
/// </summary>
public partial class TCGDexClient
{
    private const string DefaultBaseUrl = "https://api.tcgdex.net/v2/";
    private const string UserAgent = "TCGDex-NET/0.1.0";

    private string _endpointUrl = DefaultBaseUrl.TrimEnd('/');
    private SupportedLanguages _lang = SupportedLanguages.En;
    private readonly HttpClient _httpClient;
    private ITCGDexCache _cache;
    private int _cacheTTL = 60 * 60;

    /// <summary>
    /// Current language for API requests.
    /// </summary>
    public SupportedLanguages Lang
    {
        get => _lang;
        set => _lang = value;
    }

    /// <summary>
    /// Base URL for the API (e.g. https://api.tcgdex.net/v2).
    /// </summary>
    public string EndpointUrl
    {
        get => _endpointUrl;
        set => _endpointUrl = value?.TrimEnd('/') ?? DefaultBaseUrl.TrimEnd('/');
    }

    /// <summary>
    /// Cache TTL in seconds for successful responses.
    /// </summary>
    public int CacheTTL
    {
        get => _cacheTTL;
        set => _cacheTTL = value;
    }

    /// <summary>
    /// Cache implementation (get/set by URL key).
    /// </summary>
    public ITCGDexCache Cache
    {
        get => _cache;
        set => _cache = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Creates a new TCGDex client with the given language and optional HTTP client and cache.
    /// </summary>
    public TCGDexClient(
        SupportedLanguages lang = SupportedLanguages.En,
        HttpClient? httpClient = null,
        ITCGDexCache? cache = null)
    {
        _lang = lang;
        _httpClient = httpClient ?? new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        _cache = cache ?? new MemoryTCGDexCache();
        Random = new TCGDexRandom(this);
    }

    /// <summary>
    /// Builds the full URL for a path and optional query parameters.
    /// </summary>
    internal string GetFullUrl(IReadOnlyList<string> path, IReadOnlyList<(string Key, object Value)>? queryParams = null)
    {
        var lang = _lang.ToApiString();
        var pathStr = string.Join("/", path.Prepend(lang));
        var url = $"{_endpointUrl}/{pathStr}";
        if (queryParams is { Count: > 0 })
        {
            var query = string.Join("&", queryParams.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString() ?? "")}"));
            url += "?" + query;
        }
        return url;
    }

    /// <summary>
    /// Fetches raw JSON from the API (with cache). Returns null on 404; throws on 5xx.
    /// </summary>
    internal async Task<JsonElement?> FetchAsync(
        IReadOnlyList<string> path,
        IReadOnlyList<(string Key, object Value)>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        if (path == null || path.Count == 0)
            throw new ArgumentException("Path cannot be empty.", nameof(path));

        var baseEndpoint = path[0].ToLowerInvariant();
        if (!ApiEndpoints.IsValid(baseEndpoint))
            throw new ArgumentException($"Unknown endpoint: {baseEndpoint}.", nameof(path));

        var url = GetFullUrl(path, queryParams);

        var cached = _cache.Get(url);
        if (cached is string jsonCached)
        {
            using var doc = JsonDocument.Parse(jsonCached);
            return doc.RootElement.Clone();
        }

        var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
        {
            try
            {
                var errBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new InvalidOperationException($"TCGDex server error: {errBody}");
            }
            catch (InvalidOperationException) { throw; }
            catch
            {
                throw new InvalidOperationException("TCGDex server responded with an invalid error.");
            }
        }

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        _cache.Set(url, json, _cacheTTL);
        using var doc2 = JsonDocument.Parse(json);
        return doc2.RootElement.Clone();
    }
}
