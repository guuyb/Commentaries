using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Commentaries.Api.MvcProblemDetails;

public class CustomValidationProblemDetails : ValidationProblemDetails
{
    [JsonPropertyName("errorMessages")]
    public string[] ErrorMessages { get; set; }

    public CustomValidationProblemDetails(IDictionary<string, string[]> errors, string[] errorMessages)
        : base(errors.ToDictionary(kvp => ToCamelCase(kvp.Key), kvp => kvp.Value))
    {
        ErrorMessages = errorMessages;
    }

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var segments = input
            .Split('.')
            .Where(x => x.Length > 0)
            .Select(x => char.ToLower(x[0]) + x[1..]);

        return string.Join('.', segments);
    }
}
