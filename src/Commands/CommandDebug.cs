#if DEBUG
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace CSSharpWHInvi.Commands;

public class CommandDebug
{
    public static void OnDebugCommand(CCSPlayerController? caller, CommandInfo command)
    {
        if (command.ArgString == "bot")
        {
            var bot = Util.GetBots()[0];

            bot.Pawn.Value!.Teleport(caller.Pawn.Value.AbsOrigin, bot.Pawn.Value.AbsRotation, bot.Pawn.Value.AbsVelocity);
        }
        else
        {
            var plantedC4 = Utilities.CreateEntityByName<CPlantedC4>("planted_c4");

            plantedC4.AbsOrigin.X = caller.PlayerPawn.Value.AbsOrigin.X;
            plantedC4.AbsOrigin.Y = caller.PlayerPawn.Value.AbsOrigin.Y;
            plantedC4.AbsOrigin.Z = caller.PlayerPawn.Value.AbsOrigin.Z;
            plantedC4.HasExploded = false;

            plantedC4.BombSite = 0;
            plantedC4.BombTicking = true;
            plantedC4.CannotBeDefused = false;

            plantedC4.DispatchSpawn();

            var gameRules = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First().GameRules;
            gameRules.BombPlanted = true;
            gameRules.BombDefused = false;
            var eventPtr = NativeAPI.CreateEvent("bomb_planted", true);
            NativeAPI.SetEventPlayerController(eventPtr, "userid", caller.Handle);
            NativeAPI.SetEventInt(eventPtr, "site", 0);

            NativeAPI.FireEvent(eventPtr, false);
        }
    }
}
#endif