namespace PokemonCardCollector.Services;

/// <summary>
/// Service for formatting TCGdex asset URLs with proper quality and extension parameters.
/// </summary>
public interface IImageUrlService
{
    /// <summary>
    /// Formats a card image URL with the specified quality and extension.
    /// </summary>
    /// <param name="baseUrl">The base URL from the API (e.g., "https://assets.tcgdex.net/en/swsh/swsh3/136").</param>
    /// <param name="quality">The image quality: "high" (600x825) or "low" (245x337). Default is "low".</param>
    /// <param name="extension">The image extension: "webp" (recommended), "png", or "jpg". Default is "webp".</param>
    /// <returns>The formatted image URL, or null if the base URL is null or empty.</returns>
    string? FormatCardImageUrl(string? baseUrl, string quality = "low", string extension = "webp");

    /// <summary>
    /// Formats a set symbol URL with the specified extension.
    /// </summary>
    /// <param name="baseUrl">The base URL from the API.</param>
    /// <param name="extension">The image extension: "webp" (recommended), "png", or "jpg". Default is "webp".</param>
    /// <returns>The formatted symbol URL, or null if the base URL is null or empty.</returns>
    string? FormatSetSymbolUrl(string? baseUrl, string extension = "webp");

    /// <summary>
    /// Formats a set logo URL with the specified extension.
    /// </summary>
    /// <param name="baseUrl">The base URL from the API.</param>
    /// <param name="extension">The image extension: "webp" (recommended), "png", or "jpg". Default is "webp".</param>
    /// <returns>The formatted logo URL, or null if the base URL is null or empty.</returns>
    string? FormatSetLogoUrl(string? baseUrl, string extension = "webp");
}
