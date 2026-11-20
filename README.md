# BzSound

[English](README.md) | **Deutsch**

Ein CounterStrikeSharp-Plugin für Counter-Strike 2, das für alle Spieler einen Sound abspielt, sobald jemand ein Trigger-Wort in den Chat schreibt.

## Funktionen

- Sounds werden durch ein Wort im Chat ausgelöst (z. B. `buckelfips`)
- Der Sound wird für **alle Spieler** auf dem Server abgespielt
- Immer aktiv – vorher muss nichts eingeschaltet werden
- Cooldown pro Spieler und globaler Server-Cooldown
- Trigger und Sounds frei per JSON konfigurierbar
- Konfiguration im Spiel neu laden, ohne Server-Neustart
- Hilfe-Befehl, der alle verfügbaren Trigger auflistet

## Voraussetzungen

- Counter-Strike 2 Dedicated Server
- [Metamod:Source](https://www.sourcemm.net/) (CS2-Version)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) **1.0.380** oder neuer (mit .NET-10-Runtime)
- Die Sound-Dateien (`.vsnd`) müssen bei den Spielern vorhanden sein, z. B. über ein Workshop-Addon

## Installation

1. Neuestes Release herunterladen.
2. Den Ordner `BzSound` kopieren nach:
   ```
   csgo/addons/counterstrikesharp/plugins/BzSound/
   ```
3. Server neu starten oder Map wechseln.
4. Beim ersten Start wird die Konfigurationsdatei automatisch angelegt:
   ```
   csgo/addons/counterstrikesharp/configs/plugins/BzSound/BzSound.json
   ```

## Benutzung

Eines der konfigurierten Trigger-Wörter in den Chat schreiben – genau das Wort, ohne weiteren Text:

```
buckelfips
```

Der Sound wird für alle Spieler abgespielt. Groß- und Kleinschreibung bei der Eingabe ist egal.

## Befehle

| Befehl | Beschreibung | Recht |
|---|---|---|
| `!bzsound` / `css_bzsound` | Zeigt alle verfügbaren Sound-Trigger | alle |
| `!sounds` / `css_sounds` | Zeigt alle verfügbaren Sound-Trigger | alle |
| `css_reloadsoundconfig` | Lädt die Konfiguration neu | `@css/config` |

## Konfiguration

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

| Option | Beschreibung | Standard |
|---|---|---|
| `CooldownSeconds` | Cooldown pro Spieler in Sekunden | `30.0` |
| `GlobalCooldownSeconds` | Cooldown für den gesamten Server in Sekunden | `10` |
| `Commands` | Liste der Trigger und ihrer Sound-Pfade | 3 Beispiele |

> **Hinweis:** Trigger in der Config immer **klein** schreiben, sonst werden sie nicht erkannt.

## Kompilieren

Benötigt das **.NET 10 SDK**.

```bash
dotnet build -c Release
```

Das fertige Plugin liegt danach in:

```
bin/Release/addons/counterstrikesharp/plugins/BzSound/
```

## Änderungen

### 1.0.1
- Umgestellt auf .NET 10 und CounterStrikeSharp.API 1.0.380
- Behoben: `css_reloadsoundconfig` findet die Konfigurationsdatei jetzt im richtigen Ordner
- `css_reloadsoundconfig` benötigt jetzt das Recht `@css/config`

### 1.0.0
- Erste Version

## Autor

**ddbmaster**
