namespace PokemonCardCollector.Services;

using PokemonCardCollector.Models;
using PokemonCardCollector.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Implements business logic operations for card collection management.
/// Coordinates between the external API service and the repository to provide high-level workflows.
/// Handles validation, duplicate detection, and collection statistics.
/// </summary>
public class CardCollectionService(
    IPokemonCardApiService apiService,
    ICardRepository cardRepository,
    ILogger<CardCollectionService> logger)
    : ICardCollectionService
{
    private readonly IPokemonCardApiService _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
    private readonly ICardRepository _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
    private readonly ILogger<CardCollectionService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Adds a new card to the collection by fetching from API and storing in database.
    /// Validates the card doesn't already exist by checking ApiId.
    /// </summary>
    public async Task<Card> AddCardFromApiAsync(string apiId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiId))
        {
            _logger.LogWarning("Null or empty API ID provided to AddCardFromApiAsync");
            throw new ArgumentException("API ID cannot be null or empty", nameof(apiId));
        }

        try
        {
            _logger.LogInformation("Adding card from API with ID: {ApiId}", apiId);

            // Check if card already exists in collection
            var exists = await _cardRepository.CardExistsAsync(apiId, cancellationToken).ConfigureAwait(false);
            if (exists)
            {
                _logger.LogWarning("Card already exists in collection with API ID: {ApiId}", apiId);
                throw new InvalidOperationException($"Card with API ID '{apiId}' already exists in your collection");
            }

            // Fetch card from external API
            var cardFromApi = await _apiService.GetCardByApiIdAsync(apiId, cancellationToken).ConfigureAwait(false);
            if (cardFromApi is null)
            {
                _logger.LogWarning("Card not found in API for API ID: {ApiId}", apiId);
                throw new InvalidOperationException($"Card with API ID '{apiId}' not found in the external API");
            }

            // Add to database
            var addedCard = await _cardRepository.AddCardAsync(cardFromApi, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Successfully added card to collection: {CardName} (API ID: {ApiId})",
                addedCard.Name, addedCard.ApiId);

            return addedCard;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Add card from API operation cancelled for API ID: {ApiId}", apiId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding card from API with ID: {ApiId}", apiId);
            throw new InvalidOperationException($"Failed to add card with API ID '{apiId}' to collection", ex);
        }
    }

    /// <summary>
    /// Searches the user's local collection by card name.
    /// </summary>
    public async Task<IEnumerable<Card>> SearchLocalCardsAsync(string cardName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cardName))
        {
            _logger.LogWarning("Null or empty card name provided to SearchLocalCardsAsync");
            throw new ArgumentException("Card name cannot be null or empty", nameof(cardName));
        }

        try
        {
            _logger.LogInformation("Searching local collection for cards matching: {CardName}", cardName);

            var results = await _cardRepository.SearchByNameAsync(cardName, cancellationToken).ConfigureAwait(false);
            var resultList = results.ToList();

            _logger.LogInformation("Found {CardCount} cards in local collection matching: {CardName}",
                resultList.Count, cardName);

            return resultList;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Local card search cancelled for name: {CardName}", cardName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching local collection for cards matching: {CardName}", cardName);
            throw new InvalidOperationException($"Failed to search local collection for '{cardName}'", ex);
        }
    }

    /// <summary>
    /// Retrieves all cards in the collection with optional type filtering and pagination.
    /// </summary>
    public async Task<IEnumerable<Card>> GetUserCollectionAsync(
        string? cardType = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user collection - Type: {CardType}, Page: {PageNumber}, Size: {PageSize}",
                cardType ?? "all", pageNumber, pageSize);

            IEnumerable<Card> cards;

            if (!string.IsNullOrWhiteSpace(cardType))
            {
                // Get paginated results for specific type
                var allCardsOfType = await _cardRepository.GetCardsByTypeAsync(cardType, cancellationToken)
                    .ConfigureAwait(false);
                var allList = allCardsOfType.ToList();

                // Apply pagination manually
                cards = allList
                    .OrderByDescending(c => c.DateAdded)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
            else
            {
                // Get all cards with pagination
                cards = await _cardRepository.GetCardsPaginatedAsync(pageNumber, pageSize, cancellationToken)
                    .ConfigureAwait(false);
            }

            var cardList = cards.ToList();
            _logger.LogInformation("Retrieved {CardCount} cards from user collection", cardList.Count);

            return cardList;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Get user collection operation cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user collection");
            throw new InvalidOperationException("Failed to retrieve user collection", ex);
        }
    }

    /// <summary>
    /// Removes a card from the collection by its ID.
    /// </summary>
    public async Task<bool> RemoveCardAsync(int cardId, CancellationToken cancellationToken = default)
    {
        if (cardId <= 0)
        {
            _logger.LogWarning("Invalid card ID provided to RemoveCardAsync: {CardId}", cardId);
            throw new ArgumentException("Card ID must be greater than 0", nameof(cardId));
        }

        try
        {
            _logger.LogInformation("Removing card from collection with ID: {CardId}", cardId);

            var deleted = await _cardRepository.DeleteCardAsync(cardId, cancellationToken).ConfigureAwait(false);

            if (deleted)
            {
                _logger.LogInformation("Successfully removed card with ID: {CardId}", cardId);
            }
            else
            {
                _logger.LogWarning("Card with ID {CardId} not found for deletion", cardId);
            }

            return deleted;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Remove card operation cancelled for ID: {CardId}", cardId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing card with ID: {CardId}", cardId);
            throw new InvalidOperationException($"Failed to remove card with ID {cardId}", ex);
        }
    }

    /// <summary>
    /// Updates a card's condition and notes in the collection.
    /// </summary>
    public async Task<Card> UpdateCardAsync(
        int cardId,
        string? condition = null,
        string? userNotes = null,
        CancellationToken cancellationToken = default)
    {
        if (cardId <= 0)
        {
            _logger.LogWarning("Invalid card ID provided to UpdateCardAsync: {CardId}", cardId);
            throw new ArgumentException("Card ID must be greater than 0", nameof(cardId));
        }

        try
        {
            _logger.LogInformation("Updating card with ID: {CardId}, Condition: {Condition}", cardId, condition ?? "unchanged");

            // Fetch the existing card
            var existingCard = await _cardRepository.GetCardByIdAsync(cardId, cancellationToken).ConfigureAwait(false);
            if (existingCard is null)
            {
                _logger.LogWarning("Card with ID {CardId} not found for update", cardId);
                throw new ArgumentException($"Card with ID {cardId} not found", nameof(cardId));
            }

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(condition))
            {
                existingCard.Condition = condition;
            }

            if (userNotes is not null)
            {
                existingCard.UserNotes = userNotes;
            }

            // Save updates
            var updatedCard = await _cardRepository.UpdateCardAsync(existingCard, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Successfully updated card with ID: {CardId}", cardId);

            return updatedCard;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Update card operation cancelled for ID: {CardId}", cardId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating card with ID: {CardId}", cardId);
            throw new InvalidOperationException($"Failed to update card with ID {cardId}", ex);
        }
    }

    /// <summary>
    /// Retrieves comprehensive statistics about the user's collection.
    /// </summary>
    public async Task<CollectionStatistics> GetCollectionStatisticsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calculating collection statistics");

            // Get all cards for statistics
            var allCards = await _cardRepository.GetAllCardsAsync(cancellationToken).ConfigureAwait(false);
            var cardList = allCards.ToList();

            var stats = new CollectionStatistics
            {
                TotalCards = cardList.Count,
                UniqueSets = cardList.Select(c => c.SetId).Distinct().Count(),
                PokemonCardCount = cardList.OfType<PokemonCard>().Count(),
                TrainerCardCount = cardList.OfType<TrainerCard>().Count(),
                EnergyCardCount = cardList.OfType<EnergyCard>().Count(),
                RareCardCount = cardList.Count(c => c.Rarity != null && c.Rarity.Contains("Rare")),
                HoloCardCount = cardList.Count(c => c.VariantHolo),
                ReverseHoloCardCount = cardList.Count(c => c.VariantReverse),
                FirstEditionCardCount = cardList.Count(c => c.VariantFirstEdition),
                TotalValue = CalculateTotalValue(cardList),
            };

            // Build type breakdown
            stats.CardsByType = cardList
                .Where(c => c is PokemonCard pc && !string.IsNullOrWhiteSpace(pc.Types))
                .GroupBy(c => ((PokemonCard)c).Types)
                .ToDictionary(g => g.Key ?? "Unknown", g => g.Count());

            // Build rarity breakdown
            stats.CardsByRarity = cardList
                .Where(c => !string.IsNullOrWhiteSpace(c.Rarity))
                .GroupBy(c => c.Rarity)
                .ToDictionary(g => g.Key ?? "Unknown", g => g.Count());

            // Build condition breakdown
            stats.CardsByCondition = cardList
                .Where(c => !string.IsNullOrWhiteSpace(c.Condition))
                .GroupBy(c => c.Condition)
                .ToDictionary(g => g.Key ?? "Unknown", g => g.Count());

            _logger.LogInformation("Collection statistics calculated - Total cards: {TotalCards}, Total value: {TotalValue:C}",
                stats.TotalCards, stats.TotalValue);

            return stats;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Get collection statistics operation cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating collection statistics");
            throw new InvalidOperationException("Failed to calculate collection statistics", ex);
        }
    }

    /// <summary>
    /// Searches the external API for cards by name without adding to collection.
    /// </summary>
    public async Task<IEnumerable<Card>> BrowseCardsByNameAsync(string cardName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cardName))
        {
            _logger.LogWarning("Null or empty card name provided to BrowseCardsByNameAsync");
            throw new ArgumentException("Card name cannot be null or empty", nameof(cardName));
        }

        try
        {
            _logger.LogInformation("Browsing external API for cards matching name: {CardName}", cardName);

            var results = await _apiService.SearchCardsByNameAsync(cardName, cancellationToken).ConfigureAwait(false);
            var resultList = results.ToList();

            _logger.LogInformation("Found {CardCount} cards in external API matching: {CardName}",
                resultList.Count, cardName);

            return resultList;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Browse cards by name operation cancelled for: {CardName}", cardName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error browsing external API for cards matching: {CardName}", cardName);
            throw new InvalidOperationException($"Failed to search external API for '{cardName}'", ex);
        }
    }

    /// <summary>
    /// Searches the external API for cards by number with optional set filter.
    /// </summary>
    public async Task<IEnumerable<Card>> BrowseCardsByNumberAsync(
        string cardNumber,
        string? setId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            _logger.LogWarning("Null or empty card number provided to BrowseCardsByNumberAsync");
            throw new ArgumentException("Card number cannot be null or empty", nameof(cardNumber));
        }

        try
        {
            _logger.LogInformation("Browsing external API for card number: {CardNumber}, Set: {SetId}",
                cardNumber, setId ?? "any");

            var results = await _apiService.SearchCardsByNumberAsync(cardNumber, setId, cancellationToken)
                .ConfigureAwait(false);
            var resultList = results.ToList();

            _logger.LogInformation("Found {CardCount} cards in external API with number: {CardNumber}",
                resultList.Count, cardNumber);

            return resultList;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Browse cards by number operation cancelled for: {CardNumber}", cardNumber);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error browsing external API for card number: {CardNumber}", cardNumber);
            throw new InvalidOperationException($"Failed to search external API for card number '{cardNumber}'", ex);
        }
    }

    /// <summary>
    /// Retrieves detailed information about a specific card from the external API.
    /// </summary>
    public async Task<Card?> GetCardDetailsFromApiAsync(string apiId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiId))
        {
            _logger.LogWarning("Null or empty API ID provided to GetCardDetailsFromApiAsync");
            throw new ArgumentException("API ID cannot be null or empty", nameof(apiId));
        }

        try
        {
            _logger.LogInformation("Retrieving card details from API for ID: {ApiId}", apiId);

            var card = await _apiService.GetCardByApiIdAsync(apiId, cancellationToken).ConfigureAwait(false);

            if (card is null)
            {
                _logger.LogInformation("Card details not found in API for ID: {ApiId}", apiId);
            }
            else
            {
                _logger.LogInformation("Retrieved card details from API: {CardName} ({ApiId})", card.Name, apiId);
            }

            return card;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Get card details operation cancelled for API ID: {ApiId}", apiId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving card details from API for ID: {ApiId}", apiId);
            throw new InvalidOperationException($"Failed to retrieve card details for API ID '{apiId}'", ex);
        }
    }

    /// <summary>
    /// Retrieves all available card sets from the external API.
    /// </summary>
    public async Task<IEnumerable<CardSetApiDto>> GetAvailableSetsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving available card sets from API");

            var sets = await _apiService.GetAvailableSetsAsync(cancellationToken).ConfigureAwait(false);
            var setList = sets.ToList();

            _logger.LogInformation("Retrieved {SetCount} card sets from API", setList.Count);

            return setList;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Get available sets operation cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available sets from API");
            throw new InvalidOperationException("Failed to retrieve available sets from API", ex);
        }
    }

    /// <summary>
    /// Calculates the total value of a collection based on estimated card values.
    /// Uses TCGPlayer price (USD) if available, otherwise Cardmarket price (EUR) converted.
    /// </summary>
    private static decimal CalculateTotalValue(IEnumerable<Card> cards)
    {
        return cards.Sum(c => c.EstimatedValue ?? c.TcgPlayerPrice ?? (c.CardmarketPrice ?? 0m));
    }

    /// <summary>
    /// Refreshes pricing data for all cards in the collection by re-fetching from the API.
    /// </summary>
    public async Task<int> RefreshAllCardPricingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting pricing refresh for all cards in collection");

            var allCards = await _cardRepository.GetAllCardsAsync(cancellationToken).ConfigureAwait(false);
            var cardsList = allCards.ToList();
            var updatedCount = 0;

            foreach (var card in cardsList)
            {
                try
                {
                    _logger.LogInformation("Refreshing pricing for card: {CardName} ({ApiId})", card.Name, card.ApiId);

                    // Fetch latest data from API
                    var apiCard = await _apiService.GetCardByApiIdAsync(card.ApiId, cancellationToken).ConfigureAwait(false);

                    if (apiCard is not null)
                    {
                        // Update pricing fields
                        card.TcgPlayerPrice = apiCard.TcgPlayerPrice;
                        card.CardmarketPrice = apiCard.CardmarketPrice;
                        card.Updated = DateTime.UtcNow;

                        await _cardRepository.UpdateCardAsync(card, cancellationToken).ConfigureAwait(false);
                        updatedCount++;

                        _logger.LogInformation("Updated pricing for {CardName}: TCGPlayer={TcgPrice}, Cardmarket={CmPrice}",
                            card.Name, card.TcgPlayerPrice, card.CardmarketPrice);
                    }
                    else
                    {
                        _logger.LogWarning("Could not fetch updated data for card: {ApiId}", card.ApiId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error refreshing pricing for card: {ApiId}", card.ApiId);
                    // Continue with next card even if one fails
                }
            }

            _logger.LogInformation("Pricing refresh complete. Updated {UpdatedCount} of {TotalCount} cards",
                updatedCount, cardsList.Count);

            return updatedCount;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Pricing refresh operation cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during pricing refresh operation");
            throw new InvalidOperationException("Failed to refresh card pricing", ex);
        }
    }
}
