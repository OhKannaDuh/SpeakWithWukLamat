using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Data.Quests.Solution;

namespace SpeakWithWukLamat.Services.QuestSolver;

public interface IQuestSolver
{
    QuestSolution Solve(Quest quest);
}
