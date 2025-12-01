namespace PokemonCardCollector.Models.JsonConverters;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Custom JSON converter for attack damage values that handles both numeric and string representations.
/// The TCGdex API returns damage as either a number (e.g., 50) or a string (e.g., "50+", "×").
/// This converter normalizes both formats into a string representation.
/// </summary>
public class DamageJsonConverter : JsonConverter<string?>
{
    /// <summary>
    /// Reads and converts the JSON to a string, handling both number and string token types.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The type to convert to (string).</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The damage value as a string, or null if the token is null.</returns>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => reader.GetInt32().ToString(),
            _ => throw new JsonException($"Unexpected token type '{reader.TokenType}' for damage value. Expected String or Number.")
        };
    }

    /// <summary>
    /// Writes the string value to JSON.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value);
        }
    }
}
