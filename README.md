# Jelly Cache Manager Plugin

A Jellyfin plugin for monitoring, managing, and automatically maintaining Jellyfin cache directories. This plugin ensures efficient cache lifecycle management without manual intervention.

## Features

- **Cache Monitoring**: Tracks disk usage of all Jellyfin cache-related directories.
- **Automatic Cleanup**: Configurable rules for max size, file age, and type filters with safe deletion.
- **Admin UI**: Integrated web interface for configuration and manual operations.
- **Scheduled Tasks**: Background service for periodic cleanup.
- **Logging**: Comprehensive logging of all cleanup actions.
- **Safety**: Skips active files (e.g., ongoing transcodes) to prevent data loss.

## Supported Cache Directories

- Cache: `/var/cache/jellyfin`
- Image Cache: `/var/cache/jellyfin/images`
- Program Data: `/var/lib/jellyfin`
- Logs: `/var/log/jellyfin`
- Metadata: `/var/lib/jellyfin/metadata`
- Transcodes: `/var/cache/jellyfin/transcodes`
- Web: `/usr/share/jellyfin/web`

All paths are configurable via the plugin settings.

## Requirements

- Jellyfin 10.11.6 or later
- .NET 9.0 runtime (on the server)
- .NET 9.0 SDK (for building)

## Build Instructions

### Prerequisites

- Debian 13 (or compatible Debian-based system)
- .NET 9 SDK installed

### Installing .NET 9 SDK on Debian

1. Update your package list:

   ```bash
   sudo apt update
   ```

2. Install required dependencies:

   ```bash
   sudo apt install wget apt-transport-https
   ```

3. Add the Microsoft package repository:

   ```bash
   wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   rm packages-microsoft-prod.deb
   ```

4. Update packages and install .NET 9 SDK:

   ```bash
   sudo apt update
   sudo apt install dotnet-sdk-9.0
   ```

5. Verify installation:

   ```bash
   dotnet --version
   ```

### Building the Plugin

1. Navigate to the plugin directory: `cd JellyCachePlugin`
2. Run `dotnet build` to build the plugin.
3. The output DLL will be in `bin/Debug/net9.0/JellyCachePlugin.dll`

## Installation Instructions

1. Copy the built DLL to Jellyfin's plugins directory (usually `/var/lib/jellyfin/plugins`).
2. Restart Jellyfin server (compatible with Jellyfin 10.11.6 using .NET 9).
3. The plugin will appear in the admin dashboard under Plugins.

## Packaging and Distribution

For easier distribution and "instant use" via Jellyfin's plugin catalog:

### Creating a Release Package

1. Build the plugin in Release mode: `dotnet build -c Release`
2. The DLL will be in `bin/Release/net9.0/JellyCachePlugin.dll`
3. Create a ZIP archive containing the DLL and any additional files (e.g., `JellyCachePlugin.zip`)
4. Compute the MD5 checksum: `md5sum JellyCachePlugin.zip`
5. Create or update `manifest.json` with the version details, source URL, checksum, and timestamp.

Automated releases are available via GitHub Actions on tagged commits (e.g., `v1.0.0`).

### Submitting to Jellyfin Plugin Repository

To make the plugin available in Jellyfin's official plugin catalog:

1. Fork and clone the [Jellyfin Plugin Repository](https://github.com/jellyfin/jellyfin-plugin-repo).
2. Create a directory for your plugin (e.g., `jellycacheplugin`).
3. Add your `manifest.json` file to the directory.
4. Upload the ZIP file to a publicly accessible location (e.g., GitHub Releases).
5. Update the `sourceUrl` in `manifest.json` to point to the ZIP.
6. Create a pull request with your plugin information and release assets.
7. Once approved, users can install directly from Jellyfin's admin dashboard.

### NuGet Package (Optional)

While not standard for Jellyfin plugins, you can package as NuGet for custom distributions:

1. Update the `.csproj` to include `<PackageId>JellyCachePlugin</PackageId>` and other package metadata.
2. Run `dotnet pack -c Release` to create a `.nupkg` file.
3. Publish to a NuGet feed if desired.

## Usage Guide

- **Accessing the Plugin**: Log into Jellyfin as an admin and navigate to the Dashboard > Plugins > Jelly Cache Manager.
- **Monitoring**: The main page displays a table with cache directories and their configurations.
- **Configuration**: Settings are managed through the plugin configuration in Jellyfin.
- **Automatic Cleanup**: The plugin runs cleanup automatically based on the configured schedule.
- **Logs**: Check Jellyfin server logs for cleanup activities and any issues.

## Configuration

The plugin supports the following settings:

- **Cleanup Interval**: Hours between automatic cleanups (default: 24).
- **Enable Alerts**: Whether to show warnings when directories exceed thresholds (default: true).
- **Alert Threshold**: Percentage of max size to trigger warnings (default: 80%).
- **Directory Configurations**: Per-directory settings for max size (MB), max age (days), and file type filters.

## Testing

After installation, verify the plugin loads and functions correctly:

1. **Plugin Loading**: Check Jellyfin logs for successful plugin initialization. Look for messages like "Loaded plugin: Jelly Cache Manager".
2. **Admin UI Access**: Navigate to Dashboard > Plugins > Jelly Cache Manager. The UI should display cache directory information.
3. **Configuration**: Modify settings and ensure they persist. Test alerts by adjusting thresholds.
4. **Cleanup Functionality**: Manually trigger cleanup or wait for scheduled runs. Monitor logs for cleanup actions and verify files are deleted safely (e.g., no active transcodes are removed).
5. **Non-Production Testing**: Always test in a development or staging environment first. Backup data before deploying to production.
6. **Error Handling**: Simulate edge cases like invalid paths or permission issues, and check for appropriate error logging.

## Updates

Since this plugin is distributed as source code, updates are manual:

1. Pull the latest changes: `git pull origin main`
2. Rebuild the plugin: `dotnet build -c Release`
3. Replace the DLL in Jellyfin's plugins directory.
4. Restart Jellyfin to load the updated plugin.

Alternatively, download pre-built releases from the [GitHub Releases](https://github.com/cpntodd/JellyCachePlugin/releases) page and replace the DLL.

For catalog-installed plugins (if submitted), updates may be available through Jellyfin's admin dashboard automatically.

## Contributing

Contributions are welcome! Please:

1. Fork the [repository](https://github.com/cpntodd/JellyCachePlugin).
2. Create a feature branch.
3. Make your changes.
4. Submit a pull request.

Ensure code follows C# best practices and includes appropriate tests.

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.

## Disclaimer

This plugin is provided as-is. Use at your own risk. Always backup your data before deploying to production.
