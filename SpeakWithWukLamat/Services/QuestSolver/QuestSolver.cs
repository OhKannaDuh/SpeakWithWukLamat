using System.Linq;
using Ocelot.Services.Data;
using Ocelot.Services.Logger;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Data.Quests.Solution;
using Quest = SpeakWithWukLamat.Data.Quests.Quest;

namespace SpeakWithWukLamat.Services.QuestSolver;

public class QuestSolver(
    IDataRepository<QuestId, QuestSolution> solutions,
    IQuestSubStepFactory subStepFactory,
    IDataRepository<QuestId, IQuestPatch> patches,
    ILogger logger
) : IQuestSolver
{
    public QuestSolution Solve(Quest quest)
    {
        if (solutions.ContainsKey(quest.Id))
        {
            return solutions.Get(quest.Id);
        }

        var sequences = quest.Data.TodoParams
            .Where(q => q.ToDoCompleteSeq != 0)
            .GroupBy(q => q.ToDoCompleteSeq)
            .ToDictionary(g => g.Key, g => g.ToList());

        byte stepIndex = 0;
        var solution = new QuestSolution();

        foreach (var (sequenceIndex, steps) in sequences)
        {
            var sequence = new QuestSequence(sequenceIndex);

            byte subStepIndex = 0;
            foreach (var step in steps)
            {
                var levels = step.ToDoLocation.Where(l => l.IsValid).Select(l => l.Value).ToList();

                sequence.AddStep(stepIndex, new QuestStep(stepIndex)
                {
                    CountableNum = step.CountableNum,
                    ToDoQty = step.ToDoQty,
                    SubSteps = levels.Select(level => subStepFactory.Create(level, quest, sequenceIndex, subStepIndex++)).ToList(),
                });

                stepIndex++;
            }

            solution.AddSequence(sequenceIndex, sequence);
        }

        if (patches.ContainsKey(quest.Id))
        {
            logger.Info("Patching quest {name}", quest.Data.Name.ExtractText());
            var patch = patches.Get(quest.Id);
            solution.SetQuestPatch(patch);

            patch.Patch(quest.Data, solution);
        }

        solutions.Add(quest.Id, solution);

        return solution;
    }
}
