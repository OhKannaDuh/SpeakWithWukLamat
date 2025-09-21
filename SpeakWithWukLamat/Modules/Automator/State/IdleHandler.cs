using System.Linq;
using Ocelot.States.Flow;
using SpeakWithWukLamat.Services.QuestManager;

namespace SpeakWithWukLamat.Modules.Automator.State;

public class IdleHandler(IQuestManager manager, IAutomatorContext context) : FlowStateHandler<AutomatorState>(AutomatorState.Idle)
{
    public override void Enter()
    {
    }

    public override AutomatorState? Handle()
    {
        if (!context.IsActive)
        {
            return null;
        }

        var quests = manager.GetActiveQuests().ToList();
        if (quests.Count == 0)
        {
            return AutomatorState.PickUpQuest;
        }

        return AutomatorState.DoQuestStep;
    }
}
