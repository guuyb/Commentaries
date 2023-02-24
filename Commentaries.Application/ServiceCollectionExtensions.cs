using Commentaries.Application.Common.Behaviour;
using Commentaries.Application.Common.RequestPartValidators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Commentaries.Application.Test")]

namespace Commentaries.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

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
