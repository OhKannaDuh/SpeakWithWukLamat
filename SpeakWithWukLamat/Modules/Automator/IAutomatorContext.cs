using SpeakWithWukLamat.Data.Quests;

namespace SpeakWithWukLamat.Modules.Automator;

public interface IAutomatorContext
{
    bool IsActive { get; }

    void Toggle();

    void Stop()
    {
        if (IsActive)
        {
            Toggle();
        }
    }

    QuestId? GetQuest();
}
