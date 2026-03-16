using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Funnies.Commands;

namespace Funnies.Modules;

public class Wallhack
{
    public static void OnPlayerTransmit(CCheckTransmitInfo info, CCSPlayerController player)
    {
        foreach (var entity in Globals.WallhackData)
        {
            if (Globals.Wallhackers.Contains(player!))
            {
                if (!Util.IsPlayerValid(entity.Key) || !Util.IsPlayerValid(player)) continue;

                if (entity.Key.Team != player!.Team && player!.Team != CsTeam.Spectator && entity.Key.Team != CsTeam.Spectator)
                {
                    info.TransmitEntities.Add(entity.Value.ModelRelay);
                    info.TransmitEntities.Add(entity.Value.GlowEnt);
                    continue;
                }
            }
            
            info.TransmitEntities.Remove(entity.Value.ModelRelay);
            info.TransmitEntities.Remove(entity.Value.GlowEnt);
        }
    }
    
    public static HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (!Util.IsPlayerValid(player)) return HookResult.Continue;
        if (player!.Team < CsTeam.Terrorist) return HookResult.Continue; // if player isnt on a team
        // var gameRules = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First();
        // if (gameRules.GameRules!.WarmupPeriod) return HookResult.Continue;
        
        CreateWallhackData(player!);
        
        return HookResult.Continue;
    }

    public static HookResult OnPlayerTakeDamage(EventPlayerHurt @event, GameEventInfo info)
    {
        if (Globals.Config.WallHackConfig.dynamicColor)
        {
            Globals.WallhackData.TryGetValue(@event.Userid, out var wallhackData);

            var health = @event.Health;
            
            health = Math.Clamp(health, 1, 100);

            float t = (health - 1) / 99f;

            int r = (int)(255 * (1f - t));
            int g = (int)(255 * t);

            wallhackData.GlowEnt.Glow.GlowColorOverride = Color.FromArgb(255 , r, g, 0);
            Utilities.SetStateChanged(wallhackData.GlowEnt, "CBaseModelEntity", "m_Glow");
        }
        
        return HookResult.Continue;
    }

    public static HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (!Util.IsPlayerValid(player)) return HookResult.Continue;
        if (!Globals.WallhackData.TryGetValue(player!, out var glowData)) return HookResult.Continue;

        RemoveWallhackData(player);
        
        Globals.Wallhackers.Remove(player!);

        return HookResult.Continue;
    }

    public static HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (!Util.IsPlayerValid(player)) return HookResult.Continue;
        if (!Globals.WallhackData.TryGetValue(player!, out var glowData)) return HookResult.Continue;
        if (!glowData.GlowEnt.IsValid) return HookResult.Continue;

        glowData.GlowEnt.Glow.GlowRange = 0;
        glowData.GlowEnt.DispatchSpawn();

        RemoveWallhackData(@event.Userid);

        return HookResult.Continue;
    }

    public static HookResult OnPlayerChangeTeam(EventPlayerTeam @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (!Util.IsPlayerValid(player)) return HookResult.Continue;
        if (!Globals.WallhackData.TryGetValue(player!, out var glowData)) return HookResult.Continue;
        if (!glowData.GlowEnt.IsValid) return HookResult.Continue;
        if (!glowData.ModelRelay.IsValid) return HookResult.Continue;

        RemoveWallhackData(player);
        
        return HookResult.Continue;
    }

    private static void CreateWallhackData(CCSPlayerController player)
    {
        var glowEntity = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
        var modelRelay = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");

        modelRelay!.Spawnflags = 256;
        modelRelay.Render = Color.Transparent;
        modelRelay.RenderMode = RenderMode_t.kRenderNone;
        modelRelay.CBodyComponent!.SceneNode!.Owner!.Entity!.Flags &= ~(1u << 2);
        modelRelay.SetModel(Util.GetPlayerModel(player));

        glowEntity!.Spawnflags = 256;
        glowEntity!.CBodyComponent!.SceneNode!.Owner!.Entity!.Flags &= ~(1u << 2);
        glowEntity.Render = Color.FromArgb(1, 0, 0, 0);
        glowEntity.SetModel(Util.GetPlayerModel(player));

        glowEntity.DispatchSpawn();
        modelRelay.DispatchSpawn();
        
        Color color = Color.FromArgb(255, Globals.Config.WallHackConfig.R, Globals.Config.WallHackConfig.G, Globals.Config.WallHackConfig.B);

        if (Globals.Config.WallHackConfig.dynamicColor)
        {
            color = Color.FromArgb(255, 0, 255, 0);
        }
        

        glowEntity.Glow.GlowRange = 5000;
        glowEntity.Glow.GlowRangeMin = 0;
        glowEntity.Glow.GlowColorOverride = color;
        glowEntity.Glow.GlowTeam = -1;
        glowEntity.Glow.GlowType = 3;

        modelRelay.AcceptInput("FollowEntity", player.Pawn.Value, null, "!activator");
        glowEntity.AcceptInput("FollowEntity", modelRelay, null, "!activator");

        Globals.WallhackData.Remove(player);
        Globals.WallhackData.Add(player, new() {
            GlowEnt = glowEntity,
            ModelRelay = modelRelay
        });
    }

    public static void RemoveWallhackData(CCSPlayerController player)
    {
        if(!Globals.WallhackData.TryGetValue(player, out var data)) return;
        data.GlowEnt.Remove();
        data.ModelRelay.Remove();
        Globals.WallhackData.Remove(player);
    }

    public static void Setup()
    {
        Globals.Plugin.RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        Globals.Plugin.RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
        Globals.Plugin.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn, HookMode.Post);
        Globals.Plugin.RegisterEventHandler<EventPlayerTeam>(OnPlayerChangeTeam, HookMode.Post);
        Globals.Plugin.RegisterEventHandler<EventPlayerHurt>(OnPlayerTakeDamage);

        Globals.Plugin.AddCommand("css_wh", "Gives a player walls", CommandWallhack.OnWallhackCommand);
        Globals.Plugin.AddCommand("css_wallhack", "Gives a player walls", CommandWallhack.OnWallhackCommand);
    }

    public static void Cleanup()
    {
        foreach (var entity in Globals.WallhackData)
        {
            Server.NextWorldUpdate(() => 
            {
                entity.Value.GlowEnt.Remove();
                entity.Value.ModelRelay.Remove();
            });
        }

        Globals.WallhackData.Clear();
        Globals.Wallhackers.Clear();
    }
}