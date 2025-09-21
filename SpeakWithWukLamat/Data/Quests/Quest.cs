using System;
using System.Linq;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Application.Network.WorkDefinitions;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using Ocelot.Services.Data;
using SpeakWithWukLamat.Data.Quests.Solution;
using SpeakWithWukLamat.Services.QuestSolver;

namespace SpeakWithWukLamat.Data.Quests;

public class Quest
{
    public readonly QuestId Id;

    public readonly QuestData Data;

    public readonly QuestSolution Solution;

    public QuestVars Vars { get; private set; }

    public byte Sequence;

    public unsafe QuestEventHandler* Handler
    {
        get
        {
            var handler = EventFramework.Instance()->GetEventHandlerById(Id.JournalId);
            if (handler == null)
            {
                return null;
            }

            return (QuestEventHandler*)handler;
        }
    }

    public string Name
    {
        get => Data.Name.ExtractText();
    }

    public Quest(QuestId questId, IDataRepository<QuestData> quests, IQuestSolver solver)
    {
        Id = questId;
        Data = quests.Get(questId.RowId);
        Solution = solver.Solve(this);
    }

    public void Update(QuestWork work)
    {
        Vars = QuestVars.FromWork(work);
        Sequence = work.Sequence;
    }

    public unsafe QuestTodoArguments GetTodoArgs(byte index)
    {
        var player = Player.BattleChara;
        if (Handler == null || player == null)
        {
            return new QuestTodoArguments();
        }

        uint a;
        uint b;
        uint c;

        Handler->GetTodoArgs(Player.BattleChara, index, &a, &b, &c);
        return new QuestTodoArguments(index, a, b, c);
    }

    public unsafe bool IsTodoChecked(byte index)
    {
        var player = Player.BattleChara;
        if (Handler == null || player == null)
        {
            return false;
        }

        return Handler->IsTodoChecked(player, index);
    }

    public QuestSubStep? GetCurrentSubStep()
    {
        if (!Solution.Sequences.TryGetValue(Sequence, out var sequence))
        {
            return null;
        }

        foreach (var step in sequence.Steps.Values)
        {
            if (step.IsTodoChecked(this))
            {
                continue;
            }

            var todo = step.GetTodoArgs(this);
            if (todo.Max > 1)
            {
                var index = todo.Progress;

                if (index > step.SubSteps.Count - 1)
                {
                    throw new InvalidOperationException("Not enough substeps to match progress");
                }

                return step.SubSteps[(int)index];
            }

            return step.SubSteps.First();
        }


        return null;
    }
}
