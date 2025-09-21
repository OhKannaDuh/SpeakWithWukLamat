using Lumina.Excel.Sheets;
using Ocelot.Services.Data;

namespace SpeakWithWukLamat.Data.Objects;

public class AetheryteFactory(IDataRepository<AetheryteData> aetheryteData, IDataRepository<uint, Aetheryte> aetherytes) : IAetheryteFactory
{
    public Aetheryte Create(uint id, Level level)
    {
        if (aetherytes.ContainsKey(id))
        {
            return aetherytes.Get(id);
        }

        var aetheryte = new Aetheryte(id, aetheryteData);
        aetherytes.Add(id, aetheryte);

        return aetheryte;
    }
}
