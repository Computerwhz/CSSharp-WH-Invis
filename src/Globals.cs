using CounterStrikeSharp.API.Core;
using CSSharpWHInvi.Models;

namespace CSSharpWHInvi;

public static class Globals
{
    public static Config Config { get; set; }
    public static List<CCSPlayerController> Wallhackers = [];
    public static Dictionary<CCSPlayerController, WallhackData> WallhackData = [];
    
    public static List<CCSPlayerController> selfDamagePlayers = [];

    public static Dictionary<CCSPlayerController, InvisibleData> InvisiblePlayers = [];

#pragma warning disable CS8618
    public static CSSharpWHInvis Plugin;
#pragma warning restore CS8618
}