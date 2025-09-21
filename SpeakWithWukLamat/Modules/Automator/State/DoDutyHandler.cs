using ECommons.GameHelpers;
using Ocelot.Mechanic.Services;
using Ocelot.States.Flow;

namespace SpeakWithWukLamat.Modules.Automator.State;

public class DoDutyHandler(
    IAutomatorContext context,
    IMechanicService mechanic
    
)
    : FlowStateHandler<AutomatorState>(AutomatorState.DoDuty)
{
    public override void Enter()
    {
        base.Enter();
        mechanic.Enable();
    }

    public override void Exit(AutomatorState next)
    {
        base.Exit(next);
        mechanic.Disable();
    }

    public override AutomatorState? Handle()
    {
        if (!Player.IsInDuty || !context.IsActive)
        {
            return AutomatorState.Idle;
        }

        return null;
    }
}
