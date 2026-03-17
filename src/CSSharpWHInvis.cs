using CounterStrikeSharp.API.Core;
using CSSharpWHInvi.Commands;
using CSSharpWHInvi.Modules;

namespace CSSharpWHInvi;


 
public class CSSharpWHInvis : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "CSSharp Wallhack and Invisible plugin";
    public override string ModuleVersion => "1.0.1";

    public Config Config { get; set; }

    public override void Load(bool hotReload)
    {

        Globals.Plugin = this;

        RegisterListener<Listeners.CheckTransmit>(OnCheckTransmit);
        RegisterListener<Listeners.OnTick>(OnTick);

        #if DEBUG
        AddCommand("css_debug", "Debug command", CommandDebug.OnDebugCommand);
        #endif

        Invisible.Setup();
        Wallhack.Setup();
        SelfDamage.Setup();
    }

    public override void Unload(bool hotReload)
    {
        #if DEBUG
        if (hotReload)
        {
            Invisible.Cleanup();
            Wallhack.Cleanup();
        }
        #else
        Console.WriteLine($"Reloading: hotReload? {hotReload}");
        #endif
    }

    public void OnTick()
    {
        Invisible.OnTick();
    }

    public void OnCheckTransmit(CCheckTransmitInfoList infoList)
    {
        foreach ((CCheckTransmitInfo info, CCSPlayerController? player) in infoList)
        {
            if (!Util.IsPlayerValid(player))
                continue;

            Wallhack.OnPlayerTransmit(info, player!);
            Invisible.OnPlayerTransmit(info, player!);
        }
    }

    public void OnConfigParsed(Config config)
    {
        Config = config;
        Globals.Config = config;
    }
}
