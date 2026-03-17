using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace CSSharpWHInvi.Commands;

public class CommandWallhack
{
    public static void OnWallhackCommand(CCSPlayerController? caller, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(caller, Globals.Config.AdminPermission)) return;
        
        if (command.ArgString == "@ct")
        {
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (player.Team == CsTeam.CounterTerrorist)
                {
                    SetWallhack(player,  caller, command);
                    return;
                }
            }
        }
        else if (command.ArgString == "@t")
        {
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (player.Team == CsTeam.Terrorist)
                {
                    SetWallhack(player,  caller, command);
                    return;
                }
            }
        }
        else
        {
            CCSPlayerController player = Util.GetPlayerByName(command.ArgString);
            SetWallhack(player,  caller, command);
        }

        
    }

    private static void SetWallhack(CCSPlayerController? player, CCSPlayerController caller, CommandInfo command)
    {
        if (player != null)
        {
            if (Util.IsPlayerValid(caller))
                Util.ServerPrintToChat(caller!, $"Toggled wallhacks on {player.PlayerName}");

            if (!Globals.Wallhackers.Remove(player))
                Globals.Wallhackers.Add(player);
        }
        else
        {
            if (Util.IsPlayerValid(caller))
                Util.ServerPrintToChat(caller!, $"Player {command.ArgString} not found");
        }
    }
}