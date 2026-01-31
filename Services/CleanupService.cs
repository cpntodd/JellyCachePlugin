using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Controller;
using Microsoft.Extensions.Logging;

namespace JellyCachePlugin.Services
{
    public class CleanupService
    {
        private readonly ILogger<CleanupService> _logger;
        private readonly IServerApplicationHost _applicationHost;

        public CleanupService(
            ILogger<CleanupService> logger,
            IServerApplicationHost applicationHost)
        {
            _logger = logger;
            _applicationHost = applicationHost;
        }

        public async Task PerformCleanup()
        {
            var plugin = _applicationHost.Plugins.FirstOrDefault(p => p is Plugin) as Plugin;
            if (plugin == null) return;

            var config = plugin.Configuration;

            foreach (var kvp in config.CacheDirectories)
            {
                var dirName = kvp.Key;
                var dirConfig = kvp.Value;

                try
                {
                    await CleanupDirectory(dirName, dirConfig);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error cleaning up {dirName}");
                }
            }
        }

        private async Task CleanupDirectory(string name, CacheDirectoryConfig config)
        {
            if (!Directory.Exists(config.Path))
            {
                _logger.LogWarning($"Directory {config.Path} does not exist");
                return;
            }

            var dirInfo = new DirectoryInfo(config.Path);
            var files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            // Filter by age
            var cutoffDate = DateTime.Now.AddDays(-config.MaxAgeDays);
            var oldFiles = files.Where(f => f.LastWriteTime < cutoffDate).ToList();

            // Filter by type if specified
            if (config.FileTypeFilters.Any())
            {
                oldFiles = oldFiles.Where(f => config.FileTypeFilters.Contains(f.Extension.ToLower())).ToList();
            }

            // Calculate current size
            long currentSize = files.Sum(f => f.Length);
            long maxSizeBytes = config.MaxSizeMB * 1024 * 1024;

            // If over size, delete oldest files first
            if (currentSize > maxSizeBytes)
            {
                var filesToDelete = files.OrderBy(f => f.LastWriteTime).ToList();
                long sizeToDelete = currentSize - maxSizeBytes;

                foreach (var file in filesToDelete)
                {
                    if (sizeToDelete <= 0) break;

                    try
                    {
                        // Skip active transcodes (simple check: if file is being written to)
                        if (IsFileLocked(file)) continue;

                        long fileSize = file.Length;
                        file.Delete();
                        sizeToDelete -= fileSize;
                        _logger.LogInformation($"Deleted {file.FullName} to free space");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to delete {file.FullName}");
                    }
                }
            }

            // Delete old files
            foreach (var file in oldFiles)
            {
                try
                {
                    if (IsFileLocked(file)) continue;
                    file.Delete();
                    _logger.LogInformation($"Deleted old file {file.FullName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to delete old file {file.FullName}");
                }
            }
        }

        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }

        public async Task ClearAllCaches()
        {
            var plugin = _applicationHost.Plugins.FirstOrDefault(p => p is Plugin) as Plugin;
            if (plugin == null) return;

            foreach (var kvp in plugin.Configuration.CacheDirectories)
            {
                await ClearCache(kvp.Key);
            }
        }

        public async Task ClearCache(string category)
        {
            var plugin = _applicationHost.Plugins.FirstOrDefault(p => p is Plugin) as Plugin;
            if (plugin == null || !plugin.Configuration.CacheDirectories.ContainsKey(category)) return;

            var config = plugin.Configuration.CacheDirectories[category];
            if (!Directory.Exists(config.Path)) return;

            var dirInfo = new DirectoryInfo(config.Path);
            var files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    if (IsFileLocked(file)) continue;
                    file.Delete();
                    _logger.LogInformation($"Manually deleted {file.FullName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to manually delete {file.FullName}");
                }
            }
        }
    }
}