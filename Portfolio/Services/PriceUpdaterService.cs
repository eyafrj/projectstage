using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Services
{
    public class PriceUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PriceUpdaterService> _logger;

        public PriceUpdaterService(IServiceProvider serviceProvider, ILogger<PriceUpdaterService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Update prices logic
                    await UpdatePricesAsync(context);

                    _logger.LogInformation("Prices updated successfully.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task UpdatePricesAsync(ApplicationDbContext context)
        {
            // Fetch all securities from the database
            var securities = await context.securities.ToListAsync();

            // Random number generator
            var random = new Random();

            foreach (var security in securities)
            {
                // Generate a random percentage change between -40% and +40%
                var percentageChange = (decimal)(random.NextDouble() * 0.80 - 0.40);

                // Update the current price
                security.currentPrice += security.currentPrice * percentageChange;

                // Ensure the price is not negative
                if (security.currentPrice < 0)
                {
                    security.currentPrice = 0;
                }
            }

            // Save changes to the database
            await context.SaveChangesAsync();
        }
    }
}
