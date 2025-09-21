using SpeakWithWukLamat.Data.Quests.Solution;

namespace SpeakWithWukLamat.Data.Quests;

public interface IQuestPatch
{
    QuestId Id { get; }
    
    void Patch(QuestData quest, QuestSolution solution);
}
