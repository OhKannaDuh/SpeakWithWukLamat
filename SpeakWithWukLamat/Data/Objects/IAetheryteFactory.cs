using Lumina.Excel.Sheets;

namespace SpeakWithWukLamat.Data.Objects;

public interface IAetheryteFactory
{
    Aetheryte Create(uint id, Level level);
}
