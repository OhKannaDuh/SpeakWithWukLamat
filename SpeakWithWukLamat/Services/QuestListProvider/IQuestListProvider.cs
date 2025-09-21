using SpeakWithWukLamat.Data.QuestLists;

namespace SpeakWithWukLamat.Services.QuestListProvider;

public interface IQuestListProvider
{
    QuestList GetList();
}
