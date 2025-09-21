using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using Ocelot.Services.ClientState;
using Ocelot.Services.Data;
using Ocelot.Services.PlayerState;

namespace SpeakWithWukLamat.Data.Npcs;

public class EventNpc(
    uint id,
    Level level,
    IDataRepository<ENpcBase> npcBases,
    IDataRepository<ENpcResident> npcResidents,
    IClient client,
    IPlayer player
)
{
    public readonly ENpcBase BaseData = npcBases.Get(id);

    public readonly ENpcResident ResidentData = npcResidents.Get(id);

    public string Name
    {
        get => ResidentData.Singular.ExtractText();
    }

    public IGameObject? GameObject
    {
        get => Svc.Objects.OfType<ICharacter>().FirstOrDefault(c => c.DataId == id);
    }

    public Vector3 Position
    {
        get => new(
            level.X,
            level.Y,
            level.Z
        );
    }

    public TerritoryType Territory
    {
        get => level.Territory.Value;
    }

    public bool IsInCurrentTerritory
    {
        get => Territory.RowId == client.CurrentTerritoryId;
    }

    public float Distance
    {
        get => IsInCurrentTerritory ? Vector3.Distance(player.GetPosition(), Position) : 0f;
    }
}
