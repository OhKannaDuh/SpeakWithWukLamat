using System.Collections.Generic;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Ocelot.Lifecycle;
using Ocelot.Services.Data;
using Ocelot.Services.Logger;
using SpeakWithWukLamat.Data.Quests;
using DalamudQuestManager = FFXIVClientStructs.FFXIV.Client.Game.QuestManager;

namespace SpeakWithWukLamat.Services.QuestManager;

public class QuestManager(IDataRepository<QuestId, Quest> quests, IQuestFactory factory, ILogger logger)
    : IQuestManager, IOnPreUpdate, IOnUpdate, IOnPostUpdate
{
    public UpdateLimit UpdateLimit
    {
        get => UpdateLimit.EverySecond;
    }

    public void PreUpdate()
    {
        HashSet<QuestId> toRemove = [];
        foreach (var questId in quests.GetKeys())
        {
            if (IsCompleted(questId) || !IsActive(questId))
            {
                toRemove.Add(questId);
            }
        }

        foreach (var questId in toRemove)
        {
            quests.Remove(questId);
        }
    }

    public unsafe void Update()
    {
        var manager = DalamudQuestManager.Instance();
        if (manager == null)
        {
            return;
        }

        foreach (ref var entry in manager->NormalQuests)
        {
            if (entry.QuestId == 0)
            {
                continue;
            }

            var id = new QuestId(entry.QuestId);
            if (quests.ContainsKey(id))
            {
                continue;
            }

            logger.Info("Adding quest {quest} to the active quests list", id);
            quests.Add(id, factory.Create(id));
        }
    }

    public unsafe void PostUpdate()
    {
        var manager = DalamudQuestManager.Instance();
        if (manager == null)
        {
            return;
        }

        var works = manager->NormalQuests.ToArray().Where(w => w.QuestId != 0).ToDictionary(w => w.QuestId, w => w);
        foreach (var quest in quests.GetAll())
        {
            if (!works.ContainsKey(quest.Id.JournalId))
            {
                continue;
            }

            quest.Update(works.First(w => w.Key == quest.Id.JournalId).Value);
        }
    }

    public IEnumerable<Quest> GetActiveQuests()
    {
        return quests.Where(q => IsActive(q.Id));
    }

    public unsafe QuestId? GetCurrentMainScenarioQuest()
    {
        var agent = AgentScenarioTree.Instance();
        if (agent == null)
        {
            return null;
        }

        var id = agent->Data->CurrentScenarioQuest;
        return id == 0 ? null : new QuestId(id);
    }

    public unsafe QuestId? GetPreviousMainScenarioQuest()
    {
        var agent = AgentScenarioTree.Instance();
        if (agent == null)
        {
            return null;
        }

        var id = agent->Data->CompleteScenarioQuest;
        return id == 0 ? null : new QuestId(id);
    }

    public unsafe bool IsActive(QuestId questId)
    {
        var manager = DalamudQuestManager.Instance();
        return manager != null && manager->IsQuestAccepted(questId.JournalId);
    }

    public bool IsCompleted(QuestId questId)
    {
        return DalamudQuestManager.IsQuestComplete(questId.JournalId);
    }

    public bool CanDo(QuestId questId)
    {
        if (IsCompleted(questId))
        {
            return false;
        }

        return true;
    }
}
