using Dalamud.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Ocelot;
using Ocelot.Chain.Services;
using Ocelot.ECommons.Services;
using Ocelot.Mechanic.Services;
using Ocelot.Pathfinding.Services;
using Ocelot.Pictomancy.Services;
using Ocelot.Rotation.Services;
using Ocelot.Services.WindowManager;
using Ocelot.States;
using Ocelot.UI.Services;
using SpeakWithWukLamat.Config;
using SpeakWithWukLamat.Data.Npcs;
using SpeakWithWukLamat.Data.Objects;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Data.Quests.Solution;
using SpeakWithWukLamat.Modules.Automator;
using SpeakWithWukLamat.Modules.Automator.State;
using SpeakWithWukLamat.Modules.Quests;
using SpeakWithWukLamat.Renderers;
using SpeakWithWukLamat.Services.QuestListProvider;
using SpeakWithWukLamat.Services.QuestManager;
using SpeakWithWukLamat.Services.QuestRequirementChecker;
using SpeakWithWukLamat.Services.QuestSelector;
using SpeakWithWukLamat.Services.QuestSolver;

namespace SpeakWithWukLamat;

public sealed class Plugin(IDalamudPluginInterface plugin) : OcelotPlugin(plugin)
{
    private readonly IDalamudPluginInterface plugin = plugin;

    protected override void Boostrap(IServiceCollection services)
    {
        BootstrapOcelotModules(services);
        BootstrapConfiguration(services);
        BootstrapServices(services);
        BootstrapModules(services);
    }

    private static void BootstrapOcelotModules(IServiceCollection services)
    {
        services.LoadECommons();
        services.LoadPictomancy();
        services.LoadPathfinding();
        services.LoadMechanics();
        services.LoadRotations();
        services.LoadChain();
        services.LoadUI();
    }

    private void BootstrapConfiguration(IServiceCollection services)
    {
        services.AddSingleton(plugin.GetPluginConfig() as Configuration ?? new Configuration(plugin));
        services.AddSingleton<DetourConfig>(c => c.GetRequiredService<Configuration>().Detour);
    }

    private static void BootstrapServices(IServiceCollection services)
    {
        services.AddSingleton<IEventNpcFactory, EventNpcFactory>();
        services.AddSingleton<IBattleNpcFactory, BattleNpcFactory>();
        services.AddSingleton<IAetheryteFactory, AetheryteFactory>();

        services.AddSingleton<IQuestFactory, QuestFactory>();
        services.AddSingleton<IQuestManager, QuestManager>();
        services.AddSingleton<IQuestSelector, QuestSelector>();
        services.AddSingleton<IQuestSolver, QuestSolver>();
        services.AddSingleton<IQuestTodoParser, QuestTodoParser>();
        services.AddSingleton<IQuestListProvider, QuestListProvider>();
        services.AddSingleton<IQuestRequirementChecker, QuestRequirementChecker>();
        services.AddSingleton<IQuestSubStepFactory, QuestSubStepFactory>();

        services.AddSingleton<IMainRenderer, MainRenderer>();
        services.AddSingleton<IConfigRenderer, ConfigRenderer>();
    }

    private static void BootstrapModules(IServiceCollection services)
    {
        services.AddSingleton<QuestModule>();
        services.AddSingleton<AutomatorModule>();
        services.AddSingleton<IAutomatorContext, AutomatorContext>();
        services.AddFlowStateMachine(AutomatorState.Idle);
    }
}
