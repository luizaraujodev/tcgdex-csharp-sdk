namespace TCGDex;

/// <summary>
/// Cache abstraction for TCGDex API responses (by key with TTL).
/// </summary>
public interface ITCGDexCache
{
    /// <summary>
    /// Gets a cached value by key, or null if missing/expired.
    /// </summary>
    object? Get(string key);

    /// <summary>
    /// Sets a value in cache with the given TTL in seconds.
    /// </summary>
    void Set(string key, object value, int ttlSeconds);
}
