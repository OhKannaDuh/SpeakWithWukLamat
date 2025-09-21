using SpeakWithWukLamat.Data.Quests;

namespace SpeakWithWukLamat.Services.QuestSelector;

public interface IQuestSelector
{
    QuestId? Select();
}
