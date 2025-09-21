using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommons.GameHelpers;
using Ocelot.Chain;
using Ocelot.Services.Logger;
using Ocelot.States.Flow;
using SpeakWithWukLamat.Services.QuestManager;

namespace SpeakWithWukLamat.Modules.Automator.State;

public class DoQuestStepHandler(
    IAutomatorContext context,
    IQuestManager quests,
    IChainFactory chains,
    ILogger logger
) : FlowStateHandler<AutomatorState>(AutomatorState.DoQuestStep)
{
    private Task<ChainResult>? CurrentStep = null;

    private CancellationTokenSource cancel = new();

    public override void Exit(AutomatorState next)
    {
        base.Exit(next);
        cancel.Cancel();
    }

    public override AutomatorState? Handle()
    {
        var id = context.GetQuest();
        if (!context.IsActive || id == null)
        {
            return AutomatorState.Idle;
        }

        if (Player.IsInDuty)
        {
            return AutomatorState.DoDuty;
        }

        if (CurrentStep != null)
        {
            if (CurrentStep.IsCompleted)
            {
                CurrentStep = null;
            }

            return null;
        }

        var quest = quests.GetActiveQuests().FirstOrDefault(q => q.Id == id);
        if (quest == null)
        {
            logger.Info("Quest not found");
            return AutomatorState.Idle;
        }

        var step = quest.GetCurrentSubStep();
        if (step == null)
        {
            logger.Info("subStep not found");
            return AutomatorState.Idle;
        }

        cancel = new CancellationTokenSource();

        logger.Info(step.GetDescription());
        CurrentStep = step.GetChain(chains).ExecuteAsync(cancel.Token);

        return null;
    }
}
