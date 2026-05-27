using System.Text.Json;

namespace HotelJJ.DataAccess.Http.Common;

public static class DownstreamErrorMessageExtractor
{
    private static readonly string[] MessagePropertyNames =
    [
        "message",
        "mensaje",
        "detail",
        "title",
        "error",
        "descripcion"
    ];

    public static string BuildMessage(string serviceName, HttpResponseMessage response, string? body)
    {
        var extractedMessage = Extract(body);
        return string.IsNullOrWhiteSpace(extractedMessage)
            ? $"El microservicio {serviceName} respondio {(int)response.StatusCode} ({response.ReasonPhrase})."
            : extractedMessage;
    }

    public static string? Extract(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(body);
            return Extract(document.RootElement);
        }
        catch (JsonException)
        {
            return body.Trim();
        }
    }

    private static string? Extract(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.String)
        {
            return root.GetString();
        }

        if (root.ValueKind is not JsonValueKind.Object)
        {
            return null;
        }

        var validationErrors = ExtractValidationErrors(root);
        if (!string.IsNullOrWhiteSpace(validationErrors))
        {
            var title = TryGetString(root, "title");
            return string.IsNullOrWhiteSpace(title)
                ? validationErrors
                : $"{title}: {validationErrors}";
        }

        foreach (var propertyName in MessagePropertyNames)
        {
            var message = TryGetString(root, propertyName);
            if (!string.IsNullOrWhiteSpace(message))
            {
                return message;
            }
        }

        return null;
    }

    private static string? ExtractValidationErrors(JsonElement root)
    {
        if (!root.TryGetProperty("errors", out var errors) || errors.ValueKind is not JsonValueKind.Object)
        {
            return null;
        }

        var messages = new List<string>();
        foreach (var property in errors.EnumerateObject())
        {
            if (property.Value.ValueKind == JsonValueKind.Array)
            {
                messages.AddRange(
                    property.Value
                        .EnumerateArray()
                        .Where(item => item.ValueKind == JsonValueKind.String)
                        .Select(item => item.GetString())
                        .Where(message => !string.IsNullOrWhiteSpace(message))!);
            }
            else if (property.Value.ValueKind == JsonValueKind.String)
            {
                var message = property.Value.GetString();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    messages.Add(message);
                }
            }
        }

        return messages.Count == 0
            ? null
            : string.Join(" ", messages.Distinct());
    }

    private static string? TryGetString(JsonElement root, string propertyName)
    {
        return root.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;
    }
}
