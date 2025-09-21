using Lumina.Excel.Sheets;

namespace SpeakWithWukLamat.Data.Npcs;

public interface IEventNpcFactory
{
    EventNpc Create(uint id, Level level);
}
