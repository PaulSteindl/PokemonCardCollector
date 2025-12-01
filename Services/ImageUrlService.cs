namespace PokemonCardCollector.Services;

/// <summary>
/// Implementation of image URL formatting service for TCGdex assets.
/// Converts base URLs from the API into properly formatted URLs with quality and extension parameters.
/// </summary>
public class ImageUrlService : IImageUrlService
{
    private static readonly HashSet<string> ValidQualities = ["high", "low"];
    private static readonly HashSet<string> ValidExtensions = ["webp", "png", "jpg"];

    /// <inheritdoc />
    public string? FormatCardImageUrl(string? baseUrl, string quality = "low", string extension = "webp")
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return null;
        }

        // Validate quality parameter
        if (!ValidQualities.Contains(quality))
        {
            throw new ArgumentException($"Invalid quality '{quality}'. Must be 'high' or 'low'.", nameof(quality));
        }

        // Validate extension parameter
        if (!ValidExtensions.Contains(extension))
        {
            throw new ArgumentException($"Invalid extension '{extension}'. Must be 'webp', 'png', or 'jpg'.", nameof(extension));
        }

        // Remove trailing slash if present
        baseUrl = baseUrl.TrimEnd('/');

        // Format: {baseUrl}/{quality}.{extension}
        return $"{baseUrl}/{quality}.{extension}";
    }

    /// <inheritdoc />
    public string? FormatSetSymbolUrl(string? baseUrl, string extension = "webp")
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return null;
        }

        // Validate extension parameter
        if (!ValidExtensions.Contains(extension))
        {
            throw new ArgumentException($"Invalid extension '{extension}'. Must be 'webp', 'png', or 'jpg'.", nameof(extension));
        }

        // Remove trailing slash if present
        baseUrl = baseUrl.TrimEnd('/');

        // Format: {baseUrl}.{extension}
        return $"{baseUrl}.{extension}";
    }

    /// <inheritdoc />
    public string? FormatSetLogoUrl(string? baseUrl, string extension = "webp")
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return null;
        }

        // Validate extension parameter
        if (!ValidExtensions.Contains(extension))
        {
            throw new ArgumentException($"Invalid extension '{extension}'. Must be 'webp', 'png', or 'jpg'.", nameof(extension));
        }

        // Remove trailing slash if present
        baseUrl = baseUrl.TrimEnd('/');

        // Format: {baseUrl}.{extension}
        return $"{baseUrl}.{extension}";
    }
}
