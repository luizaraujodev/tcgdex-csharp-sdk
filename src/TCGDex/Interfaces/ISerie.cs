namespace TCGDex.Interfaces;

/// <summary>
/// Full series with list of sets.
/// </summary>
public interface ISerie : ISerieResume
{
    IReadOnlyList<ISetResume> Sets { get; }
}
