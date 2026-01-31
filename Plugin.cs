using System;
using System.Collections.Generic;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace JellyCachePlugin
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
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

        public override Guid Id => Guid.Parse("f5c8d4e3-2b1a-4c5d-8e9f-1a2b3c4d5e6f");

        public override string Description => "Monitors and manages Jellyfin cache directories.";

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "JellyCachePlugin",
                    EmbeddedResourcePath = GetType().Namespace + ".Web.index.html"
                }
            };
        }

        public IPluginServiceRegistrator GetServiceRegistrator()
        {
            return new PluginServiceRegistrator();
        }

        public override void UpdateConfiguration(BasePluginConfiguration configuration)
        {
            base.UpdateConfiguration(configuration);
        }
    }
}