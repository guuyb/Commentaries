using Commentaries.Domain.Common.Behaviour;
using Commentaries.Domain.Common.RequestPartValidators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace Commentaries.Domain.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services,
        IConfiguration configuration)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
        ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
        {
            if (member != null)
            {
                return member.Name;
            }
            return null;
        };

        services.AddValidatorsFromAssemblyContaining<ObjectIdValidator>(includeInternalTypes: true);

        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

        return services;
    }
}
