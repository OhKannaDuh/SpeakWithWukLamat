using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using Ocelot.Services.ClientState;
using Ocelot.Services.Data;
using Map = ECommons.GameHelpers.Map;

namespace SpeakWithWukLamat.Data.Objects;

public class Aetheryte(uint id, IDataRepository<AetheryteData> aetherytes)
{
    public readonly AetheryteData Data = aetherytes.Get(id);

    public IGameObject? GameObject
    {
        get => Svc.Objects.Where(o => o.ObjectKind == ObjectKind.Aetheryte).FirstOrDefault(o => o.DataId == id);
    }

    public Vector3 Position
    {
        get => Map.AetherytePosition(id);
    }
}
