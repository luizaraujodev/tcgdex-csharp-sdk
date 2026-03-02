namespace TCGDex.Interfaces;

/// <summary>
/// Full set with serie, cards, legal, variants, release date, boosters.
/// </summary>
public interface ISet : ISetResume
{
    ISerieResume Serie { get; }
    string? TcgOnline { get; }
    IVariants? Variants { get; }
    string ReleaseDate { get; }
    ILegal Legal { get; }
    IReadOnlyList<ICardResume> Cards { get; }
    IReadOnlyList<IBooster>? Boosters { get; }
}

/// <summary>
/// Tournament legality (standard, expanded).
/// </summary>
public interface ILegal
{
    bool Standard { get; }
    bool Expanded { get; }
}
