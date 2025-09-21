using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Data.Quests.Solution;

namespace SpeakWithWukLamat.Services.QuestSolver;

public class QuestTodoParser : IQuestTodoParser
{
    public QuestTodo Parse(QuestTodoArguments args)
    {
        var max = args.Max == 0 ? 1 : args.Max;

        return new QuestTodo(args.Progress, max, args.Other);
    }
}
