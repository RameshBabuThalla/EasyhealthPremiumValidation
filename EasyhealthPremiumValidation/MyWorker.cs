using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace HERGPremiumValidationSchedular
{
    public class MyWorker : BackgroundService
    {
        private readonly ILogger<MyWorker> _logger;

        public MyWorker(ILogger<MyWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Working...");
                await Task.Delay(1000, stoppingToken);  // Simulate some work
            }

            _logger.LogInformation("Worker stopped");
        }
    }
}
