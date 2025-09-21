using System;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace SpeakWithWukLamat.Config;

[Serializable]
public class Configuration(IDalamudPluginInterface plugin) : IConfiguration
{
    public int Version { get; set; } = 1;

    public DetourConfig Detour { get; set; } = new();

    public void Save()
    {
        plugin.SavePluginConfig(this);
    }
}
