namespace TCGDex;

/// <summary>
/// Fluent query builder for TCGDex API filter and sort parameters.
/// </summary>
public class Query
{
    private readonly List<(string Key, object Value)> _params = [];

    /// <summary>
    /// The current query parameters (key-value pairs for URL query string).
    /// </summary>
    public IReadOnlyList<(string Key, object Value)> Params => _params;

    /// <summary>
    /// Negation helpers (not equal, not contains, is not null).
    /// </summary>
    public QueryNot Not { get; }

    private Query()
    {
        Not = new QueryNot(this);
    }

    /// <summary>
    /// Creates a new empty query.
    /// </summary>
    public static Query Create() => new();

    /// <summary>
    /// Chainable: add a "contains" filter (includes value).
    /// </summary>
    public Query Contains(string key, string value)
    {
        _params.Add((key, value));
        return this;
    }

    /// <summary>
    /// Alias for Contains.
    /// </summary>
    public Query Includes(string key, string value) => Contains(key, value);

    /// <summary>
    /// Alias for Contains (like/similar).
    /// </summary>
    public Query Like(string key, string value) => Contains(key, value);

    /// <summary>
    /// Chainable: add an equality filter (eq:value).
    /// </summary>
    public Query Equal(string key, string value)
    {
        _params.Add((key, $"eq:{value}"));
        return this;
    }

    /// <summary>
    /// Chainable: add sort by field and order.
    /// </summary>
    public Query Sort(string key, SortOrder order = SortOrder.Asc)
    {
        _params.Add(("sort:field", key));
        _params.Add(("sort:order", order == SortOrder.Asc ? "ASC" : "DESC"));
        return this;
    }

    /// <summary>
    /// Chainable: greater than or equal (gte:value).
    /// </summary>
    public Query GreaterOrEqualThan(string key, int value)
    {
        _params.Add((key, $"gte:{value}"));
        return this;
    }

    /// <summary>
    /// Chainable: less than or equal (lte:value).
    /// </summary>
    public Query LesserOrEqualThan(string key, int value)
    {
        _params.Add((key, $"lte:{value}"));
        return this;
    }

    /// <summary>
    /// Chainable: greater than (gt:value).
    /// </summary>
    public Query GreaterThan(string key, int value)
    {
        _params.Add((key, $"gt:{value}"));
        return this;
    }

    /// <summary>
    /// Chainable: less than (lt:value).
    /// </summary>
    public Query LesserThan(string key, int value)
    {
        _params.Add((key, $"lt:{value}"));
        return this;
    }

    /// <summary>
    /// Chainable: filter where key is null.
    /// </summary>
    public Query IsNull(string key)
    {
        _params.Add((key, "null:"));
        return this;
    }

    /// <summary>
    /// Chainable: paginate (page and items per page).
    /// </summary>
    public Query Paginate(int page, int itemsPerPage)
    {
        _params.Add(("pagination:page", page));
        _params.Add(("pagination:itemsPerPage", itemsPerPage));
        return this;
    }

    /// <summary>
    /// Inner class for negation methods (Not.Equal, Not.Contains, Not.IsNull).
    /// </summary>
    public class QueryNot
    {
        private readonly Query _owner;

        internal QueryNot(Query owner) => _owner = owner;

        public Query Equal(string key, string value)
        {
            _owner._params.Add((key, $"neq:{value}"));
            return _owner;
        }

        public Query Contains(string key, string value)
        {
            _owner._params.Add((key, $"not:{value}"));
            return _owner;
        }

        public Query Includes(string key, string value) => Contains(key, value);
        public Query Like(string key, string value) => Contains(key, value);

        public Query IsNull(string key)
        {
            _owner._params.Add((key, "notnull:"));
            return _owner;
        }
    }
}

/// <summary>
/// Sort order for query.
/// </summary>
public enum SortOrder
{
    Asc,
    Desc
}
