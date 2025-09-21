using Ocelot.Lifecycle;
using Ocelot.States;
using SpeakWithWukLamat.Modules.Automator.State;

namespace SpeakWithWukLamat.Modules.Automator;

public class AutomatorModule(IStateMachine<AutomatorState> stateMachine) : IOnUpdate, IOnStart
{
    public void Update()
    {
        stateMachine.Update();
    }


    public void OnStart()
    {
    }
}
