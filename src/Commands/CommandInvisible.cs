using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace CSSharpWHInvi.Commands;

public class CommandInvisible
{
    public static void OnInvisibleCommand(CCSPlayerController? caller, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(caller, Globals.Config.AdminPermission)) return;

        if (command.ArgString == "@ct")
        {
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (player.Team == CsTeam.CounterTerrorist)
                {
                    SetInvis(player,  caller, command);
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
                    SetInvis(player,  caller, command);
                    return;
                }
            }
        }
        else
        {
            CCSPlayerController player = Util.GetPlayerByName(command.ArgString);
            SetInvis(player,  caller, command);
        }
    }

    private static void SetInvis(CCSPlayerController player, CCSPlayerController? caller, CommandInfo command)
    {
        if (player != null)
        {
            if (Util.IsPlayerValid(caller))
                Util.ServerPrintToChat(caller!, $"Toggled invisiblity on {player.PlayerName}");

            if (Globals.InvisiblePlayers.Remove(player))
            {
                var pawn = player.PlayerPawn.Value;
                pawn!.Render = Color.FromArgb(255, pawn.Render);
                Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");

                foreach (var weapon in pawn.WeaponServices!.MyWeapons)
                {
                    weapon.Value!.Render = pawn!.Render;
                    Utilities.SetStateChanged(weapon.Value, "CBaseModelEntity", "m_clrRender");
                }
            }
            else
                Globals.InvisiblePlayers.Add(player, new());
        }
        else
        {
            if (Util.IsPlayerValid(caller))
                Util.ServerPrintToChat(caller!, $"Player {command.ArgString} not found");
        }
    }
    
}