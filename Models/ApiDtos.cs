namespace PokemonCardCollector.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PokemonCardCollector.Models.JsonConverters;

/// <summary>
/// DTO for a brief card response from TCGdex v2 API search results.
/// Contains minimal card information for listing operations.
/// </summary>
public class CardBriefApiDto
{
    /// <summary>
    /// Gets or sets the unique API identifier (e.g., "swsh3-136").
    /// </summary>
    [Required(ErrorMessage = "Card ID is required")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the card number within its set.
    /// </summary>
    [Required(ErrorMessage = "Local ID is required")]
    public required string LocalId { get; set; }

    /// <summary>
    /// Gets or sets the official card name.
    /// </summary>
    [Required(ErrorMessage = "Card name is required")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL to the card image.
    /// Optional - some API responses may not include it.
    /// </summary>
    [Url(ErrorMessage = "Image must be a valid URL")]
    public string? Image { get; set; }
}

/// <summary>
/// DTO for a card response from the TCGdex API.
/// </summary>
public class PokemonCardApiDto
{
    /// <summary>
    /// Gets or sets the unique API identifier (e.g., "swsh3-136").
    /// </summary>
    [Required(ErrorMessage = "Card ID is required")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the card number within its set.
    /// </summary>
    public string? LocalId { get; set; }

    /// <summary>
    /// Gets or sets the official card name.
    /// </summary>
    [Required(ErrorMessage = "Card name is required")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the card category: "Pokemon", "Trainer", or "Energy".
    /// </summary>
    [Required(ErrorMessage = "Card category is required")]
    [RegularExpression("^(Pokemon|Trainer|Energy)$", ErrorMessage = "Category must be Pokemon, Trainer, or Energy")]
    public required string Category { get; set; }

    /// <summary>
    /// Gets or sets the URL to the card image.
    /// </summary>
    [Url(ErrorMessage = "Image must be a valid URL")]
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the name of the artist who illustrated the card.
    /// </summary>
    public string? Illustrator { get; set; }

    /// <summary>
    /// Gets or sets the rarity of the card.
    /// </summary>
    public string? Rarity { get; set; }

    /// <summary>
    /// Gets or sets the set information.
    /// </summary>
    public CardSetApiDto? Set { get; set; }

    /// <summary>
    /// Gets or sets the available variants for the card.
    /// </summary>
    public CardVariantsApiDto? Variants { get; set; }

    /// <summary>
    /// Gets or sets when the card data was last updated.
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// Gets or sets the National Pokédex IDs.
    /// </summary>
    public int[]? DexId { get; set; }

    /// <summary>
    /// Gets or sets the Hit Points.
    /// </summary>
    [Range(0, 500, ErrorMessage = "HP must be between 0 and 500")]
    public int? Hp { get; set; }

    /// <summary>
    /// Gets or sets the Pokémon types.
    /// </summary>
    public string[]? Types { get; set; }

    /// <summary>
    /// Gets or sets the name of the Pokémon this card evolves from.
    /// </summary>
    public string? EvolveFrom { get; set; }

    /// <summary>
    /// Gets or sets the flavor text description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the evolution stage.
    /// </summary>
    public string? Stage { get; set; }

    /// <summary>
    /// Gets or sets the attacks.
    /// </summary>
    public AttackApiDto[]? Attacks { get; set; }

    /// <summary>
    /// Gets or sets the abilities.
    /// </summary>
    public AbilityApiDto[]? Abilities { get; set; }

    /// <summary>
    /// Gets or sets the weaknesses.
    /// </summary>
    public WeaknessResistanceApiDto[]? Weaknesses { get; set; }

    /// <summary>
    /// Gets or sets the resistances.
    /// </summary>
    public WeaknessResistanceApiDto[]? Resistances { get; set; }

    /// <summary>
    /// Gets or sets the retreat cost.
    /// </summary>
    [Range(0, 50, ErrorMessage = "Retreat cost must be between 0 and 50")]
    public int? Retreat { get; set; }

    /// <summary>
    /// Gets or sets the regulation mark.
    /// </summary>
    public string? RegulationMark { get; set; }

    /// <summary>
    /// Gets or sets the legality information.
    /// </summary>
    public CardLegalityApiDto? Legal { get; set; }

    /// <summary>
    /// Gets or sets the card effect text.
    /// </summary>
    public string? Effect { get; set; }

    /// <summary>
    /// Gets or sets the trainer type.
    /// </summary>
    public string? TrainerType { get; set; }

    /// <summary>
    /// Gets or sets the energy type.
    /// </summary>
    public string? EnergyType { get; set; }

    /// <summary>
    /// Gets or sets pricing information.
    /// </summary>
    public PricingApiDto? Pricing { get; set; }
}

/// <summary>
/// DTO for set information in card API responses.
/// </summary>
public class CardSetApiDto
{
    /// <summary>
    /// Gets or sets the set identifier.
    /// </summary>
    [Required(ErrorMessage = "Set ID is required")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the set name.
    /// </summary>
    [Required(ErrorMessage = "Set name is required")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the set logo URL.
    /// </summary>
    [Url(ErrorMessage = "Logo URL must be a valid URL")]
    public string? Logo { get; set; }

    /// <summary>
    /// Gets or sets the set symbol URL.
    /// </summary>
    [Url(ErrorMessage = "Symbol URL must be a valid URL")]
    public string? Symbol { get; set; }

    /// <summary>
    /// Gets or sets the card count information for the set.
    /// </summary>
    public CardCountApiDto? CardCount { get; set; }
}

/// <summary>
/// DTO for card count information in set responses.
/// </summary>
public class CardCountApiDto
{
    /// <summary>
    /// Gets or sets the total number of cards in the set.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Total card count must be non-negative")]
    public int Total { get; set; }

    /// <summary>
    /// Gets or sets the number of officially released cards in the set.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Official card count must be non-negative")]
    public int Official { get; set; }
}

/// <summary>
/// DTO for card variants in API responses.
/// </summary>
public class CardVariantsApiDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the normal variant is available.
    /// </summary>
    public bool Normal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the reverse holofoil variant is available.
    /// </summary>
    public bool Reverse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the holofoil variant is available.
    /// </summary>
    public bool Holo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the first edition variant is available.
    /// </summary>
    public bool FirstEdition { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the WPromo variant is available (v2 API).
    /// </summary>
    public bool WPromo { get; set; }
}

/// <summary>
/// DTO for attack information in card API responses.
/// Supports multi-language API responses (en, de, etc.).
/// </summary>
public class AttackApiDto
{
    /// <summary>
    /// Gets or sets the attack name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the energy cost of the attack.
    /// </summary>
    public string[]? Cost { get; set; }

    /// <summary>
    /// Gets or sets the attack effect text.
    /// </summary>
    public string? Effect { get; set; }

    /// <summary>
    /// Gets or sets the damage dealt by the attack (can be a string like "50+" or a number).
    /// </summary>
    [JsonConverter(typeof(DamageJsonConverter))]
    public string? Damage { get; set; }
}

/// <summary>
/// DTO for ability information in card API responses.
/// Supports multi-language API responses (en, de, etc.).
/// </summary>
public class AbilityApiDto
{
    /// <summary>
    /// Gets or sets the ability name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the ability type.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the ability effect text.
    /// May be null or empty in some localized API responses.
    /// </summary>
    public string? Effect { get; set; }
}

/// <summary>
/// DTO for weakness/resistance information in card API responses.
/// Supports multi-language API responses (en, de, etc.).
/// </summary>
public class WeaknessResistanceApiDto
{
    /// <summary>
    /// Gets or sets the type affected by weakness or resistance.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the value (e.g., "×2" for weakness or "-20" for resistance).
    /// </summary>
    public string? Value { get; set; }
}

/// <summary>
/// DTO for card legality information in API responses.
/// </summary>
public class CardLegalityApiDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the card is legal in Standard format.
    /// </summary>
    public bool Standard { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the card is legal in Expanded format.
    /// </summary>
    public bool Expanded { get; set; }
}

/// <summary>
/// DTO for pricing information in card API responses.
/// </summary>
public class PricingApiDto
{
    /// <summary>
    /// Gets or sets the TCGPlayer pricing information.
    /// </summary>
    public PriceMarketApiDto? Tcgplayer { get; set; }

    /// <summary>
    /// Gets or sets the Cardmarket pricing information.
    /// </summary>
    public PriceMarketApiDto? Cardmarket { get; set; }
}

/// <summary>
/// DTO for marketplace pricing information in API responses.
/// Supports both TCGPlayer (variant-based) and Cardmarket (flat structure) pricing formats.
/// </summary>
public class PriceMarketApiDto
{
    /// <summary>
    /// Gets or sets when the pricing was last updated.
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// Gets or sets the currency unit (USD, EUR, etc.).
    /// </summary>
    [StringLength(10, ErrorMessage = "Currency unit cannot exceed 10 characters")]
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the normal (non-foil) variant pricing.
    /// </summary>
    public PriceVariantApiDto? Normal { get; set; }

    /// <summary>
    /// Gets or sets the holofoil variant pricing (TCGPlayer).
    /// </summary>
    [JsonPropertyName("holofoil")]
    public PriceVariantApiDto? Holofoil { get; set; }

    /// <summary>
    /// Gets or sets the reverse holofoil variant pricing (TCGPlayer).
    /// </summary>
    [JsonPropertyName("reverse-holofoil")]
    public PriceVariantApiDto? ReverseHolofoil { get; set; }

    /// <summary>
    /// Gets or sets the average price (for Cardmarket).
    /// </summary>
    [Range(0.01, 999999.99, ErrorMessage = "Average price must be between 0.01 and 999999.99")]
    public decimal? Avg { get; set; }

    /// <summary>
    /// Gets or sets the lowest price (for Cardmarket).
    /// </summary>
    [Range(0.01, 999999.99, ErrorMessage = "Low price must be between 0.01 and 999999.99")]
    public decimal? Low { get; set; }

    /// <summary>
    /// Gets or sets the trend price (for Cardmarket).
    /// </summary>
    [Range(0.01, 999999.99, ErrorMessage = "Trend price must be between 0.01 and 999999.99")]
    public decimal? Trend { get; set; }
}

/// <summary>
/// DTO for individual variant pricing in API responses.
/// </summary>
public class PriceVariantApiDto
{
    /// <summary>
    /// Gets or sets the lowest available price.
    /// </summary>
    [Range(0.01, 999999.99, ErrorMessage = "Low price must be between 0.01 and 999999.99")]
    public decimal? LowPrice { get; set; }

    /// <summary>
    /// Gets or sets the median market price.
    /// </summary>
    [Range(0.01, 999999.99, ErrorMessage = "Mid price must be between 0.01 and 999999.99")]
    public decimal? MidPrice { get; set; }

    /// <summary>
    /// Gets or sets the highest available price.
    /// </summary>
    [Range(0.01, 999999.99, ErrorMessage = "High price must be between 0.01 and 999999.99")]
    public decimal? HighPrice { get; set; }

    /// <summary>
    /// Gets or sets the current market price.
    /// </summary>
    public decimal? MarketPrice { get; set; }

    /// <summary>
    /// Gets or sets the lowest direct seller price.
    /// </summary>
    public decimal? DirectLowPrice { get; set; }
}
