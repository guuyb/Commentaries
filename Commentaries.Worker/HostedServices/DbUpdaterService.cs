using Microsoft.EntityFrameworkCore;

namespace Commentaries.Worker.HostedServices
{
    public class DbUpdaterService<T> : BackgroundService where T : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public DbUpdaterService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<T>();

                dbContext.Database.Migrate();

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
