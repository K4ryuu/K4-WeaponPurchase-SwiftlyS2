# Changelog

All notable changes to this project will be documented in this file.

## [1.1.0] - 2026-02-11

### Added

- **Map Restrictions**: Weapons can now be restricted to specific maps
  - `AllowedMaps` (whitelist): Weapon only available on listed maps
  - `DisabledMaps` (blacklist): Weapon disabled on listed maps
- **Team Restrictions**: Weapons can now be restricted to specific teams
  - `AllowedTeams`: Limit weapon availability to T, CT, or both teams
- **Command Registration Improvements**:
  - First alias is now the main command, others registered as aliases
  - Improved logging during plugin load showing main command and aliases
- New translation keys:
  - `k4.weaponpurchase.map_restricted`
  - `k4.weaponpurchase.team_restricted`

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
