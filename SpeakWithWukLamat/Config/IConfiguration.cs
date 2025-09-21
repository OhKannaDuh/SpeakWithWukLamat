using Dalamud.Configuration;

namespace SpeakWithWukLamat.Config;

public interface IConfiguration : IPluginConfiguration
{
    DetourConfig Detour { get; set; }
}
