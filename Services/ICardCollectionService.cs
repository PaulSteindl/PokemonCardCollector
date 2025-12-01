namespace PokemonCardCollector.Services;

using PokemonCardCollector.Models;

/// <summary>
/// Defines the contract for business logic operations related to card collection management.
/// Coordinates between the external API service and the repository to provide high-level card operations.
/// All operations are async to prevent blocking the thread pool.
/// </summary>
public interface ICardCollectionService
{
    /// <summary>
    /// Adds a new card to the user's collection by fetching it from the external API and storing in the database.
    /// Validates that the card does not already exist before adding.
    /// </summary>
    /// <param name="apiId">The unique API identifier from TCGdex (e.g., "swsh3-136").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The added card entity with database-generated ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown if card already exists or API fetch fails.</exception>
    /// <exception cref="ArgumentException">Thrown if apiId is null or empty.</exception>
    Task<Card> AddCardFromApiAsync(string apiId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches the user's local collection by card name using case-insensitive partial matching.
    /// </summary>
    /// <param name="cardName">The card name or partial name to search for.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of matching cards from the local collection.</returns>
    /// <exception cref="ArgumentException">Thrown if cardName is null or empty.</exception>
    Task<IEnumerable<Card>> SearchLocalCardsAsync(string cardName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all cards in the user's collection with optional filtering by type.
    /// Supports pagination for efficient loading of large collections.
    /// </summary>
    /// <param name="cardType">Optional card type filter (null for all types).</param>
    /// <param name="pageNumber">The page number (1-indexed) for pagination.</param>
    /// <param name="pageSize">The number of cards per page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of paginated cards, optionally filtered by type.</returns>
    Task<IEnumerable<Card>> GetUserCollectionAsync(
        string? cardType = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a card from the user's collection by its database ID.
    /// </summary>
    /// <param name="cardId">The database ID of the card to remove.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>True if the card was successfully deleted; false if not found.</returns>
    /// <exception cref="InvalidOperationException">Thrown if card deletion fails due to database error.</exception>
    Task<bool> RemoveCardAsync(int cardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the condition and notes of a card in the collection.
    /// </summary>
    /// <param name="cardId">The database ID of the card to update.</param>
    /// <param name="condition">The new condition value (Mint, NearMint, LightlyPlayed, Played, PoorCondition).</param>
    /// <param name="userNotes">Optional user notes about the card.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The updated card entity.</returns>
    /// <exception cref="ArgumentException">Thrown if cardId is invalid or card not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if update fails due to database error.</exception>
    Task<Card> UpdateCardAsync(
        int cardId,
        string? condition = null,
        string? userNotes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves comprehensive statistics about the user's collection.
    /// Includes counts by type, rarity, condition, and estimated total value.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A CollectionStatistics object containing aggregated collection data.</returns>
    Task<CollectionStatistics> GetCollectionStatisticsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for a card by name in the external API and returns results without adding to collection.
    /// Useful for browsing before adding.
    /// </summary>
    /// <param name="cardName">The card name or partial name to search for.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of cards from the external API matching the search.</returns>
    /// <exception cref="ArgumentException">Thrown if cardName is null or empty.</exception>
    Task<IEnumerable<Card>> BrowseCardsByNameAsync(string cardName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for a card by number in the external API with optional set filter.
    /// </summary>
    /// <param name="cardNumber">The card number to search for (e.g., "136").</param>
    /// <param name="setId">Optional set identifier to narrow the search (e.g., "swsh3").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of matching cards from the external API.</returns>
    /// <exception cref="ArgumentException">Thrown if cardNumber is null or empty.</exception>
    Task<IEnumerable<Card>> BrowseCardsByNumberAsync(
        string cardNumber,
        string? setId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific card's detailed information by its API ID from the external API.
    /// </summary>
    /// <param name="apiId">The unique API identifier from TCGdex (e.g., "swsh3-136").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The card details from the external API, or null if not found.</returns>
    /// <exception cref="ArgumentException">Thrown if apiId is null or empty.</exception>
    Task<Card?> GetCardDetailsFromApiAsync(string apiId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all available card sets from the external API.
    /// Useful for displaying set information to users.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of all available card sets.</returns>
    Task<IEnumerable<CardSetApiDto>> GetAvailableSetsAsync(CancellationToken cancellationToken = default);
}
