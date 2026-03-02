namespace TCGDex.Interfaces;

/// <summary>
/// Booster pack reference (id, name, logo, artwork).
/// </summary>
public interface IBooster
{
    string Id { get; }
    string Name { get; }
    string? Logo { get; }
    string? ArtworkFront { get; }
    string? ArtworkBack { get; }
}
