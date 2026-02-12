<a name="readme-top"></a>

![GitHub tag (with filter)](https://img.shields.io/github/v/tag/K4ryuu/K4-WeaponPurchase-SwiftlyS2?style=for-the-badge&label=Version)
![GitHub Repo stars](https://img.shields.io/github/stars/K4ryuu/K4-WeaponPurchase-SwiftlyS2?style=for-the-badge)
![GitHub issues](https://img.shields.io/github/issues/K4ryuu/K4-WeaponPurchase-SwiftlyS2?style=for-the-badge)
![GitHub](https://img.shields.io/github/license/K4ryuu/K4-WeaponPurchase-SwiftlyS2?style=for-the-badge)
![GitHub all releases](https://img.shields.io/github/downloads/K4ryuu/K4-WeaponPurchase-SwiftlyS2/total?style=for-the-badge)
[![Discord](https://img.shields.io/badge/Discord-Join%20Server-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://dsc.gg/k4-fanbase)

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h1 align="center">KitsuneLab©</h1>
  <h3 align="center">K4 - Weapon Purchase</h3>
  <a align="center">Purchase weapons, grenades, and equipment through chat commands in Counter-Strike 2.</a>

  <p align="center">
    <br />
    <a href="https://github.com/K4ryuu/K4-WeaponPurchase-SwiftlyS2/releases/latest">Download</a>
    ·
    <a href="https://github.com/K4ryuu/K4-WeaponPurchase-SwiftlyS2/issues/new?assignees=K4ryuu&labels=bug&projects=&template=bug_report.md&title=%5BBUG%5D">Report Bug</a>
    ·
    <a href="https://github.com/K4ryuu/K4-WeaponPurchase-SwiftlyS2/issues/new?assignees=K4ryuu&labels=enhancement&projects=&template=feature_request.md&title=%5BREQ%5D">Request Feature</a>
  </p>
</div>

## Features

- **Chat Command Purchases** - Buy weapons using simple chat commands (e.g., `!ak47`, `!awp`, `!deagle`)
- **Full Weapon Support** - All CS2 weapons including rifles, SMGs, shotguns, pistols, grenades, and utility items
- **Configurable Aliases** - Multiple command aliases per weapon (e.g., `!ak` or `!ak47`)
- **Custom Pricing** - Override default weapon prices in configuration
- **Map Restrictions** - Whitelist or blacklist weapons on specific maps
- **Team Restrictions** - Limit weapons to specific teams (T/CT)
- **Buy Zone Check** - Optional enforcement of buy zone requirement
- **Buy Time Check** - Optional enforcement of buy time restriction
- **Grenade Limits** - Respects game's grenade carry limits
- **Multi-language Support** - Fully translatable weapon names and messages

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Support My Work

I create free, open-source Counter-Strike 2 plugins for the community. If you'd like to support my work, consider becoming a sponsor!

#### 💖 GitHub Sponsors

Support this project through [GitHub Sponsors](https://github.com/sponsors/K4ryuu) with flexible options:

- **One-time** or **monthly** contributions
- **Custom amount** - choose what works for you
- **Multiple tiers available** - from basic benefits to priority support or private project access

Every contribution helps me dedicate more time to development, support, and creating new features. Thank you! 🙏

<p align="center">
  <a href="https://github.com/sponsors/K4ryuu">
    <img src="https://img.shields.io/badge/sponsor-30363D?style=for-the-badge&logo=GitHub-Sponsors&logoColor=#EA4AAA" alt="GitHub Sponsors" />
  </a>
</p>

⭐ **Or support me for free by starring this repository!**
### Dependencies

To use this server addon, you'll need the following dependencies installed:

- [**SwiftlyS2**](https://github.com/swiftly-solution/swiftlys2): SwiftlyS2 is a server plugin framework for Counter-Strike 2

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- INSTALLATION -->

## Installation

1. Install [SwiftlyS2](https://github.com/swiftly-solution/swiftlys2) on your server
2. [Download the latest release](https://github.com/K4ryuu/K4-WeaponPurchase-SwiftlyS2/releases/latest)
3. Extract to your server's `swiftlys2/plugins/` directory
4. Restart your server or load the plugin

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONFIGURATION -->

## Configuration

Configuration file: `swiftlys2/configs/k4-weaponpurchase.jsonc`

### General Settings

| Option         | Description                       | Default |
| -------------- | --------------------------------- | ------- |
| `CheckBuyZone` | Require players to be in buy zone | `true`  |
| `CheckBuyTime` | Require buy time to be active     | `true`  |

### Weapon Configuration

Each weapon entry supports:

| Option         | Description                                      | Required |
| -------------- | ------------------------------------------------ | -------- |
| `Aliases`      | List of command aliases (without prefix)         | Yes      |
| `CustomPrice`  | Override default weapon price                    | No       |
| `AllowedMaps`  | List of maps where weapon is available (whitelist) | No       |
| `DisabledMaps` | List of maps where weapon is disabled (blacklist)  | No       |
| `AllowedTeams` | List of teams that can use weapon (`T`, `CT`, or empty for both) | No       |

**Restriction Logic:**
- **AllowedMaps** (Whitelist): If set, weapon is ONLY available on listed maps
  - ⚠️ **PRIORITY**: If `AllowedMaps` is set, `DisabledMaps` is IGNORED
  - Use when you want explicit control over where weapon is available
- **DisabledMaps** (Blacklist): If set, weapon is blocked on listed maps, allowed everywhere else
  - Only applies if `AllowedMaps` is NOT set
  - Use when weapon should be available everywhere except specific maps
- **AllowedTeams**: If set, weapon is ONLY available for listed teams (values: `T`, `CT`)
  - Empty or not set = available for both teams
- **Best Practice**: Don't use `AllowedMaps` and `DisabledMaps` together (AllowedMaps takes priority)
- All map and team names are case-insensitive

Example configuration:

```jsonc
{
  "K4WeaponPurchase": {
    "Weapons": {
      // No restrictions - available everywhere for everyone
      "weapon_ak47": { "Aliases": ["ak47", "ak"] },

      // Blacklist example - AWP disabled on specific competitive maps
      "weapon_awp": {
        "Aliases": ["awp"],
        "CustomPrice": 5000,
        "DisabledMaps": ["de_dust2", "de_mirage"]
      },

      // Whitelist example - Negev ONLY available on fun maps
      "weapon_negev": {
        "Aliases": ["negev"],
        "AllowedMaps": ["aim_map", "fy_poolday"]
      },

      // Team restriction - Deagle only for Terrorists
      "weapon_deagle": {
        "Aliases": ["deagle"],
        "AllowedTeams": ["T"]
      },

      // Combined restrictions - Scout only for CT on specific maps
      "weapon_ssg08": {
        "Aliases": ["ssg", "scout"],
        "AllowedMaps": ["de_nuke", "de_train"],
        "AllowedTeams": ["CT"]
      },

      "weapon_healthshot": { "Aliases": ["healthshot"], "CustomPrice": 500 }
    },
    "CheckBuyZone": true,
    "CheckBuyTime": true
  }
}
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- COMMANDS -->

## Commands

All commands use the aliases defined in configuration. Default aliases include:

### Rifles

| Command                    | Weapon   |
| -------------------------- | -------- |
| `!ak47`, `!ak`             | AK-47    |
| `!m4a1s`, `!m4s`           | M4A1-S   |
| `!m4a1`, `!m4`, `!m4a4`    | M4A4     |
| `!awp`                     | AWP      |
| `!aug`                     | AUG      |
| `!famas`                   | FAMAS    |
| `!galilar`, `!galil`       | Galil AR |
| `!sg556`, `!sg`            | SG 553   |
| `!ssg`, `!scout`, `!ssg08` | SSG 08   |
| `!scar20`, `!scar`         | SCAR-20  |
| `!g3sg1`                   | G3SG1    |
| `!m249`                    | M249     |
| `!negev`                   | Negev    |

### SMGs

| Command          | Weapon   |
| ---------------- | -------- |
| `!mp9`           | MP9      |
| `!mp7`           | MP7      |
| `!mp5sd`, `!mp5` | MP5-SD   |
| `!ump45`, `!ump` | UMP-45   |
| `!p90`           | P90      |
| `!bizon`         | PP-Bizon |
| `!mac10`, `!mac` | MAC-10   |

### Shotguns

| Command          | Weapon    |
| ---------------- | --------- |
| `!nova`          | Nova      |
| `!xm1014`, `!xm` | XM1014    |
| `!sawedoff`      | Sawed-Off |
| `!mag7`, `!mag`  | MAG-7     |

### Pistols

| Command                           | Weapon        |
| --------------------------------- | ------------- |
| `!deagle`                         | Desert Eagle  |
| `!elite`, `!dualberettas`         | Dual Berettas |
| `!fiveseven`                      | Five-SeveN    |
| `!glock`, `!glock18`              | Glock-18      |
| `!hkp2000`, `!hkp`, `!p2000`      | P2000         |
| `!p250`                           | P250          |
| `!tec9`, `!tec`                   | Tec-9         |
| `!usp_silencer`, `!usp`, `!usp-s` | USP-S         |
| `!cz75a`, `!cz`, `!cs75-auto`     | CZ75-Auto     |
| `!revolver`, `!r8revolver`, `!r8` | R8 Revolver   |

### Grenades

| Command                         | Weapon             |
| ------------------------------- | ------------------ |
| `!flashbang`, `!flash`          | Flashbang          |
| `!smokegrenade`, `!smoke`       | Smoke Grenade      |
| `!hegrenade`, `!grenade`, `!he` | HE Grenade         |
| `!molotov`                      | Molotov            |
| `!incgrenade`                   | Incendiary Grenade |
| `!decoy`                        | Decoy Grenade      |
| `!tagrenade`                    | Tactical Grenade   |

### Utility

| Command       | Weapon      |
| ------------- | ----------- |
| `!taser`      | Zeus x27    |
| `!healthshot` | Healthshot  |
| `!kevlar`     | Kevlar Vest |
| `!shield`     | Riot Shield |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- TRANSLATIONS -->

## Translations

Translation files are located in `resources/translations/`. Create new language files (e.g., `hu.jsonc`, `de.jsonc`) following the same structure as `en.jsonc`.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->

## License

Distributed under the GPL-3.0 License. See [`LICENSE.md`](LICENSE.md) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
