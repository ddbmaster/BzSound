# BzSound

**English** | [Deutsch](README.de.md)

A CounterStrikeSharp plugin for Counter-Strike 2 that plays sounds for all players when someone types a trigger word in chat.

## Features

- Sounds are triggered by typing a word in chat (e.g. `buckelfips`)
- The sound is played for **all players** on the server
- Always active – no need to enable anything first
- Per-player cooldown and global server cooldown
- Freely configurable triggers and sounds via JSON
- Reload the configuration in-game without restarting the server
- Help command that lists all available triggers

## Requirements

- Counter-Strike 2 dedicated server
- [Metamod:Source](https://www.sourcemm.net/) (CS2 version)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) **1.0.380** or newer (with .NET 10 runtime)
- The sound files (`.vsnd`) must be available to the players, e.g. via a Workshop addon

## Installation

1. Download the latest release.
2. Copy the folder `BzSound` to:
   ```
   csgo/addons/counterstrikesharp/plugins/BzSound/
   ```
3. Restart the server or change the map.
4. On first start, the configuration file is created automatically:
   ```
   csgo/addons/counterstrikesharp/configs/plugins/BzSound/BzSound.json
   ```

## Usage

Type one of the configured trigger words in chat – exactly the word, without anything else:

```
buckelfips
```

The sound is played for all players. Upper and lower case of the chat input do not matter.

## Commands

| Command | Description | Permission |
|---|---|---|
| `!bzsound` / `css_bzsound` | Shows all available sound triggers | everyone |
| `!sounds` / `css_sounds` | Shows all available sound triggers | everyone |
| `css_reloadsoundconfig` | Reloads the configuration | `@css/config` |

## Configuration

`BzSound.json`:

```json
{
  "CooldownSeconds": 30.0,
  "GlobalCooldownSeconds": 10,
  "Commands": [
    { "Trigger": "buckelfips", "Sound": "sounds/bz/buckelfips.vsnd" },
    { "Trigger": "baerbel",    "Sound": "sounds/bz/baerbel.vsnd" },
    { "Trigger": "hellas",     "Sound": "sounds/bz/hellas.vsnd" }
  ],
  "ConfigVersion": 1
}
```

| Option | Description | Default |
|---|---|---|
| `CooldownSeconds` | Cooldown per player in seconds | `30.0` |
| `GlobalCooldownSeconds` | Cooldown for the whole server in seconds | `10` |
| `Commands` | List of triggers and their sound paths | 3 examples |

> **Note:** Always write triggers in **lower case** in the config, otherwise they will not be recognized.

## Building

Requires the **.NET 10 SDK**.

```bash
dotnet build -c Release
```

The compiled plugin is placed in:

```
bin/Release/addons/counterstrikesharp/plugins/BzSound/
```

## Changelog

### 1.0.1
- Updated to .NET 10 and CounterStrikeSharp.API 1.0.380
- Fixed: `css_reloadsoundconfig` now finds the configuration file in the correct folder
- `css_reloadsoundconfig` now requires the `@css/config` permission

### 1.0.0
- Initial release

## Author

**ddbmaster**
