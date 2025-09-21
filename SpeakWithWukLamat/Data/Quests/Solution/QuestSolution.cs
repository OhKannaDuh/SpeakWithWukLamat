using System.Collections.Generic;
using System.Linq;

namespace SpeakWithWukLamat.Data.Quests.Solution;

public sealed class QuestSolution
{
    private readonly Dictionary<byte, QuestSequence> sequences = [];

    public IReadOnlyDictionary<byte, QuestSequence> Sequences
    {
        get => sequences;
    }

    public void AddSequence(byte index, QuestSequence sequence)
    {
        sequences[index] = sequence;
    }

    public bool TryGetSequence(byte index, out QuestSequence sequence)
    {
        return sequences.TryGetValue(index, out sequence!);
    }
    
    public IQuestPatch? QuestPatch { get; private set; }

    public int CountAssociations(QuestData.QuestParamsStruct param)
    {
        bool Is(QuestData.QuestParamsStruct a, QuestData.QuestParamsStruct b)
        {
            return a.ScriptArg == b.ScriptArg && a.ScriptInstruction == b.ScriptInstruction;
        }

        return sequences.Values
            .SelectMany(seq => seq.Steps.Values)
            .SelectMany(step => step.SubSteps)
            .SelectMany(subStep => subStep.Params)
            .Count(p => Is(p, param));
    }

    public int CountAssociations(QuestData.QuestListenerParamsStruct param)
    {
        bool Is(QuestData.QuestListenerParamsStruct a, QuestData.QuestListenerParamsStruct b)
        {
            if (a.Listener != b.Listener)
            {
                return false;
            }

            if (a.ConditionValue != b.ConditionValue)
            {
                return false;
            }

            if (a.Behavior != b.Behavior)
            {
                return false;
            }

            if (a.ActorSpawnSeq != b.ActorSpawnSeq)
            {
                return false;
            }

            if (a.ActorDespawnSeq != b.ActorDespawnSeq)
            {
                return false;
            }

            if (a.QuestUInt8A != b.QuestUInt8A)
            {
                return false;
            }

            if (a.ConditionType != b.ConditionType)
            {
                return false;
            }

            if (a.ConditionOperator != b.ConditionOperator)
            {
                return false;
            }

            if (a.VisibleBool != b.VisibleBool)
            {
                return false;
            }

            if (a.ConditionBool != b.ConditionBool)
            {
                return false;
            }

            if (a.ItemBool != b.ItemBool)
            {
                return false;
            }

            if (a.AnnounceBool != b.AnnounceBool)
            {
                return false;
            }

            if (a.BehaviorBool != b.BehaviorBool)
            {
                return false;
            }

            if (a.AcceptBool != b.AcceptBool)
            {
                return false;
            }

            if (a.QualifiedBool != b.QualifiedBool)
            {
                return false;
            }

            if (a.CanTargetBool != b.CanTargetBool)
            {
                return false;
            }

            return true;
        }

        return sequences.Values
            .SelectMany(seq => seq.Steps.Values)
            .SelectMany(step => step.SubSteps)
            .SelectMany(subStep => subStep.Listeners)
            .Count(p => Is(p, param));
    }

    public void SetQuestPatch(IQuestPatch questPatch)
    {
        QuestPatch = questPatch;
    }
}
