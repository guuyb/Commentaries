using System.Reflection;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db;

public class DbConfigurationsOptions
{
    public DbConfigurationsOptions(Assembly assembly)
    {
        Assembly = assembly;
    }

    public Assembly Assembly { get; private set; }
}
