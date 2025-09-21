using System.Collections.Generic;

namespace SpeakWithWukLamat.Data.Quests.Solution;

public sealed class QuestSequence(byte sequence)
{
    public byte Sequence
    {
        get => sequence;
    }

    private readonly Dictionary<byte, QuestStep> steps = [];

    public IReadOnlyDictionary<byte, QuestStep> Steps
    {
        get => steps;
    }

    public void AddStep(byte step, QuestStep questStep)
    {
        steps[step] = questStep;
    }
}
