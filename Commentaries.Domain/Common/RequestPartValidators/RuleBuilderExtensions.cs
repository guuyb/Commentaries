global using static Commentaries.Domain.Common.RequestPartValidators.RuleBuilderExtensions;
using FluentValidation;
using FluentValidation.Results;

namespace Commentaries.Domain.Common.RequestPartValidators;

internal static class RuleBuilderExtensions
{
    public static IRuleBuilderOptionsConditions<T, string?> MatchObjectTypeFullName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Custom((fullName, context) =>
        {
            if (fullName is not null && fullName.IndexOf('.') < 0)
            {
                var failure = new ValidationFailure(context.PropertyName, "Требуется полное наименование типа объекта (FullName)")
                {
                    ErrorCode = "ObjectTypeFullNameValidator"
                };
                context.AddFailure(failure);
            }
        });
    }
}
