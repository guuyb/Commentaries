global using static Commentaries.Application.Common.Extensions.ValidatorExtentions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Common.Extensions;

public static class ValidatorExtentions
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exceptions.ValidationException"></exception>
    public static async Task ValidateOrThrowExceptionAsync<T>(this IValidator<T> validator, T instance,
        CancellationToken cancellation)
    {
        if (validator is null)
        {
            throw new ArgumentNullException(nameof(validator));
        }

        var validationResult = await validator.ValidateAsync(instance, cancellation);
        if (validationResult.IsValid)
            return;

        var errors = new Dictionary<string, List<string>>();
        validationResult.Errors.Aggregate(errors,
            (errors, error) =>
            {
                AddError(errors, error.PropertyName, error.ErrorMessage);
                return errors;
            });

        throw new Exceptions.ValidationException(errors.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray()));
    }

    private static void AddError(
            IDictionary<string, List<string>> errors,
            string key,
            string value)
    {
        if (errors is null)
            throw new ArgumentNullException(nameof(errors));

        if (key is null)
            throw new ArgumentNullException(nameof(key));

        if (errors.ContainsKey(key))
        {
            errors[key].Add(value);
        }
        else
        {
            errors.Add(key, new List<string> { value });
        }
    }
}
