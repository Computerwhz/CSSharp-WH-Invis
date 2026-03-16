using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Extensions;
using Funnies.Commands;

namespace Funnies.Modules;

public class SelfDamage
{
    
    private static readonly Dictionary<string, Func<SelfDamageConfig, int>> WeaponDamage = new(StringComparer.OrdinalIgnoreCase)
    {
        ["weapon_ak47"] = c => c.WeaponAk47,
        ["weapon_aug"] = c => c.WeaponAug,
        ["weapon_awp"] = c => c.WeaponAwp,
        ["weapon_cz75a"] = c => c.WeaponCz75a,
        ["weapon_deagle"] = c => c.WeaponDeagle,
        ["weapon_elite"] = c => c.WeaponElite,
        ["weapon_famas"] = c => c.WeaponFamas,
        ["weapon_fiveseven"] = c => c.WeaponFiveseven,
        ["weapon_g3sg1"] = c => c.WeaponG3sg1,
        ["weapon_galilar"] = c => c.WeaponGalilar,
        ["weapon_glock"] = c => c.WeaponGlock,
        ["weapon_m249"] = c => c.WeaponM249,
        ["weapon_m4a1_silencer"] = c => c.WeaponM4a1Silencer,
        ["weapon_m4a1"] = c => c.WeaponM4a1,
        ["weapon_mac10"] = c => c.WeaponMac10,
        ["weapon_mag7"] = c => c.WeaponMag7,
        ["weapon_mp5sd"] = c => c.WeaponMp5sd,
        ["weapon_mp7"] = c => c.WeaponMp7,
        ["weapon_mp9"] = c => c.WeaponMp9,
        ["weapon_negev"] = c => c.WeaponNegev,
        ["weapon_nova"] = c => c.WeaponNova,
        ["weapon_hkp2000"] = c => c.WeaponHkp2000,
        ["weapon_p250"] = c => c.WeaponP250,
        ["weapon_p90"] = c => c.WeaponP90,
        ["weapon_bizon"] = c => c.WeaponBizon,
        ["weapon_revolver"] = c => c.WeaponRevolver,
        ["weapon_sawedoff"] = c => c.WeaponSawedoff,
        ["weapon_scar20"] = c => c.WeaponScar20,
        ["weapon_sg556"] = c => c.WeaponSg556,
        ["weapon_ssg08"] = c => c.WeaponSsg08,
        ["weapon_tec9"] = c => c.WeaponTec9,
        ["weapon_ump45"] = c => c.WeaponUmp45,
        ["weapon_usp_silencer"] = c => c.WeaponUspSilencer,
        ["weapon_xm1014"] = c => c.WeaponXm1014,
        ["weapon_taser"] = c => c.WeaponTaser
    };
    
    public static HookResult OnPlayerShoot(EventWeaponFire @event, GameEventInfo info)
    {
        if (Globals.selfDamagePlayers.Contains(@event.Userid))
        {
            var player =  @event.Userid;

            int damage = getWeaponDamage(@event.Weapon);

            int newHealth = player.PlayerPawn.Value.Health - damage;
            
            if (newHealth <= 0)
            {
                player.PlayerPawn.Value.CommitSuicide(false, true);
            }
            else
            {
                player.PlayerPawn.Value.Health = newHealth;
            }
        }
        return HookResult.Continue;
    }

    public static HookResult OnShotHit(EventPlayerHurt @event, GameEventInfo info)
    {
        if (Globals.selfDamagePlayers.Contains(@event.Attacker))
        {
            if (@event.Userid.Team != @event.Attacker.Team)
            {
                var player = @event.Attacker;
                
                int heal = getWeaponDamage(@event.Attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon.Get().GetWeaponName());

                int newHealth = player.PlayerPawn.Value.Health + heal;

                player.PlayerPawn.Value.Health = newHealth;
            }
        }
        return HookResult.Continue;
    }

    public static void Setup()
    {
        Globals.Plugin.RegisterEventHandler<EventWeaponFire>(OnPlayerShoot);
        Globals.Plugin.RegisterEventHandler<EventPlayerHurt>(OnShotHit);
        Globals.Plugin.AddCommand("css_selfdamage", "Turns on self damage for a player", CommandSelfDamage.OnSelfDamageCommand);
        Globals.Plugin.AddCommand("css_sdamage", "Turns on self damage for a player", CommandSelfDamage.OnSelfDamageCommand);
    }

    public static int getWeaponDamage(String weapon)
    {
        try
        {
            return WeaponDamage[weapon](Globals.Config.SelfDamageConfig);
        }
        catch
        {
            // Attack by knife most likely
            return 0;
        }
    }
}