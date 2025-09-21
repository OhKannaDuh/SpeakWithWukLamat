using Ocelot.Services.Data;
using SpeakWithWukLamat.Services.QuestSolver;

namespace SpeakWithWukLamat.Data.Quests;

public class QuestFactory(IDataRepository<QuestData> quests, IQuestSolver solver) : IQuestFactory
{
    public Quest Create(QuestId id)
    {
        return new Quest(id, quests, solver);
    }
}
