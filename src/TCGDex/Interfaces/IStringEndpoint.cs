namespace TCGDex.Interfaces;

/// <summary>
/// String-filter endpoint result (e.g. types, rarities): name + list of cards.
/// </summary>
public interface IStringEndpoint
{
    string Name { get; }
    IReadOnlyList<ICardResume> Cards { get; }
}
