namespace TCGDex.Interfaces;

/// <summary>
/// Full card with category, rarity, set, attacks, abilities, etc.
/// </summary>
public interface ICard : ICardResume
{
    string? Illustrator { get; }
    string Rarity { get; }
    string Category { get; }
    IVariants? Variants { get; }
    ISetResume Set { get; }
    /// <summary>
    /// Pokemon Pokédex IDs (numeric).
    /// </summary>
    IReadOnlyList<int>? DexId { get; }
    int? Hp { get; }
    IReadOnlyList<string>? Types { get; }
    string? EvolveFrom { get; }
    string? Weight { get; }
    string? Description { get; }
    object? Level { get; } // number | string (e.g. "X")
    string? Stage { get; }
    string? Suffix { get; }
    ICardItem? Item { get; }
    IReadOnlyList<IAbility>? Abilities { get; }
    IReadOnlyList<IAttack>? Attacks { get; }
    IReadOnlyList<IWeaknessResistance>? Weaknesses { get; }
    IReadOnlyList<IWeaknessResistance>? Resistances { get; }
    int? Retreat { get; }
    string? Effect { get; }
    string? TrainerType { get; }
    string? EnergyType { get; }
    string? RegulationMark { get; }
    ILegal Legal { get; }
    IReadOnlyList<IBooster>? Boosters { get; }
}

/// <summary>
/// Held item on a Pokemon card.
/// </summary>
public interface ICardItem
{
    string Name { get; }
    string Effect { get; }
}

/// <summary>
/// Ability (type, name, effect).
/// </summary>
public interface IAbility
{
    string Type { get; }
    string Name { get; }
    string Effect { get; }
}

/// <summary>
/// Attack (cost, name, effect, damage).
/// </summary>
public interface IAttack
{
    IReadOnlyList<string>? Cost { get; }
    string Name { get; }
    string? Effect { get; }
    object? Damage { get; } // string | number
}

/// <summary>
/// Weakness or resistance (type, value).
/// </summary>
public interface IWeaknessResistance
{
    string Type { get; }
    string? Value { get; }
}
