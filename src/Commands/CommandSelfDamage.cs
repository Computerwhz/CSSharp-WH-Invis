using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace CSSharpWHInvi.Commands;

public class CommandSelfDamage
{
    public static void OnSelfDamageCommand(CCSPlayerController? caller, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(caller, Globals.Config.AdminPermission)) return;
        
        if (command.ArgString == "@ct")
        {
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (player.Team == CsTeam.CounterTerrorist)
                {
                    SetSelfDamage(player,  caller, command);
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
                    SetSelfDamage(player,  caller, command);
                    return;
                }
            }
        }
        else
        {
            CCSPlayerController player = Util.GetPlayerByName(command.ArgString);
            SetSelfDamage(player,  caller, command);
        }

        
    }

    private static void SetSelfDamage(CCSPlayerController? player, CCSPlayerController caller, CommandInfo command)
    {
        if (player != null)
        {
            if (Util.IsPlayerValid(caller))
                Util.ServerPrintToChat(caller!, $"Toggled Self Damage on {player.PlayerName}");

            if (!Globals.selfDamagePlayers.Remove(player))
                Globals.selfDamagePlayers.Add(player);
        }
        else
        {
            if (Util.IsPlayerValid(caller))
                Util.ServerPrintToChat(caller!, $"Player {command.ArgString} not found");
        }
    }
}
