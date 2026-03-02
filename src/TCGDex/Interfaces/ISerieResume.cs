namespace TCGDex.Interfaces;

/// <summary>
/// Brief series information (id, name, logo).
/// </summary>
public interface ISerieResume
{
    string Id { get; }
    string Name { get; }
    string? Logo { get; }
}
