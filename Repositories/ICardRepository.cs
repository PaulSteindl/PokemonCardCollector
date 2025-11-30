namespace PokemonCardCollector.Repositories;

using PokemonCardCollector.Models;

/// <summary>
/// Defines the contract for card repository operations.
/// Provides abstraction for data access layer, enabling loose coupling and testability.
/// All operations are async to prevent blocking the thread pool.
/// </summary>
public interface ICardRepository
{
    /// <summary>
    /// Retrieves a card by its unique database identifier.
    /// </summary>
    /// <param name="id">The database ID of the card.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The card if found; null if not found.</returns>
    Task<Card?> GetCardByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all cards in the database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of all cards.</returns>
    Task<IEnumerable<Card>> GetAllCardsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for cards by name using case-insensitive partial matching.
    /// </summary>
    /// <param name="name">The card name or partial name to search for.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of cards matching the search criteria.</returns>
    Task<IEnumerable<Card>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for a card by its card number (LocalId) within a set.
    /// </summary>
    /// <param name="cardNumber">The card number to search for (e.g., "136").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of cards with the matching number.</returns>
    Task<IEnumerable<Card>> SearchByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all cards of a specific type (PokemonCard, TrainerCard, or EnergyCard).
    /// </summary>
    /// <param name="cardType">The card type to filter by (e.g., "PokemonCard").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of cards of the specified type.</returns>
    Task<IEnumerable<Card>> GetCardsByTypeAsync(string cardType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a card with the given API ID already exists in the database.
    /// Critical for preventing duplicate imports from external APIs.
    /// </summary>
    /// <param name="apiId">The external API ID to check (e.g., "swsh3-136").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>True if a card with the API ID exists; false otherwise.</returns>
    Task<bool> CardExistsAsync(string apiId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new card to the database.
    /// Validates that the card does not already exist (by ApiId) before inserting.
    /// </summary>
    /// <param name="card">The card entity to add.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The added card with its database-generated ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a card with the same ApiId already exists.</exception>
    Task<Card> AddCardAsync(Card card, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing card in the database.
    /// </summary>
    /// <param name="card">The card entity with updated values.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The updated card entity.</returns>
    /// <exception cref="ArgumentException">Thrown if the card ID is invalid or card not found.</exception>
    Task<Card> UpdateCardAsync(Card card, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a card from the database by its ID.
    /// </summary>
    /// <param name="id">The database ID of the card to delete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>True if the card was successfully deleted; false if not found.</returns>
    Task<bool> DeleteCardAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves cards with pagination support for efficient data loading.
    /// </summary>
    /// <param name="pageNumber">The page number (1-indexed).</param>
    /// <param name="pageSize">The number of cards per page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of paginated cards.</returns>
    Task<IEnumerable<Card>> GetCardsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of cards in the database.
    /// Useful for pagination and statistics.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The total number of cards in the database.</returns>
    Task<int> GetCardCountAsync(CancellationToken cancellationToken = default);
}
