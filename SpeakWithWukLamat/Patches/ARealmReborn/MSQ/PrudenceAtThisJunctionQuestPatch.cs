using Ocelot.Services.Logger;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Data.Quests.Solution;

namespace SpeakWithWukLamat.Patches.ARealmReborn.MSQ;

[QuestPatch("Faye", "7.31")]
public class PrudenceAtThisJunctionQuestPatch(ILogger logger) : IQuestPatch
{
    public QuestId Id { get; } = new(3852);

    public void Patch(QuestData data, QuestSolution solution)
    {
        logger.Debug("This was just made to test the patching system for now.");
        // What is needed for this quest?
        // Need to equip items of at least ilvl 5 in the 5 main slots
        // This should just mean ensuring we buy a hat from the nearby merchant and equipping it
    }
}
