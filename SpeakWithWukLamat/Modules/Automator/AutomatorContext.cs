using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Services.QuestSelector;

namespace SpeakWithWukLamat.Modules.Automator;

public class AutomatorContext(IQuestSelector selector) : IAutomatorContext
{
    public bool IsActive { get; private set; }

    public void Toggle()
    {
        IsActive = !IsActive;
    }

    public QuestId? GetQuest()
    {
        return selector.Select();
    }
}
