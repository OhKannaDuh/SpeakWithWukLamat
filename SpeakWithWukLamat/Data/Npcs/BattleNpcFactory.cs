using Lumina.Excel.Sheets;
using Ocelot.Services.ClientState;
using Ocelot.Services.Data;
using Ocelot.Services.PlayerState;

namespace SpeakWithWukLamat.Data.Npcs;

public class BattleNpcFactory(
    IDataRepository<BNpcBase> npcBases,
    IDataRepository<BNpcName> npcNames,
    IClient client,
    IPlayer player,
    IDataRepository<uint, BattleNpc> battleNpcs
) : IBattleNpcFactory
{
    public BattleNpc Create(uint id, Level level)
    {
        if (battleNpcs.ContainsKey(id))
        {
            return battleNpcs.Get(id);
        }

        var battleNpc = new BattleNpc(id, level, npcBases, npcNames, client, player);
        battleNpcs.Add(id, battleNpc);

        return battleNpc;
    }
}
