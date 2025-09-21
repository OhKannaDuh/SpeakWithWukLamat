using Ocelot.Services.Logger;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Data.Quests.Solution;

namespace SpeakWithWukLamat.Patches.ARealmReborn.MSQ;

[QuestPatch("Faye, Doug", "7.31")]
public class PrudenceAtThisJunctionQuestPatch(ILogger logger) : IQuestPatch
{
    // public QuestId Id { get; } = new(3852);
    public QuestId Id { get; } = new(533);

    public void Patch(QuestData data, QuestSolution solution)
    {
    }
}
