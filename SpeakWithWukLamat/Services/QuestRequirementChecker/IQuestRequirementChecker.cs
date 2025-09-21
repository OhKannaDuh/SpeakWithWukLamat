using SpeakWithWukLamat.Data.Quests;

namespace SpeakWithWukLamat.Services.QuestRequirementChecker;

public interface IQuestRequirementChecker
{
    bool MeetsRequirements(QuestId questId);
}
