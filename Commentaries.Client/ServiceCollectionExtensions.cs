using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Commentaries.Client.Test")]

namespace Commentaries.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommentariesClient(this IServiceCollection services, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new System.ArgumentException($"'{nameof(baseUrl)}' cannot be null or whitespace.", nameof(baseUrl));
        }

        services
            .AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>()
            .AddTransient<ICommentariesClient, CommentariesClient>()
            .AddSingleton(new CommentariesApiConfig
            {
                BaseUrl = baseUrl,
            });

        return services;
    }
}
