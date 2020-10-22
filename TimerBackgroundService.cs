using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using aspnetcore_backgroundservice.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetcore_backgroundservice
{
    public class TimerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TimerBackgroundService> _logger;
        private Timer _timer;
        public TimerBackgroundService(IServiceScopeFactory scopeFactory,
                                      ILogger<TimerBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;

        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                _logger.LogInformation("Start calculating store worth");
                var store = _context.Stores.FirstOrDefault();
                store.StoreWorth = _context.StoreItems.Select(i => i.Price * i.Stock).Sum();
                _context.SaveChanges();
                _logger.LogInformation("Done calculating store worth");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer.Dispose();

        }
    }
}