using System.Collections.Generic;
using SpeakWithWukLamat.Data.Quests;

namespace SpeakWithWukLamat.Services.QuestManager;

public interface IQuestManager
{
    IEnumerable<Quest> GetActiveQuests();

    QuestId? GetCurrentMainScenarioQuest();

    QuestId? GetPreviousMainScenarioQuest();

    bool IsActive(QuestId questId);

    bool IsCompleted(QuestId questId);

    bool CanDo(QuestId questId);
}
