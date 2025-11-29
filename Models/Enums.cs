namespace PokemonCardCollector.Models;

/// <summary>
/// Enumerates the possible card conditions in a Pokémon card collection.
/// </summary>
public enum CardCondition
{
    /// <summary>
    /// Unspecified or unknown condition.
    /// </summary>
    Unspecified,

    /// <summary>
    /// Mint condition - Near perfect, minimal wear.
    /// </summary>
    Mint,

    /// <summary>
    /// Near Mint condition - Slight wear visible.
    /// </summary>
    NearMint,

    /// <summary>
    /// Lightly Played condition - Light wear, visible use.
    /// </summary>
    LightlyPlayed,

    /// <summary>
    /// Played condition - Moderate wear and damage.
    /// </summary>
    Played,

    /// <summary>
    /// Poor condition - Heavy wear and significant damage.
    /// </summary>
    PoorCondition
}

/// <summary>
/// Enumerates the possible Pokémon card types.
/// </summary>
public enum PokemonType
{
    /// <summary>
    /// Colorless type Pokémon.
    /// </summary>
    Colorless,

    /// <summary>
    /// Darkness type Pokémon.
    /// </summary>
    Darkness,

    /// <summary>
    /// Dragon type Pokémon.
    /// </summary>
    Dragon,

    /// <summary>
    /// Fairy type Pokémon.
    /// </summary>
    Fairy,

    /// <summary>
    /// Fighting type Pokémon.
    /// </summary>
    Fighting,

    /// <summary>
    /// Fire type Pokémon.
    /// </summary>
    Fire,

    /// <summary>
    /// Grass type Pokémon.
    /// </summary>
    Grass,

    /// <summary>
    /// Lightning type Pokémon.
    /// </summary>
    Lightning,

    /// <summary>
    /// Metal type Pokémon.
    /// </summary>
    Metal,

    /// <summary>
    /// Psychic type Pokémon.
    /// </summary>
    Psychic,

    /// <summary>
    /// Water type Pokémon.
    /// </summary>
    Water
}

/// <summary>
/// Enumerates the possible evolution stages for Pokémon cards.
/// </summary>
public enum EvolutionStage
{
    /// <summary>
    /// Basic Pokémon - can be played directly from hand.
    /// </summary>
    Basic,

    /// <summary>
    /// Stage 1 evolution - evolves from a Basic Pokémon.
    /// </summary>
    Stage1,

    /// <summary>
    /// Stage 2 evolution - evolves from a Stage 1 Pokémon.
    /// </summary>
    Stage2,

    /// <summary>
    /// Level Up evolution - older evolution style.
    /// </summary>
    LevelUp,

    /// <summary>
    /// V-MAX evolution - Pokémon V that evolved to maximum form.
    /// </summary>
    VMAX,

    /// <summary>
    /// V-STAR evolution - Pokémon V that evolved to star form.
    /// </summary>
    VSTAR,

    /// <summary>
    /// EX card - high power alternate card.
    /// </summary>
    EX,

    /// <summary>
    /// GX card - high power alternate card.
    /// </summary>
    GX
}

/// <summary>
/// Enumerates the card categories supported by the TCGdex API.
/// </summary>
public enum CardCategory
{
    /// <summary>
    /// A Pokémon card - battle creature with HP, attacks, and abilities.
    /// </summary>
    Pokemon,

    /// <summary>
    /// A Trainer card - support card with various effects.
    /// </summary>
    Trainer,

    /// <summary>
    /// An Energy card - resource card needed to power attacks.
    /// </summary>
    Energy
}

/// <summary>
/// Enumerates the types of Trainer cards.
/// </summary>
public enum TrainerType
{
    /// <summary>
    /// Supporter - powerful cards limited to one per turn.
    /// </summary>
    Supporter,

    /// <summary>
    /// Item - single-use cards with immediate effects.
    /// </summary>
    Item,

    /// <summary>
    /// Stadium - continuous effect cards affecting both players.
    /// </summary>
    Stadium,

    /// <summary>
    /// Pokémon Tool - cards attached to Pokémon for effects.
    /// </summary>
    PokemonTool,

    /// <summary>
    /// Pokémon Tool F - restricted tool cards.
    /// </summary>
    PokemonToolF,

    /// <summary>
    /// Ace Spec - high-power cards with special rule.
    /// </summary>
    AceSpec,

    /// <summary>
    /// Ancient Rotor - ancient type tools.
    /// </summary>
    AncientRotor,

    /// <summary>
    /// Pendant - pendant type tools.
    /// </summary>
    Pendant,

    /// <summary>
    /// Technical Machine - temporary effects for Pokémon.
    /// </summary>
    TechnicalMachine,

    /// <summary>
    /// Scoop Up - utility cards.
    /// </summary>
    ScoopUp
}

/// <summary>
/// Enumerates the types of Energy cards.
/// </summary>
public enum EnergyType
{
    /// <summary>
    /// Basic energy - standard single-type energy.
    /// </summary>
    Basic,

    /// <summary>
    /// Special energy - multi-type or special effect energy.
    /// </summary>
    Special
}

/// <summary>
/// Enumerates the format legality of cards.
/// </summary>
public enum CardLegality
{
    /// <summary>
    /// Card is legal in the current Standard format.
    /// </summary>
    Standard,

    /// <summary>
    /// Card is legal in the Expanded format (includes older sets).
    /// </summary>
    Expanded,

    /// <summary>
    /// Card is restricted - only in specific tournament formats.
    /// </summary>
    Restricted,

    /// <summary>
    /// Card is banned and cannot be used.
    /// </summary>
    Banned
}
