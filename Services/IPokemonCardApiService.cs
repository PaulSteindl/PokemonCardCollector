namespace PokemonCardCollector.Services;

using PokemonCardCollector.Models;

/// <summary>
/// Defines the contract for external Pokémon card API client operations.
/// Provides abstraction for fetching card data from TCGdex API and mapping to domain models.
/// All operations are async to prevent blocking the thread pool.
/// </summary>
public interface IPokemonCardApiService
{
    /// <summary>
    /// Searches for cards by name using the TCGdex API.
    /// Supports case-insensitive partial name matching.
    /// </summary>
    /// <param name="cardName">The card name or partial name to search for.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of cards matching the search criteria.</returns>
    /// <exception cref="HttpRequestException">Thrown if the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the API response is invalid.</exception>
    Task<IEnumerable<Card>> SearchCardsByNameAsync(string cardName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for a card by its card number (LocalId) within a specific set.
    /// </summary>
    /// <param name="cardNumber">The card number to search for (e.g., "136").</param>
    /// <param name="setId">Optional set identifier to narrow search (e.g., "swsh3").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of cards matching the search criteria.</returns>
    /// <exception cref="HttpRequestException">Thrown if the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the API response is invalid.</exception>
    Task<IEnumerable<Card>> SearchCardsByNumberAsync(string cardNumber, string? setId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches a single card by its API ID from TCGdex API.
    /// </summary>
    /// <param name="apiId">The unique API identifier (e.g., "swsh3-136").</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The card if found; null if not found.</returns>
    /// <exception cref="HttpRequestException">Thrown if the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the API response is invalid.</exception>
    Task<Card?> GetCardByApiIdAsync(string apiId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all cards from a specific set using TCGdex API.
    /// Supports pagination for efficient data retrieval.
    /// </summary>
    /// <param name="setId">The set identifier (e.g., "swsh3").</param>
    /// <param name="pageNumber">The page number (1-indexed) for pagination.</param>
    /// <param name="pageSize">The number of cards per page (max 250 per API limits).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of cards from the set.</returns>
    /// <exception cref="HttpRequestException">Thrown if the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the API response is invalid.</exception>
    Task<IEnumerable<Card>> GetCardsBySetAsync(string setId, int pageNumber = 1, int pageSize = 50, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all available sets from the TCGdex API.
    /// Useful for filtering and discovering available expansions.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An enumerable collection of available sets.</returns>
    /// <exception cref="HttpRequestException">Thrown if the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the API response is invalid.</exception>
    Task<IEnumerable<CardSetApiDto>> GetAvailableSetsAsync(CancellationToken cancellationToken = default);
}
