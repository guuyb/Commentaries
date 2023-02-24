using Commentaries.Application.Common.RequestPartValidators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Commentaries.Application.Test;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ObjectIdValidator>(includeInternalTypes: true);
    }
}
