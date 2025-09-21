using Dalamud.Bindings.ImGui;
using Ocelot.Services.WindowManager;

namespace SpeakWithWukLamat.Renderers;

public class ConfigRenderer : IConfigRenderer
{
    public void Render()
    {
        ImGui.Text("Hello World! Config");
    }
}
