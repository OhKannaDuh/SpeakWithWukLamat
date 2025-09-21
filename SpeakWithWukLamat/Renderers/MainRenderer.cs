using System.Linq;
using System.Reflection;
using Dalamud.Bindings.ImGui;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using Lumina.Excel.Sheets;
using Ocelot.Services.Data;
using Ocelot.Services.WindowManager;
using Ocelot.States;
using Ocelot.UI.Services;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Modules.Automator;
using SpeakWithWukLamat.Modules.Automator.State;
using SpeakWithWukLamat.Services.QuestManager;
using SpeakWithWukLamat.Services.QuestSelector;
using SpeakWithWukLamat.Services.QuestSolver;
using Quest = SpeakWithWukLamat.Data.Quests.Quest;

namespace SpeakWithWukLamat.Renderers;

public class MainRenderer(
    IQuestManager manager,
    IQuestSelector selector,
    IQuestFactory factory,
    IAutomatorContext context,
    IUIService ui,
    IQuestTodoParser todoParser,
    IStateMachine<AutomatorState> stateMachine,
    IDataRepository<Status> statuses) : IMainRenderer
{
    public void Render()
    {
        foreach (var status in Player.Status)
        {
            var impl = statuses.Get(status.StatusId);
            ui.Text(impl.Name);
        }

        foreach (var condition in Svc.Condition.AsReadOnlySet())
        {
            ui.Text(condition);
        }

        ImGui.Separator();
        RenderAutomator();
        ImGui.Separator();

        if (manager.GetActiveQuests().Any())
        {
            RenderActiveQuests();
        }
        else
        {
            var candidate = selector.Select();
            if (candidate == null)
            {
                return;
            }

            var quest = factory.Create(candidate.Value);
            RenderQuest(quest);
        }
    }

    private void RenderAutomator()
    {
        ui.LabelledValue("Automator State", context.IsActive ? "Active" : "Inactive");
        ui.LabelledValue("State Machine", stateMachine.State);

        if (ImGui.Button("Toggle Automator"))
        {
            context.Toggle();
        }
    }

    private void RenderActiveQuests()
    {
        foreach (var quest in manager.GetActiveQuests())
        {
            RenderQuest(quest);
        }
    }

    private void RenderQuest(Quest quest)
    {
        if (!ImGui.CollapsingHeader($"{quest.Name} (Sequence: {quest.Sequence})##({quest.Id})"))
        {
            return;
        }

        ImGui.Indent();
        
        if (quest.Solution.QuestPatch != null)
        {
            var patch = quest.Solution.QuestPatch;
            var attr = patch.GetType().GetCustomAttribute<QuestPatchAttribute>();
            if (attr != null)
            {
                ui.LabelledValue("Patch Author(s)", attr.Author);
                ui.LabelledValue("Patch Game Patch", attr.GamePatch);
            }
        }

        unsafe
        {
            if (quest.Handler != null)
            {
                ui.LabelledValue("Quest script", quest.Handler->ScriptPath);
            }
        }


        if (ImGui.CollapsingHeader($"Params##{quest.Id}"))
        {
            ImGui.Indent();

            foreach (var param in quest.Data.QuestParams)
            {
                if (param.ScriptArg == 0)
                {
                    continue;
                }

                var associations = quest.Solution.CountAssociations(param);
                ui.LabelledValue(param.ScriptInstruction, $"{param.ScriptArg} (Associated Steps: {associations})");
            }

            ImGui.Unindent();
        }

        if (ImGui.CollapsingHeader($"Listener Params##{quest.Id}"))
        {
            ImGui.Indent();

            var index = 0;
            foreach (var param in quest.Data.QuestListenerParams)
            {
                if (param.Listener == 0)
                {
                    continue;
                }

                var associations = quest.Solution.CountAssociations(param);
                index++;
                if (ImGui.CollapsingHeader($"{index} - {param.Listener} (Associated Steps: {associations})##{quest.Id}"))
                {
                    ImGui.Indent();
                    if (param.ConditionValue > 0)
                    {
                        ui.LabelledValue("Condition Value", param.ConditionValue);
                    }

                    if (param.Behavior > 0)
                    {
                        ui.LabelledValue("Behavior", param.Behavior);
                    }

                    ui.LabelledValue("Actor Spawn Seq", param.ActorSpawnSeq);

                    if (param.ActorDespawnSeq > 0)
                    {
                        ui.LabelledValue("Actor Despawn Seq", param.ActorDespawnSeq);
                    }

                    ui.LabelledValue("QuestUInt8A", param.QuestUInt8A);
                    if (param.ConditionType > 0)
                    {
                        ui.LabelledValue("Condition Type", param.ConditionType);
                    }

                    if (param.ConditionOperator > 0)
                    {
                        ui.LabelledValue("Condition Operator", param.ConditionOperator);
                    }

                    ui.LabelledValue("Visible Bool", param.VisibleBool);
                    ui.LabelledValue("Condition Bool", param.ConditionBool);
                    ui.LabelledValue("Item Bool", param.ItemBool);
                    ui.LabelledValue("Announce Bool", param.AnnounceBool);
                    ui.LabelledValue("Behavior Bool", param.BehaviorBool);
                    ui.LabelledValue("Accept Bool", param.AcceptBool);
                    ui.LabelledValue("Qualified Bool", param.QualifiedBool);
                    ui.LabelledValue("Can Target Bool", param.CanTargetBool);
                    ImGui.Unindent();
                }
            }

            ImGui.Unindent();
        }

        if (ImGui.CollapsingHeader($"Sequences and Steps##{quest.Id}"))
        {
            ImGui.Indent();
            QuestSolutionRenderer.Render(quest, todoParser);
            ImGui.Unindent();
        }


        ImGui.Unindent();
    }
}
