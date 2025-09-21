using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine;
using Lumina.Excel.Sheets;
using Ocelot.Services.Data;
using SpeakWithWukLamat.Data.Npcs;
using SpeakWithWukLamat.Data.Objects;
using SpeakWithWukLamat.Extensions.Data;

namespace SpeakWithWukLamat.Data.Quests.Solution;

public class QuestSubStepFactory(
    IBattleNpcFactory battleNpcFactory,
    IEventNpcFactory eventNpcFactory,
    IAetheryteFactory aetheryteFactory,
    IDataRepository<int, QuestSubStep> subSteps
) : IQuestSubStepFactory
{
    public QuestSubStep Create(Level level, Quest quest, byte sequence, byte step)
    {
        var subStepKey = new HashCode();
        subStepKey.Add(level.RowId);
        subStepKey.Add(step);
        var id = subStepKey.ToHashCode();

        if (subSteps.ContainsKey(id))
        {
            return subSteps.Get(id);
        }

        var subStep = new QuestSubStep(level, quest, sequence, step);

        var type = (InstanceType)level.Type;
        if (type == InstanceType.QuestMarker)
        {
            var listenersForSequence = quest.Data.QuestListenerParams.Where(l => l.ActorSpawnSeq == sequence).ToList();
            if (step >= listenersForSequence.Count)
            {
                return subStep;
            }

            var listener = listenersForSequence[step];

            var paramsForListener = quest.Data.QuestParams.Where(p => p.ScriptArg == listener.Listener).ToList();
            if (!paramsForListener.Any())
            {
                return subStep;
            }

            subStep.AddListener(listener);
            subStep.AddQuestParams(paramsForListener);

            foreach (var param in paramsForListener)
            {
                if (param.GetInstructionKey() == "ENEMY")
                {
                    subStep.WithBattleNpc(battleNpcFactory.Create(listener.ConditionValue, level));
                }
            }

            subSteps.Add(id, subStep);
            return subStep;
        }

        var questParams = quest.Data.QuestParams.Where(p => p.ScriptArg == level.Object.RowId).ToList();
        subStep.AddQuestParams(questParams);
        subStep.AddListeners(quest.Data.QuestListenerParams.Where(p => p.Listener == level.Object.RowId).ToList());

        foreach (var param in questParams)
        {
            var key = Regex.Replace(param.ScriptInstruction.ExtractText(), @"\d+$", "");
            if (key == "ACTOR")
            {
                subStep.WithEventNpc(eventNpcFactory.Create(param.ScriptArg, level));
            }

            if (key == "AETHERYTE")
            {
                subStep.WithAetheryte(aetheryteFactory.Create(param.ScriptArg, level));
            }
        }

        subSteps.Add(id, subStep);
        return subStep;
    }
}
