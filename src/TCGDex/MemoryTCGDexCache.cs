namespace TCGDex;

/// <summary>
/// In-memory cache for TCGDex API responses (Dictionary + expiration).
/// </summary>
public sealed class MemoryTCGDexCache : ITCGDexCache
{
    private readonly Dictionary<string, (object Value, DateTime ExpiresAt)> _store = new();
    private readonly object _lock = new();

    /// <inheritdoc />
    public object? Get(string key)
    {
        lock (_lock)
        {
            if (!_store.TryGetValue(key, out var entry))
                return null;
            if (DateTime.UtcNow >= entry.ExpiresAt)
            {
                _store.Remove(key);
                return null;
            }
            return entry.Value;
        }
    }

    /// <inheritdoc />
    public void Set(string key, object value, int ttlSeconds)
    {
        var expiresAt = DateTime.UtcNow.AddSeconds(ttlSeconds);
        lock (_lock)
        {
            _store[key] = (value, expiresAt);
        }
    }
}
