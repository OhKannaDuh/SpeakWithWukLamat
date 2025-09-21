using Lumina.Excel.Sheets;

namespace SpeakWithWukLamat.Data.Npcs;

public interface IBattleNpcFactory
{
    BattleNpc Create(uint id, Level level);
}
