using System.Linq;
using Lumina.Excel.Sheets;
using Ocelot.Services.Data;
using Ocelot.Services.PlayerState;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Extensions.Data;
using SpeakWithWukLamat.Services.QuestManager;

namespace SpeakWithWukLamat.Services.QuestRequirementChecker;

public class QuestRequirementChecker(
    IQuestFactory factory,
    IQuestManager questManager,
    IDataRepository<ClassJob> jobs,
    IPlayer player
) : IQuestRequirementChecker
{
    public bool MeetsRequirements(QuestId questId)
    {
        var quest = factory.Create(questId);
        var data = quest.Data;

        var classJob = player.GetClassJob();
        if (!classJob.HasValue)
        {
            return false;
        }

        var requiredQuests = data.PreviousQuest.Where(q => q.IsValid).Select(q => new QuestId(q.RowId));
        foreach (var requiredQuest in requiredQuests)
        {
            if (!questManager.IsCompleted(requiredQuest))
            {
                return false;
            }
        }

        var classJobRequired = data.ClassJobRequired;
        if (classJobRequired.IsValid && classJobRequired.Value.RowId != classJob.Value.RowId)
        {
            return false;
        }

        if (data.ClassJobCategory0.IsValid && data.ClassJobLevel is [> 0, ..])
        {
            foreach (var job in data.ClassJobCategory0.Value.GetJobs(jobs))
            {
                if (player.GetLevel(job) < data.ClassJobLevel[0])
                {
                    return false;
                }
            }
        }

        if (data.ClassJobCategory1.IsValid && data.ClassJobLevel is [_, > 0, ..])
        {
            foreach (var job in data.ClassJobCategory1.Value.GetJobs(jobs))
            {
                if (player.GetLevel(job) < data.ClassJobLevel[1])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
