using Lumina.Excel.Sheets;
using Ocelot.Services.ClientState;
using Ocelot.Services.Data;
using Ocelot.Services.PlayerState;

namespace SpeakWithWukLamat.Data.Npcs;

public class EventNpcFactory(
    IDataRepository<ENpcBase> npcBases,
    IDataRepository<ENpcResident> npcResidents,
    IClient client,
    IPlayer player,
    IDataRepository<uint, EventNpc> eventNpcs
) :  IEventNpcFactory
{
    public EventNpc Create(uint id, Level level)
    {
        if (eventNpcs.ContainsKey(id))
        {
            return eventNpcs.Get(id);
        }

        var eventNpc = new EventNpc(id, level, npcBases, npcResidents, client, player);
        eventNpcs.Add(id, eventNpc);

        return eventNpc;
    }
}
