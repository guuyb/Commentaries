using Commentaries.Domain.Common.RequestPartValidators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Commentaries.Domain.Test;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ObjectIdValidator>(includeInternalTypes: true);
    }
}
