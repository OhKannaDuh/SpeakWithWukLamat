using System.Linq;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using SpeakWithWukLamat.Services.QuestSolver;
using Quest = SpeakWithWukLamat.Data.Quests.Quest;

namespace SpeakWithWukLamat.Renderers;

public static class QuestSolutionRenderer
{
    public static void Render(Quest quest, IQuestTodoParser todoParser)
    {
        var currentSequence = quest.Sequence;
        var flow = quest.Solution;

        if (flow.Sequences.Count == 0)
        {
            ImGui.TextDisabled("No sequences to display.");
            return;
        }

        foreach (var seqKv in flow.Sequences.OrderBy(kv => kv.Key))
        {
            var seqId = seqKv.Key;
            var seq = seqKv.Value;
            var sequenceComplete = seqId < currentSequence;

            ImGui.PushID($"seq_{seqId}");

            var headerOpen = ImGui.CollapsingHeader($"Sequence {seqId} ({(sequenceComplete ? "✓" : "X")})##{quest.Id}", ImGuiTreeNodeFlags.DefaultOpen);
            if (headerOpen)
            {
                ImGui.Indent();
                var steps = seq.Steps;
                if (steps.Count == 0)
                {
                    ImGui.TextDisabled("No steps in this sequence.");
                    ImGui.PopID();
                    ImGui.Indent();
                    continue;
                }

                var tableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;

                if (ImGui.BeginTable($"steps_{seqId}##{quest.Id}", 4, tableFlags))
                {
                    ImGui.TableSetupColumn("Step", ImGuiTableColumnFlags.WidthFixed, 70f);
                    ImGui.TableSetupColumn("Progress", ImGuiTableColumnFlags.WidthFixed, 160f);
                    ImGui.TableSetupColumn("Args");
                    ImGui.TableSetupColumn("Levels", ImGuiTableColumnFlags.WidthStretch);
                    ImGui.TableHeadersRow();

                    foreach (var stepKv in steps.OrderBy(kv => kv.Key))
                    {
                        var stepId = stepKv.Key;
                        var step = stepKv.Value;

                        ImGui.TableNextRow();

                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextUnformatted(stepId.ToString());

                        if (seqId == currentSequence)
                        {
                            var args = step.GetTodoArgs(quest);
                            var todo = todoParser.Parse(args);

                            ImGui.TableSetColumnIndex(1);
                            ImGui.ProgressBar(todo.Percentage, new Vector2(160f, 0f), todo.Text);

                            ImGui.TableSetColumnIndex(2);
                            ImGui.Text($"Progress: {todo.Progress}, Max: {todo.Max}, Other: {todo.Argument}");
                        }
                        else
                        {
                            ImGui.TableSetColumnIndex(1);
                            ImGui.Text("N/A");
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.SetTooltip("Only available for current sequence.");
                            }

                            ImGui.TableSetColumnIndex(2);
                            ImGui.Text("N/A");
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.SetTooltip("Only available for current sequence.");
                            }
                        }

                        ImGui.TableSetColumnIndex(3);
                        if (step.SubSteps.Count == 0)
                        {
                            ImGui.TextDisabled("<no steps found>");
                        }
                        else
                        {
                            var first = true;
                            foreach (var subStep in step.SubSteps)
                            {
                                if (!first)
                                {
                                    ImGui.TableNextRow();
                                    ImGui.TableSetColumnIndex(3);
                                }

                                ImGui.PushTextWrapPos(0);
                                ImGui.Text(subStep.GetDescription());
                                ImGui.PopTextWrapPos();
                                first = false;
                            }
                        }
                    }

                    ImGui.EndTable();
                }

                ImGui.Unindent();
            }

            ImGui.PopID();
        }
    }
}
