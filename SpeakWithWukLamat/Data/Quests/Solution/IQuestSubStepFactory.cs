using Lumina.Excel.Sheets;

namespace SpeakWithWukLamat.Data.Quests.Solution;

public interface IQuestSubStepFactory
{
    QuestSubStep Create(Level level, Quest quest, byte sequence, byte step);
}
