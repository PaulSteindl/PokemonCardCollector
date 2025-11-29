namespace PokemonCardCollector.Models;

/// <summary>
/// Represents statistics about a user's Pokémon card collection.
/// </summary>
public class CollectionStatistics
{
    /// <summary>
    /// Gets or sets the total number of cards in the collection.
    /// </summary>
    public int TotalCards { get; set; }

    /// <summary>
    /// Gets or sets the total estimated value of all cards in the collection.
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Gets or sets the number of unique sets in the collection.
    /// </summary>
    public int UniqueSets { get; set; }

    /// <summary>
    /// Gets or sets the number of Pokémon cards in the collection.
    /// </summary>
    public int PokemonCardCount { get; set; }

    /// <summary>
    /// Gets or sets the number of Trainer cards in the collection.
    /// </summary>
    public int TrainerCardCount { get; set; }

    /// <summary>
    /// Gets or sets the number of Energy cards in the collection.
    /// </summary>
    public int EnergyCardCount { get; set; }

    /// <summary>
    /// Gets or sets the number of Rare cards in the collection.
    /// </summary>
    public int RareCardCount { get; set; }

    /// <summary>
    /// Gets or sets the number of Holo variant cards in the collection.
    /// </summary>
    public int HoloCardCount { get; set; }

    /// <summary>
    /// Gets or sets the number of reverse Holo variant cards in the collection.
    /// </summary>
    public int ReverseHoloCardCount { get; set; }

    /// <summary>
    /// Gets or sets the number of first edition cards in the collection.
    /// </summary>
    public int FirstEditionCardCount { get; set; }

    /// <summary>
    /// Gets or sets a breakdown of cards by type.
    /// Key is the type, value is the count.
    /// </summary>
    public Dictionary<string, int> CardsByType { get; set; } = [];

    /// <summary>
    /// Gets or sets a breakdown of cards by rarity.
    /// Key is the rarity, value is the count.
    /// </summary>
    public Dictionary<string, int> CardsByRarity { get; set; } = [];

    /// <summary>
    /// Gets or sets a breakdown of cards by condition.
    /// Key is the condition, value is the count.
    /// </summary>
    public Dictionary<string, int> CardsByCondition { get; set; } = [];
}
