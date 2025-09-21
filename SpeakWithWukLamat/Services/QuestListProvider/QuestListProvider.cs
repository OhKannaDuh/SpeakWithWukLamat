using SpeakWithWukLamat.Data.QuestLists;

namespace SpeakWithWukLamat.Services.QuestListProvider;

public class QuestListProvider : IQuestListProvider
{
    public QuestList GetList()
    {
        // Empty, this is just a concept right now
        return new QuestList();
    }
}
