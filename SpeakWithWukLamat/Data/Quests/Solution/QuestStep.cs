using System.Collections.Generic;

namespace SpeakWithWukLamat.Data.Quests.Solution;

public sealed class QuestStep(byte step)
{
    public byte CountableNum { get; init; }

    public byte ToDoQty { get; init; }

    public List<QuestSubStep> SubSteps { get; init; } = new();

    public QuestTodoArguments GetTodoArgs(Quest quest)
    {
        return quest.GetTodoArgs(step);
    }

    public bool IsTodoChecked(Quest quest)
    {
        return quest.IsTodoChecked(step);
    }
}
