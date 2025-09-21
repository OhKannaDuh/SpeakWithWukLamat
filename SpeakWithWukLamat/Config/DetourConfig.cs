using System;

namespace SpeakWithWukLamat.Config;

[Serializable]
public class DetourConfig
{
    public bool ShouldDetourForAetherytes { get; set; } = true;

    public bool ShouldDetourForAethernetShards { get; set; } = true;

    public bool ShouldDetourForFeatureQuests { get; set; } = false;
}
