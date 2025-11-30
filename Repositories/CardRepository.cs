namespace PokemonCardCollector.Repositories;

using PokemonCardCollector.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Implements the card repository for data access operations.
/// Provides async CRUD methods following the repository pattern and C# async best practices.
/// </summary>
public class CardRepository(
    PokemonCardDbContext context,
    ILogger<CardRepository> logger)
    : ICardRepository
{
    private readonly PokemonCardDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<CardRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Retrieves a card by its unique database identifier.
    /// </summary>
    public async Task<Card?> GetCardByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid card ID requested: {CardId}", id);
            return null;
        }

        try
        {
            _logger.LogInformation("Retrieving card with ID {CardId}", id);

            var card = await _context.Cards
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
                .ConfigureAwait(false);

            if (card is null)
                _logger.LogInformation("Card with ID {CardId} not found", id);

            return card;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card retrieval cancelled");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error retrieving card with ID {CardId}", id);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all cards in the database.
    /// </summary>
    public async Task<IEnumerable<Card>> GetAllCardsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all cards from database");

            var cards = await _context.Cards
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {CardCount} cards from database", cards.Count);
            return cards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card retrieval cancelled");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error retrieving all cards");
            throw;
        }
    }

    /// <summary>
    /// Searches for cards by name using case-insensitive partial matching.
    /// </summary>
    public async Task<IEnumerable<Card>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _logger.LogWarning("Empty card name provided for search");
            return Enumerable.Empty<Card>();
        }

        try
        {
            _logger.LogInformation("Searching for cards by name: {CardName}", name);

            var cards = await _context.Cards
                .AsNoTracking()
                .Where(c => c.Name.Contains(name))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Found {CardCount} cards matching name: {CardName}", cards.Count, name);
            return cards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card search by name cancelled: {CardName}", name);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error searching for cards by name: {CardName}", name);
            throw;
        }
    }

    /// <summary>
    /// Searches for a card by its card number (LocalId) within a set.
    /// </summary>
    public async Task<IEnumerable<Card>> SearchByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            _logger.LogWarning("Empty card number provided for search");
            return Enumerable.Empty<Card>();
        }

        try
        {
            _logger.LogInformation("Searching for cards by number: {CardNumber}", cardNumber);

            var cards = await _context.Cards
                .AsNoTracking()
                .Where(c => c.LocalId == cardNumber)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Found {CardCount} cards with number: {CardNumber}", cards.Count, cardNumber);
            return cards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card search by number cancelled: {CardNumber}", cardNumber);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error searching for cards by number: {CardNumber}", cardNumber);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all cards of a specific type (PokemonCard, TrainerCard, or EnergyCard).
    /// </summary>
    public async Task<IEnumerable<Card>> GetCardsByTypeAsync(string cardType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cardType))
        {
            _logger.LogWarning("Empty card type provided");
            return Enumerable.Empty<Card>();
        }

        try
        {
            _logger.LogInformation("Retrieving cards of type: {CardType}", cardType);

            var cards = await _context.Cards
                .AsNoTracking()
                .OfType<Card>()
                .Where(c => c.GetType().Name == cardType)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Found {CardCount} cards of type: {CardType}", cards.Count, cardType);
            return cards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card retrieval by type cancelled: {CardType}", cardType);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error retrieving cards by type: {CardType}", cardType);
            throw;
        }
    }

    /// <summary>
    /// Checks if a card with the given API ID already exists in the database.
    /// </summary>
    public async Task<bool> CardExistsAsync(string apiId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiId))
        {
            _logger.LogWarning("Empty API ID provided for existence check");
            return false;
        }

        try
        {
            _logger.LogInformation("Checking if card exists with API ID: {ApiId}", apiId);

            var exists = await _context.Cards
                .AsNoTracking()
                .AnyAsync(c => c.ApiId == apiId, cancellationToken)
                .ConfigureAwait(false);

            if (exists)
                _logger.LogWarning("Card already exists with API ID: {ApiId}", apiId);

            return exists;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card existence check cancelled");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error checking card existence for API ID: {ApiId}", apiId);
            throw;
        }
    }

    /// <summary>
    /// Adds a new card to the database.
    /// Validates that the card does not already exist (by ApiId) before inserting.
    /// </summary>
    public async Task<Card> AddCardAsync(Card card, CancellationToken cancellationToken = default)
    {
        if (card is null)
        {
            _logger.LogError("Null card provided to AddCardAsync");
            throw new ArgumentNullException(nameof(card));
        }

        if (string.IsNullOrWhiteSpace(card.ApiId))
        {
            _logger.LogError("Card with null or empty ApiId cannot be added");
            throw new ArgumentException("Card must have a valid ApiId", nameof(card));
        }

        try
        {
            _logger.LogInformation("Adding card: {CardName} (API ID: {ApiId})", card.Name, card.ApiId);

            if (await CardExistsAsync(card.ApiId, cancellationToken).ConfigureAwait(false))
            {
                _logger.LogError("Cannot add card - duplicate API ID: {ApiId}", card.ApiId);
                throw new InvalidOperationException($"A card with API ID '{card.ApiId}' already exists in the database");
            }

            _context.Cards.Add(card);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully added card with ID {CardId}: {CardName}", card.Id, card.Name);
            return card;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card addition cancelled for API ID: {ApiId}", card.ApiId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error adding card: {CardName}", card.Name);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing card in the database.
    /// </summary>
    public async Task<Card> UpdateCardAsync(Card card, CancellationToken cancellationToken = default)
    {
        if (card is null)
        {
            _logger.LogError("Null card provided to UpdateCardAsync");
            throw new ArgumentNullException(nameof(card));
        }

        if (card.Id <= 0)
        {
            _logger.LogError("Cannot update card with invalid ID: {CardId}", card.Id);
            throw new ArgumentException("Card must have a valid ID", nameof(card));
        }

        try
        {
            _logger.LogInformation("Updating card with ID {CardId}: {CardName}", card.Id, card.Name);

            var existingCard = await _context.Cards.FindAsync(new object[] { card.Id }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (existingCard is null)
            {
                _logger.LogError("Card with ID {CardId} not found for update", card.Id);
                throw new ArgumentException($"Card with ID {card.Id} not found", nameof(card));
            }

            _context.Entry(existingCard).CurrentValues.SetValues(card);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully updated card with ID {CardId}: {CardName}", card.Id, card.Name);
            return card;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card update cancelled for ID: {CardId}", card.Id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error updating card with ID {CardId}", card.Id);
            throw;
        }
    }

    /// <summary>
    /// Deletes a card from the database by its ID.
    /// </summary>
    public async Task<bool> DeleteCardAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid card ID provided for deletion: {CardId}", id);
            return false;
        }

        try
        {
            _logger.LogInformation("Deleting card with ID {CardId}", id);

            var card = await _context.Cards.FindAsync(new object[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (card is null)
            {
                _logger.LogInformation("Card with ID {CardId} not found for deletion", id);
                return false;
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully deleted card with ID {CardId}", id);
            return true;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card deletion cancelled for ID: {CardId}", id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error deleting card with ID {CardId}", id);
            throw;
        }
    }

    /// <summary>
    /// Retrieves cards with pagination support for efficient data loading.
    /// </summary>
    public async Task<IEnumerable<Card>> GetCardsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
        {
            _logger.LogWarning("Invalid page number: {PageNumber}, using 1 instead", pageNumber);
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            _logger.LogWarning("Invalid page size: {PageSize}, using 10 instead", pageSize);
            pageSize = 10;
        }

        try
        {
            _logger.LogInformation("Retrieving paginated cards - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            var skip = (pageNumber - 1) * pageSize;
            var cards = await _context.Cards
                .AsNoTracking()
                .OrderByDescending(c => c.DateAdded)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {CardCount} cards from page {PageNumber}", cards.Count, pageNumber);
            return cards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Paginated card retrieval cancelled");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error retrieving paginated cards");
            throw;
        }
    }

    /// <summary>
    /// Gets the total count of cards in the database.
    /// Useful for pagination and statistics.
    /// </summary>
    public async Task<int> GetCardCountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting total card count");

            var count = await _context.Cards
                .AsNoTracking()
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Total cards in database: {CardCount}", count);
            return count;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card count retrieval cancelled");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error getting card count");
            throw;
        }
    }
}
