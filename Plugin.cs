using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Api.Controllers;
using Jellyfin.Api.Helpers;
using Jellyfin.Data.Entities;
using Jellyfin.Data.Enums;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JellyCachePlugin
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages, IHasPluginServices
    {
        private readonly ILogger<Plugin> _logger;
        private readonly IServerApplicationHost _applicationHost;

        public Plugin(
            IApplicationPaths applicationPaths,
            IXmlSerializer xmlSerializer,
            ILogger<Plugin> logger,
            IServerApplicationHost applicationHost)
            : base(applicationPaths, xmlSerializer)
        {
            _logger = logger;
            _applicationHost = applicationHost;
        }

        public override string Name => "Jelly Cache Manager";

        public override Guid Id => Guid.Parse("12345678-1234-1234-1234-123456789abc");

        public override string Description => "Monitors and manages Jellyfin cache directories.";

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "JellyCachePlugin",
                    EmbeddedResourcePath = GetType().Namespace + ".UI.index.html"
                }
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Services.CleanupService>();
            services.AddHostedService<Services.CacheMonitorService>();
        }

        public override void UpdateConfiguration(BasePluginConfiguration configuration)
        {
            base.UpdateConfiguration(configuration);
            // Trigger any necessary updates
        }
    }
}