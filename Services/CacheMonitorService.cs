using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JellyCachePlugin.Services
{
    public class CacheMonitorService : IHostedService, IDisposable
    {
        private readonly ILogger<CacheMonitorService> _logger;
        private readonly IServerApplicationHost _applicationHost;
        private readonly CleanupService _cleanupService;
        private Timer _timer;

        public CacheMonitorService(
            ILogger<CacheMonitorService> logger,
            IServerApplicationHost applicationHost,
            CleanupService cleanupService)
        {
            _logger = logger;
            _applicationHost = applicationHost;
            _cleanupService = cleanupService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Cache Monitor Service");

            var plugin = _applicationHost.Plugins.FirstOrDefault(p => p is Plugin) as Plugin;
            if (plugin == null) return Task.CompletedTask;

            var config = plugin.Configuration;
            var interval = TimeSpan.FromHours(config.CleanupIntervalHours);

            _timer = new Timer(DoWork, null, TimeSpan.Zero, interval);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Running scheduled cache cleanup");
            _cleanupService.PerformCleanup().Wait();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Cache Monitor Service");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}