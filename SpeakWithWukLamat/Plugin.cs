using Dalamud.Plugin;
using ECommons;
using Ocelot;
using Ocelot.Chain;

namespace SpeakWithWukLamat;

public sealed class Plugin : OcelotPlugin
{
    public override string Name {
        get => "SpeakWithWukLamat";
    }

    public Config Config { get; }

    public override OcelotConfig OcelotConfig {
        get => Config;
    }

    public static ChainQueue Chain {
        get => ChainManager.Get("SpeakWithWukLamat.Chain");
    }

    public Plugin(IDalamudPluginInterface plugin)
        : base(plugin, Module.DalamudReflector)
    {
        Config = plugin.GetPluginConfig() as Config ?? new Config();

        SetupLanguage(plugin);

        OcelotInitialize();
    }

    private void SetupLanguage(IDalamudPluginInterface plugin)
    {
        I18N.SetDirectory(plugin.AssemblyLocation.Directory?.FullName!);
        I18N.LoadAllFromDirectory("en", "Translations/en");

        I18N.SetLanguage("en");
    }

    protected override void OnCoreLoaded()
    {
        Commands.InitializeMainCommand("/speak_with_wuk_lamat");
        Commands.InitializeConfigCommand("/speak_with_wuk_lamatcfg", aliases: ["/speak_with_wuk_lamatc"]);
    }
}
