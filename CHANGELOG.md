# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-01-31

### Added

- Initial release of Jelly Cache Manager plugin
- Cache monitoring for all Jellyfin directories
- Automatic cleanup based on size and age rules
- Admin UI for configuration and manual operations
- Scheduled background cleanup service
- Comprehensive logging
- Safety features to avoid deleting active files

### Features

- Configurable cleanup intervals
- Alert system for threshold breaches
- Manual cache clearing options
- Rebuild image cache functionality
- Embedded web interface

### Technical

- Built for Jellyfin 10.11.6 with .NET 9.0
- Compatible with Debian 13 and similar systems
- MIT licensed
