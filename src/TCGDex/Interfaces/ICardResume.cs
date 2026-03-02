namespace TCGDex.Interfaces;

/// <summary>
/// Brief card information (id, localId, name, image).
/// </summary>
public interface ICardResume
{
    string Id { get; }
    string LocalId { get; }
    string Name { get; }
    string? Image { get; }
}
