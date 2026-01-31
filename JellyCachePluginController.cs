using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jellyfin.Api.Controllers;
using MediaBrowser.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JellyCachePlugin
{
    [ApiController]
    [Route("/Plugins/JellyCachePlugin")]
    public class JellyCachePluginController : ControllerBase
    {
        private readonly ILogger<JellyCachePluginController> _logger;
        private readonly IServerApplicationHost _applicationHost;
        private readonly Services.CleanupService _cleanupService;

        public JellyCachePluginController(
            ILogger<JellyCachePluginController> logger,
            IServerApplicationHost applicationHost,
            Services.CleanupService cleanupService)
        {
            _logger = logger;
            _applicationHost = applicationHost;
            _cleanupService = cleanupService;
        }

        [HttpGet("CacheData")]
        public IActionResult GetCacheData()
        {
            var plugin = _applicationHost.Plugins.FirstOrDefault(p => p is Plugin) as Plugin;
            if (plugin == null) return NotFound();

            var data = new List<object>();
            foreach (var kvp in plugin.Configuration.CacheDirectories)
            {
                var config = kvp.Value;
                long currentSize = 0;
                string status = "OK";

                if (Directory.Exists(config.Path))
                {
                    var dirInfo = new DirectoryInfo(config.Path);
                    currentSize = dirInfo.GetFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
                    long maxSizeBytes = config.MaxSizeMB * 1024 * 1024;
                    if (currentSize > maxSizeBytes * plugin.Configuration.AlertThresholdPercent / 100)
                    {
                        status = "Warning";
                    }
                }
                else
                {
                    status = "Directory not found";
                }

                data.Add(new
                {
                    category = kvp.Key,
                    path = config.Path,
                    currentSize = $"{currentSize / (1024 * 1024)} MB",
                    maxSize = $"{config.MaxSizeMB} MB",
                    status
                });
            }

            return Ok(data);
        }

        [HttpGet("Config")]
        public IActionResult GetConfig()
        {
            var plugin = _applicationHost.Plugins.FirstOrDefault(p => p is Plugin) as Plugin;
            if (plugin == null) return NotFound();

            return Ok(plugin.Configuration);
        }

        [HttpPost("Config")]
        public IActionResult UpdateConfig([FromBody] PluginConfiguration config)
        {
            var plugin = _applicationHost.Plugins.FirstOrDefault(p => p is Plugin) as Plugin;
            if (plugin == null) return NotFound();

            plugin.Configuration.CleanupIntervalHours = config.CleanupIntervalHours;
            plugin.Configuration.EnableAlerts = config.EnableAlerts;
            plugin.Configuration.AlertThresholdPercent = config.AlertThresholdPercent;

            plugin.UpdateConfiguration(plugin.Configuration);

            return Ok();
        }

        [HttpPost("ClearAll")]
        public async Task<IActionResult> ClearAllCaches()
        {
            await _cleanupService.ClearAllCaches();
            return Ok();
        }

        [HttpPost("Clear/{category}")]
        public async Task<IActionResult> ClearCache(string category)
        {
            await _cleanupService.ClearCache(category);
            return Ok();
        }

        [HttpPost("RebuildImageCache")]
        public IActionResult RebuildImageCache()
        {
            // Implement rebuild logic if needed
            _logger.LogInformation("Image cache rebuild requested");
            return Ok();
        }

        [HttpGet("Logs")]
        public IActionResult GetLogs()
        {
            // Simple log retrieval, in practice use proper logging
            return Ok("Logs would be here");
        }
    }
}