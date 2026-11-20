using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Events;
using Microsoft.Extensions.Logging;

namespace BzSound;

public class BzSound : BasePlugin, IPluginConfig<ConfigGen>
{
    public override string ModuleName => "BzSound";
    public override string ModuleAuthor => "ddbmaster / BetreutesZocken";
    public override string ModuleVersion => "1.0.1";

    private readonly Dictionary<ulong, DateTime> _cooldowns = new();
    private DateTime _lastGlobalSound = DateTime.MinValue;

    public ConfigGen Config { get; set; } = new();

    // 🔧 Config laden
    public void OnConfigParsed(ConfigGen config)
    {
        Config = config;
        Logger.LogInformation($"[BzSound] Konfiguration geladen ({Config.Commands.Count} Sounds).");
    }

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerChat>(OnPlayerChat);

        // ✅ Nur noch diese Commands
        AddCommand("css_reloadsoundconfig", "BzSound Konfiguration neu laden", Cmd_ReloadConfig);
        AddCommand("bzsound", "BzSound Hilfe anzeigen", Cmd_Help);
        AddCommand("sounds", "BzSound Hilfe anzeigen", Cmd_Help);

        Logger.LogInformation("[BzSound] Plugin geladen – Sounds sind IMMER AN.");
    }

    private HookResult OnPlayerChat(EventPlayerChat ev, GameEventInfo info)
    {
        string text = ev.Text?.Trim().ToLower() ?? "";
        var cmd = Config.Commands.FirstOrDefault(c => c.Trigger == text);

        if (cmd == null)
            return HookResult.Continue;

        var player = Utilities.GetPlayerFromUserid(ev.Userid);
        if (player == null || !player.IsValid)
            return HookResult.Continue;

        ulong steamId = player.SteamID;

        // 🌍 Globaler Cooldown
        double globalElapsed = (DateTime.UtcNow - _lastGlobalSound).TotalSeconds;
        if (globalElapsed < Config.GlobalCooldownSeconds)
        {
            player.PrintToChat(
                $" {ChatColors.Green}[BZ Sound]{ChatColors.Default} ⏳ Globaler Cooldown: {Config.GlobalCooldownSeconds - globalElapsed:0.0}s"
            );
            return HookResult.Continue;
        }

        // 👤 Spieler-Cooldown
        if (_cooldowns.TryGetValue(steamId, out var lastUsed))
        {
            double elapsed = (DateTime.UtcNow - lastUsed).TotalSeconds;
            if (elapsed < Config.CooldownSeconds)
            {
                player.PrintToChat(
                    $" {ChatColors.Green}[BZ Sound]{ChatColors.Default} ⏳ Bitte warte {Config.CooldownSeconds - elapsed:0.0}s"
                );
                return HookResult.Continue;
            }
        }

        // Cooldowns setzen
        _cooldowns[steamId] = DateTime.UtcNow;
        _lastGlobalSound = DateTime.UtcNow;

        // 🔊 Sound für ALLE Spieler abspielen (immer AN)
        foreach (var p in Utilities.GetPlayers())
        {
            if (p != null && p.IsValid)
            {
                p.ExecuteClientCommand($"play {cmd.Sound}");
            }
        }

        Logger.LogInformation($"[BzSound] Sound abgespielt: {cmd.Sound}");
        return HookResult.Continue;
    }

    // 🔄 Konfiguration neu laden (nur Admins mit @css/config)
    [RequiresPermissions("@css/config")]
    private void Cmd_ReloadConfig(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsValid) return;

        try
        {
            // CSS legt die Config unter configs/plugins/<Plugin>/<Plugin>.json ab
            string configPath = Path.Combine(
                Server.GameDirectory, "csgo", "addons", "counterstrikesharp",
                "configs", "plugins", ModuleName, $"{ModuleName}.json");

            if (!File.Exists(configPath))
            {
                player.PrintToChat(
                    $" {ChatColors.Green}[BZ Sound]{ChatColors.Default} ⚠ Konfigurationsdatei nicht gefunden!"
                );
                return;
            }

            string json = File.ReadAllText(configPath);
            var newConfig = System.Text.Json.JsonSerializer.Deserialize<ConfigGen>(json);

            if (newConfig != null)
            {
                Config = newConfig;
                player.PrintToChat(
                    $" {ChatColors.Green}[BZ Sound]{ChatColors.Default} ✅ Konfiguration neu geladen."
                );
                Logger.LogInformation($"[BzSound] Konfiguration neu geladen von {player.PlayerName}");
            }
        }
        catch (Exception ex)
        {
            player.PrintToChat(
                $" {ChatColors.Green}[BZ Sound]{ChatColors.Default} ❌ Fehler beim Neuladen der Konfiguration."
            );
            Logger.LogError(ex, "[BzSound] Fehler beim Reload");
        }
    }

    // 📖 Hilfe
    private void Cmd_Help(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsValid) return;

        player.PrintToChat($" {ChatColors.Green}[BZ Sound]{ChatColors.Default} 📢 Sound-Trigger:");

        if (Config.Commands.Count == 0)
        {
            player.PrintToChat($" {ChatColors.Red}⚠ Keine Sounds konfiguriert!");
            return;
        }

        foreach (var c in Config.Commands)
        {
            player.PrintToChat($" {ChatColors.LightBlue}{c.Trigger}{ChatColors.Default}");
        }
    }
}

// ⚙ Konfiguration (configs/plugins/BzSound/BzSound.json)
public class ConfigGen : BasePluginConfig
{
    public float CooldownSeconds { get; set; } = 30f;
    public int GlobalCooldownSeconds { get; set; } = 10;

    public List<SoundCommandItem> Commands { get; set; } = new()
    {
        new SoundCommandItem { Trigger = "buckelfips", Sound = "sounds/bz/buckelfips.vsnd" },
        new SoundCommandItem { Trigger = "baerbel",    Sound = "sounds/bz/baerbel.vsnd" },
        new SoundCommandItem { Trigger = "hellas",     Sound = "sounds/bz/hellas.vsnd" }
    };
}

public class SoundCommandItem
{
    public string Trigger { get; set; } = string.Empty;
    public string Sound { get; set; } = string.Empty;
}
