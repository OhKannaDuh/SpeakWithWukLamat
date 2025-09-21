using System.Collections.Generic;
using System.Linq;
using Ocelot.Services.ClientState;
using Ocelot.Services.PlayerState;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Services.QuestListProvider;
using SpeakWithWukLamat.Services.QuestManager;

namespace SpeakWithWukLamat.Services.QuestSelector;

public class QuestSelector(
    IQuestManager questManager,
    IQuestListProvider questLists,
    IClient client,
    IPlayer player
) : IQuestSelector
{
    private readonly static Dictionary<uint, ushort> TerritoryToStarterQuest = new()
    {
        { 182, 594 }, // Ul' dah
        { 183, 39 }, // Gridania
    };

    public QuestId? Select()
    {
        var list = questLists.GetList();
        foreach (var quest in list.Quests)
        {
            if (questManager.CanDo(quest))
            {
                return quest;
            }
        }

        if (IsFreshCharacter())
        {
            var territory = client.CurrentTerritory;
            if (territory.HasValue && TerritoryToStarterQuest.TryGetValue(territory.Value.RowId, out var id))
            {
                return new QuestId(id);
            }
        }

        var active = questManager.GetActiveQuests().ToList();
        if (active.Any())
        {
            return active.Last().Id;
        }

        return questManager.GetCurrentMainScenarioQuest();
    }

    private bool IsFreshCharacter()
    {
        var level = player.GetLevel();
        if (level == 0)
        {
            return false;
        }

        return questManager.GetCurrentMainScenarioQuest() == null && questManager.GetPreviousMainScenarioQuest() == null && level == 1;
    }
}
