using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace JellyCachePlugin
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public Dictionary<string, CacheDirectoryConfig> CacheDirectories { get; set; } = new Dictionary<string, CacheDirectoryConfig>
        {
            { "Cache", new CacheDirectoryConfig { Path = "/var/cache/jellyfin", MaxSizeMB = 1000, MaxAgeDays = 30 } },
            { "ImageCache", new CacheDirectoryConfig { Path = "/var/cache/jellyfin/images", MaxSizeMB = 500, MaxAgeDays = 7 } },
            { "ProgramData", new CacheDirectoryConfig { Path = "/var/lib/jellyfin", MaxSizeMB = 2000, MaxAgeDays = 60 } },
            { "Logs", new CacheDirectoryConfig { Path = "/var/log/jellyfin", MaxSizeMB = 100, MaxAgeDays = 14 } },
            { "Metadata", new CacheDirectoryConfig { Path = "/var/lib/jellyfin/metadata", MaxSizeMB = 1500, MaxAgeDays = 90 } },
            { "Transcodes", new CacheDirectoryConfig { Path = "/var/cache/jellyfin/transcodes", MaxSizeMB = 2000, MaxAgeDays = 1 } },
            { "Web", new CacheDirectoryConfig { Path = "/usr/share/jellyfin/web", MaxSizeMB = 500, MaxAgeDays = 365 } }
        };

        public int CleanupIntervalHours { get; set; } = 24;
        public bool EnableAlerts { get; set; } = true;
        public int AlertThresholdPercent { get; set; } = 80;
    }

    public class CacheDirectoryConfig
    {
        public string Path { get; set; }
        public long MaxSizeMB { get; set; }
        public int MaxAgeDays { get; set; }
        public List<string> FileTypeFilters { get; set; } = new List<string>();
    }
}