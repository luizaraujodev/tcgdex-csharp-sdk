namespace TCGDex.Interfaces;

/// <summary>
/// Card/set variant flags (normal, reverse, holo, first edition).
/// </summary>
public interface IVariants
{
    bool? Normal { get; }
    bool? Reverse { get; }
    bool? Holo { get; }
    bool? FirstEdition { get; }
}
