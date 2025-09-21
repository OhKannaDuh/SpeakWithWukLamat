namespace SpeakWithWukLamat.Data.Quests;

public interface IQuestFactory
{
    Quest Create(QuestId id);
}
