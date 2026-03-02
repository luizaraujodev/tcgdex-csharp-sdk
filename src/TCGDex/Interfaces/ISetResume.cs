namespace TCGDex.Interfaces;

/// <summary>
/// Brief set information (id, name, logo, symbol, card count).
/// </summary>
public interface ISetResume
{
    string Id { get; }
    string Name { get; }
    string? Logo { get; }
    string? Symbol { get; }
    ISetCardCount CardCount { get; }
}

/// <summary>
/// Card count summary (total, official).
/// </summary>
public interface ISetCardCount
{
    int Total { get; }
    int Official { get; }
    int? Normal { get; }
    int? Reverse { get; }
    int? Holo { get; }
    int? FirstEd { get; }
}
