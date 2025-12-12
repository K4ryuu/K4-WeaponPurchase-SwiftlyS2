# Changelog

All notable changes to this project will be documented in this file.

## [1.0.1] - 2025-12-12

### Changed
- Configuration system now uses `IOptionsMonitor<PluginConfig>` for hot-reload support
- Configuration values accessed via `Config.CurrentValue` pattern
- Simplified game rules access using `Core.EntitySystem.GetGameRules()` instead of caching

### Removed
- Removed `EventRoundStart` hook (no longer needed for game rules caching)
- Removed `GameEventDefinitions` dependency

## [1.0.0] - Initial Release

### Added
- Purchase weapons, grenades, and utility items through chat commands
- Configurable weapon aliases
- Custom price support per weapon
- Buy zone and buy time restrictions (configurable)
- Grenade and healthshot limit enforcement
- Automatic weapon slot management (drops existing weapon when purchasing)
- Multi-language support via translation files
