namespace PokemonCardCollector.Models;

/// <summary>
/// Represents the base domain model for all Trading Card Game cards.
/// Uses Table-Per-Hierarchy (TPH) inheritance with discriminator column.
/// This abstract base class defines common properties shared across all card types.
/// </summary>
public abstract class Card
{
    /// <summary>
    /// Gets or sets the unique database identifier for the card.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique API identifier from TCGdex (e.g., "swsh3-136").
    /// </summary>
    public required string ApiId { get; set; }

    /// <summary>
    /// Gets or sets the card number within its set (e.g., "136").
    /// </summary>
    public required string LocalId { get; set; }

    /// <summary>
    /// Gets or sets the official card name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL to the card image.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the name of the artist who illustrated the card.
    /// </summary>
    public string? Illustrator { get; set; }

    /// <summary>
    /// Gets or sets the rarity of the card (Common, Uncommon, Rare, RareHolo, etc.).
    /// </summary>
    public string? Rarity { get; set; }

    /// <summary>
    /// Gets or sets the set identifier (e.g., "swsh3").
    /// </summary>
    public required string SetId { get; set; }

    /// <summary>
    /// Gets or sets the set name (e.g., "Darkness Ablaze").
    /// </summary>
    public required string SetName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the normal (non-foil) variant is available.
    /// </summary>
    public bool VariantNormal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the reverse holofoil variant is available.
    /// </summary>
    public bool VariantReverse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the regular holofoil variant is available.
    /// </summary>
    public bool VariantHolo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the first edition variant is available.
    /// </summary>
    public bool VariantFirstEdition { get; set; }

    /// <summary>
    /// Gets or sets the TCGPlayer market price in USD.
    /// </summary>
    public decimal? TcgPlayerPrice { get; set; }

    /// <summary>
    /// Gets or sets the Cardmarket price in EUR.
    /// </summary>
    public decimal? CardmarketPrice { get; set; }

    /// <summary>
    /// Gets or sets when the card data was last updated from the API.
    /// </summary>
    public DateTime Updated { get; set; }

    /// <summary>
    /// Gets or sets the date when the card was added to the collection.
    /// </summary>
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets user notes about the card (e.g., condition details, origin, etc.).
    /// </summary>
    public string? UserNotes { get; set; }

    /// <summary>
    /// Gets or sets the condition of the card: Mint, NearMint, LightlyPlayed, Played, PoorCondition.
    /// </summary>
    public string? Condition { get; set; }

    /// <summary>
    /// Gets or sets the estimated value of the card based on market data.
    /// </summary>
    public decimal? EstimatedValue { get; set; }
}

/// <summary>
/// Represents a Pokémon Trading Card Game Pokémon card.
/// Inherits common card properties from Card base class.
/// Contains properties specific to Pokémon cards including attributes, attacks, and abilities.
/// </summary>
public class PokemonCard : Card
{
    /// <summary>
    /// Gets or sets the National Pokédex ID(s) as a JSON array string (e.g., "[162]").
    /// </summary>
    public string? DexId { get; set; }

    /// <summary>
    /// Gets or sets the Hit Points of the Pokémon.
    /// </summary>
    public int? Hp { get; set; }

    /// <summary>
    /// Gets or sets the Pokémon types as a JSON array string (e.g., "["Colorless"]").
    /// Valid types: Colorless, Darkness, Dragon, Fairy, Fighting, Fire, Grass, Lightning, Metal, Psychic, Water.
    /// </summary>
    public string? Types { get; set; }

    /// <summary>
    /// Gets or sets the name of the Pokémon this card evolves from.
    /// </summary>
    public string? EvolveFrom { get; set; }

    /// <summary>
    /// Gets or sets the flavor text description of the Pokémon.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the evolution stage: Basic, Stage1, Stage2, LevelUp, VMAX, VSTAR.
    /// </summary>
    public string? Stage { get; set; }

    /// <summary>
    /// Gets or sets the attacks as a JSON array string.
    /// Each attack contains: name, cost (array of energy types), effect, damage (optional).
    /// </summary>
    public string? Attacks { get; set; }

    /// <summary>
    /// Gets or sets the abilities as a JSON array string.
    /// Each ability contains: name, type, effect.
    /// </summary>
    public string? Abilities { get; set; }

    /// <summary>
    /// Gets or sets the weaknesses as a JSON array string.
    /// Each weakness contains: type, value (e.g., "×2").
    /// </summary>
    public string? Weaknesses { get; set; }

    /// <summary>
    /// Gets or sets the resistances as a JSON array string.
    /// Each resistance contains: type, value (e.g., "-20").
    /// </summary>
    public string? Resistances { get; set; }

    /// <summary>
    /// Gets or sets the retreat cost of the Pokémon.
    /// </summary>
    public int? RetreatCost { get; set; }

    /// <summary>
    /// Gets or sets the regulation mark indicating the card format legality (e.g., "D").
    /// </summary>
    public string? RegulationMark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the card is legal in the Standard format.
    /// </summary>
    public bool LegalStandard { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the card is legal in the Expanded format.
    /// </summary>
    public bool LegalExpanded { get; set; }
}

/// <summary>
/// Represents a Trainer Trading Card Game card (Supporter, Item, Stadium, Pokémon Tool, etc.).
/// Inherits common card properties from Card base class.
/// Contains properties specific to Trainer cards.
/// </summary>
public class TrainerCard : Card
{
    /// <summary>
    /// Gets or sets the type of Trainer card: Supporter, Item, Stadium, Pokémon Tool, etc.
    /// </summary>
    public required string TrainerType { get; set; }

    /// <summary>
    /// Gets or sets the effect text of the card describing what it does.
    /// </summary>
    public string? Effect { get; set; }
}

/// <summary>
/// Represents an Energy Trading Card Game card (Basic Energy or Special Energy).
/// Inherits common card properties from Card base class.
/// Contains properties specific to Energy cards.
/// </summary>
public class EnergyCard : Card
{
    /// <summary>
    /// Gets or sets the energy type: "Basic" or "Special".
    /// </summary>
    public required string EnergyType { get; set; }

    /// <summary>
    /// Gets or sets the effect text of the card for special energy cards.
    /// </summary>
    public string? Effect { get; set; }
}
