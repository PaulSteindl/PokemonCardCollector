namespace PokemonCardCollector.Services;

using PokemonCardCollector.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

/// <summary>
/// Implements the Pokemon Card API client for TCGdex v2 API integration.
/// Provides async methods for fetching and mapping Pokémon card data from the v2 REST API.
/// Uses language-specific endpoint (en) and follows C# async best practices with proper error handling and logging.
/// </summary>
public class PokemonCardApiService : IPokemonCardApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PokemonCardApiService> _logger;
    private const string BaseUrl = "https://api.tcgdex.net/v2/en/";
    private const int MaxPageSize = 100; // Recommended page size for v2 API

    /// <summary>
    /// Initializes a new instance of the PokemonCardApiService class.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance for API requests.</param>
    /// <param name="logger">The logger for diagnostic and error messages.</param>
    /// <exception cref="ArgumentNullException">Thrown if httpClient or logger is null.</exception>
    public PokemonCardApiService(
        HttpClient httpClient,
        ILogger<PokemonCardApiService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Configure default headers if not already set
        if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "PokemonCardCollector/1.0");
        }

        // Set base address if not already configured
        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        // Set default timeout if not already configured
        if (_httpClient.Timeout == TimeSpan.FromSeconds(100))
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }
    }

    /// <summary>
    /// Searches for cards by name using the TCGdex v2 API.
    /// Supports case-insensitive partial name matching.
    /// </summary>
    public async Task<IEnumerable<Card>> SearchCardsByNameAsync(
        string cardName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cardName))
        {
            _logger.LogWarning("Empty card name provided for search");
            return Enumerable.Empty<Card>();
        }

        try
        {
            _logger.LogInformation("Searching for cards by name: {CardName}", cardName);

            var requestUrl = $"cards?name={Uri.EscapeDataString(cardName)}";
            _logger.LogInformation("Request URL: {BaseAddress}{RequestUrl}", _httpClient.BaseAddress, requestUrl);

            var response = await _httpClient
                .GetAsync(requestUrl, cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Response status: {StatusCode}", response.StatusCode);
            response.EnsureSuccessStatusCode();

            var apiDtos = await response.Content
                .ReadFromJsonAsync<List<CardBriefApiDto>>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (apiDtos is null || apiDtos.Count == 0)
            {
                _logger.LogInformation("No cards found for name: {CardName}", cardName);
                return Enumerable.Empty<Card>();
            }

            // For brief results, we need to fetch full card details for each
            var fullCards = new List<Card>();
            foreach (var brief in apiDtos)
            {
                var fullCard = await GetCardByApiIdAsync(brief.Id, cancellationToken).ConfigureAwait(false);
                if (fullCard is not null)
                {
                    fullCards.Add(fullCard);
                }
            }

            _logger.LogInformation("Found {CardCount} cards for name: {CardName}", fullCards.Count, cardName);
            return fullCards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card search by name cancelled: {CardName}", cardName);
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API request failed while searching for cards by name: {CardName}", cardName);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse API response for card name search: {CardName}", cardName);
            throw new InvalidOperationException("Failed to parse API response", ex);
        }
    }

    /// <summary>
    /// Searches for a card by its card number (LocalId) within a specific set.
    /// </summary>
    public async Task<IEnumerable<Card>> SearchCardsByNumberAsync(
        string cardNumber,
        string? setId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            _logger.LogWarning("Empty card number provided for search");
            return Enumerable.Empty<Card>();
        }

        try
        {
            _logger.LogInformation("Searching for cards by number: {CardNumber}, Set: {SetId}", cardNumber, setId ?? "any");

            // Build query parameters for v2 API
            var requestUrl = string.IsNullOrWhiteSpace(setId)
                ? $"cards?localId={Uri.EscapeDataString(cardNumber)}"
                : $"cards?localId={Uri.EscapeDataString(cardNumber)}&set.id={Uri.EscapeDataString(setId)}";

            var response = await _httpClient
                .GetAsync(requestUrl, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var apiDtos = await response.Content
                .ReadFromJsonAsync<List<CardBriefApiDto>>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (apiDtos is null || apiDtos.Count == 0)
            {
                _logger.LogInformation("No cards found for number: {CardNumber}, Set: {SetId}", cardNumber, setId ?? "any");
                return Enumerable.Empty<Card>();
            }

            // Fetch full card details for each brief result
            var fullCards = new List<Card>();
            foreach (var brief in apiDtos)
            {
                var fullCard = await GetCardByApiIdAsync(brief.Id, cancellationToken).ConfigureAwait(false);
                if (fullCard is not null)
                {
                    fullCards.Add(fullCard);
                }
            }

            _logger.LogInformation("Found {CardCount} cards for number: {CardNumber}", fullCards.Count, cardNumber);
            return fullCards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card search by number cancelled: {CardNumber}", cardNumber);
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API request failed while searching for cards by number: {CardNumber}", cardNumber);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse API response for card number search: {CardNumber}", cardNumber);
            throw new InvalidOperationException("Failed to parse API response", ex);
        }
    }

    /// <summary>
    /// Fetches a single card by its API ID from TCGdex API.
    /// </summary>
    public async Task<Card?> GetCardByApiIdAsync(
        string apiId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiId))
        {
            _logger.LogWarning("Empty API ID provided");
            return null;
        }

        try
        {
            _logger.LogInformation("Fetching card by API ID: {ApiId}", apiId);

            var response = await _httpClient
                .GetAsync($"cards/{apiId}", cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Card not found for API ID: {ApiId}", apiId);
                    return null;
                }

                response.EnsureSuccessStatusCode();
            }

            var apiDto = await response.Content
                .ReadFromJsonAsync<PokemonCardApiDto>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (apiDto is null)
            {
                _logger.LogWarning("API returned null for API ID: {ApiId}", apiId);
                return null;
            }

            var mappedCard = MapApiDtoToCard(apiDto);
            _logger.LogInformation("Successfully fetched card: {CardName} ({ApiId})", apiDto.Name, apiId);

            return mappedCard;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card fetch cancelled for API ID: {ApiId}", apiId);
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API request failed for API ID: {ApiId}", apiId);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse API response for API ID: {ApiId}", apiId);
            throw new InvalidOperationException("Failed to parse API response", ex);
        }
    }

    /// <summary>
    /// Retrieves all cards from a specific set using TCGdex v2 API.
    /// Supports pagination for efficient data retrieval.
    /// </summary>
    public async Task<IEnumerable<Card>> GetCardsBySetAsync(
        string setId,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(setId))
        {
            _logger.LogWarning("Empty set ID provided");
            return Enumerable.Empty<Card>();
        }

        if (pageNumber < 1)
        {
            _logger.LogWarning("Invalid page number: {PageNumber}, using 1 instead", pageNumber);
            pageNumber = 1;
        }

        // Validate and clamp page size
        var clampedPageSize = Math.Min(Math.Max(pageSize, 1), MaxPageSize);
        if (clampedPageSize != pageSize)
        {
            _logger.LogWarning("Page size {PageSize} adjusted to {ClampedPageSize}", pageSize, clampedPageSize);
        }

        try
        {
            _logger.LogInformation("Fetching cards for set: {SetId}, Page: {PageNumber}, Size: {PageSize}", setId, pageNumber, clampedPageSize);

            // v2 API uses pagination:page and pagination:itemsPerPage query parameters
            var requestUrl = $"cards?set.id={Uri.EscapeDataString(setId)}&pagination:page={pageNumber}&pagination:itemsPerPage={clampedPageSize}";

            var response = await _httpClient
                .GetAsync(requestUrl, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var apiDtos = await response.Content
                .ReadFromJsonAsync<List<PokemonCardApiDto>>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (apiDtos is null || apiDtos.Count == 0)
            {
                _logger.LogInformation("No cards found for set: {SetId}", setId);
                return Enumerable.Empty<Card>();
            }

            var mappedCards = apiDtos.Select(MapApiDtoToCard).Where(c => c is not null).Cast<Card>();
            _logger.LogInformation("Retrieved {CardCount} cards for set: {SetId}", apiDtos.Count, setId);

            return mappedCards;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card retrieval cancelled for set: {SetId}", setId);
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API request failed for set: {SetId}", setId);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse API response for set: {SetId}", setId);
            throw new InvalidOperationException("Failed to parse API response", ex);
        }
    }

    /// <summary>
    /// Retrieves all available sets from the TCGdex API.
    /// Useful for filtering and discovering available expansions.
    /// </summary>
    public async Task<IEnumerable<CardSetApiDto>> GetAvailableSetsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching available sets from API");

            var response = await _httpClient
                .GetAsync("sets", cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var sets = await response.Content
                .ReadFromJsonAsync<List<CardSetApiDto>>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (sets is null || sets.Count == 0)
            {
                _logger.LogWarning("No sets returned from API");
                return Enumerable.Empty<CardSetApiDto>();
            }

            _logger.LogInformation("Retrieved {SetCount} sets from API", sets.Count);
            return sets;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Set retrieval cancelled");
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API request failed while fetching sets");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse API response for sets");
            throw new InvalidOperationException("Failed to parse API response", ex);
        }
    }

    /// <summary>
    /// Maps a PokemonCardApiDto from the external API to a Card domain entity.
    /// Handles null values gracefully and selects appropriate card type based on category.
    /// </summary>
    private Card? MapApiDtoToCard(PokemonCardApiDto apiDto)
    {
        try
        {
            if (apiDto is null || string.IsNullOrWhiteSpace(apiDto.Id) || string.IsNullOrWhiteSpace(apiDto.Name))
            {
                _logger.LogWarning("Invalid API DTO: missing required fields");
                return null;
            }

            // Determine card category and create appropriate derived type
            var cardCategory = apiDto.Category?.ToLowerInvariant() ?? "pokemon";
            var setId = apiDto.Set?.Id ?? "unknown";
            var setName = apiDto.Set?.Name ?? "Unknown Set";

            Card card = cardCategory switch
            {
                "pokemon" => new PokemonCard
                {
                    ApiId = apiDto.Id,
                    LocalId = apiDto.LocalId ?? "",
                    Name = apiDto.Name,
                    ImageUrl = apiDto.Image,
                    Illustrator = apiDto.Illustrator,
                    SetId = setId,
                    SetName = setName,
                    Rarity = apiDto.Rarity,
                    VariantNormal = apiDto.Variants?.Normal ?? false,
                    VariantReverse = apiDto.Variants?.Reverse ?? false,
                    VariantHolo = apiDto.Variants?.Holo ?? false,
                    VariantFirstEdition = apiDto.Variants?.FirstEdition ?? false,
                    TcgPlayerPrice = apiDto.Pricing?.Tcgplayer?.Normal?.MarketPrice,
                    CardmarketPrice = apiDto.Pricing?.Cardmarket?.Avg,
                    Updated = apiDto.Updated ?? DateTime.UtcNow,
                    DateAdded = DateTime.UtcNow,
                    DexId = apiDto.DexId is not null ? string.Join(",", apiDto.DexId) : null,
                    Hp = apiDto.Hp,
                    Types = apiDto.Types is not null ? string.Join(",", apiDto.Types) : null,
                    EvolveFrom = apiDto.EvolveFrom,
                    Description = apiDto.Description,
                    Stage = apiDto.Stage,
                },
                "trainer" => new TrainerCard
                {
                    ApiId = apiDto.Id,
                    LocalId = apiDto.LocalId ?? "",
                    Name = apiDto.Name,
                    ImageUrl = apiDto.Image,
                    Illustrator = apiDto.Illustrator,
                    SetId = setId,
                    SetName = setName,
                    Rarity = apiDto.Rarity,
                    VariantNormal = apiDto.Variants?.Normal ?? false,
                    VariantReverse = apiDto.Variants?.Reverse ?? false,
                    VariantHolo = apiDto.Variants?.Holo ?? false,
                    VariantFirstEdition = apiDto.Variants?.FirstEdition ?? false,
                    TcgPlayerPrice = apiDto.Pricing?.Tcgplayer?.Normal?.MarketPrice,
                    CardmarketPrice = apiDto.Pricing?.Cardmarket?.Avg,
                    Updated = apiDto.Updated ?? DateTime.UtcNow,
                    DateAdded = DateTime.UtcNow,
                    Effect = apiDto.Effect,
                    TrainerType = apiDto.TrainerType ?? "",
                },
                "energy" => new EnergyCard
                {
                    ApiId = apiDto.Id,
                    LocalId = apiDto.LocalId ?? "",
                    Name = apiDto.Name,
                    ImageUrl = apiDto.Image,
                    Illustrator = apiDto.Illustrator,
                    SetId = setId,
                    SetName = setName,
                    Rarity = apiDto.Rarity,
                    VariantNormal = apiDto.Variants?.Normal ?? false,
                    VariantReverse = apiDto.Variants?.Reverse ?? false,
                    VariantHolo = apiDto.Variants?.Holo ?? false,
                    VariantFirstEdition = apiDto.Variants?.FirstEdition ?? false,
                    TcgPlayerPrice = apiDto.Pricing?.Tcgplayer?.Normal?.MarketPrice,
                    CardmarketPrice = apiDto.Pricing?.Cardmarket?.Avg,
                    Updated = apiDto.Updated ?? DateTime.UtcNow,
                    DateAdded = DateTime.UtcNow,
                    EnergyType = apiDto.EnergyType ?? "",
                },
                _ => new PokemonCard  // Default to PokemonCard for unknown categories
                {
                    ApiId = apiDto.Id,
                    LocalId = apiDto.LocalId ?? "",
                    Name = apiDto.Name,
                    ImageUrl = apiDto.Image,
                    Illustrator = apiDto.Illustrator,
                    SetId = setId,
                    SetName = setName,
                    Rarity = apiDto.Rarity,
                    VariantNormal = apiDto.Variants?.Normal ?? false,
                    VariantReverse = apiDto.Variants?.Reverse ?? false,
                    VariantHolo = apiDto.Variants?.Holo ?? false,
                    VariantFirstEdition = apiDto.Variants?.FirstEdition ?? false,
                    TcgPlayerPrice = apiDto.Pricing?.Tcgplayer?.Normal?.MarketPrice,
                    CardmarketPrice = apiDto.Pricing?.Cardmarket?.Avg,
                    Updated = apiDto.Updated ?? DateTime.UtcNow,
                    DateAdded = DateTime.UtcNow,
                }
            };

            _logger.LogInformation("Mapped API DTO to {CardType}: {CardName}", card.GetType().Name, card.Name);
            return card;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error mapping API DTO to Card entity");
            return null;
        }
    }
}
