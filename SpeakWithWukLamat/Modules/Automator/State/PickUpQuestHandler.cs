using System;
using System.Numerics;
using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using Lumina.Excel.Sheets;
using Ocelot.Pathfinding;
using Ocelot.Pathfinding.Extensions;
using Ocelot.Pathfinding.Services;
using Ocelot.Services.ClientState;
using Ocelot.Services.Data;
using Ocelot.Services.Logger;
using Ocelot.Services.Pathfinding;
using Ocelot.Services.PlayerState;
using Ocelot.States.Flow;
using SpeakWithWukLamat.Data.Npcs;
using SpeakWithWukLamat.Data.Quests;
using SpeakWithWukLamat.Services.QuestManager;

namespace SpeakWithWukLamat.Modules.Automator.State;

public class PickUpQuestHandler(
    IAutomatorContext context,
    IQuestManager manager,
    IQuestFactory questFactory,
    IPathfinder pathfinder,
    IClient client,
    IPlayer player,
    IEventNpcFactory eventNpcFactory,
    ILogger logger
) : FlowStateHandler<AutomatorState>(AutomatorState.PickUpQuest)
{
    public override AutomatorState? Handle()
    {
        if (!context.IsActive)
        {
            return AutomatorState.Idle;
        }

        var questId = context.GetQuest();
        if (questId == null || manager.IsActive(questId.Value))
        {
            return AutomatorState.Idle;
        }

        var quest = questFactory.Create(questId.Value);
        var issuerLocation = quest.Data.IssuerLocation;
        if (!issuerLocation.IsValid || !issuerLocation.Value.Territory.IsValid)
        {
            return AutomatorState.Idle;
        }

        var npc = eventNpcFactory.Create(quest.Data.IssuerLocation.Value.Object.RowId, quest.Data.IssuerLocation.Value);
        var destination = new Vector3(issuerLocation.Value.X, issuerLocation.Value.Y, issuerLocation.Value.Z);
        var territory = issuerLocation.Value.Territory.Value;
        var isNear = IsNearIssuer(destination, territory);

        if (pathfinder.IsIdle() && !isNear)
        {
            logger.Log("Moving to destination {npcName}", npc.Name);
            pathfinder.PathfindAndMoveTo(new Path(destination)
            {
                DistanceThreshold = 3f,
                TerritoryType = territory,
            });
        }

        if (EzThrottler.Throttle("PickUpQuestHandler.Target") && isNear && Svc.Targets.Target == null)
        {
            Svc.Targets.Target = npc.GameObject;
        }

        if (Svc.Targets.Target != null && !IsInteracting())
        {
            unsafe
            {
                var target = TargetSystem.Instance();
                if (target == null)
                {
                    return null;
                }
                
                target->InteractWithObject(Svc.Targets.Target.Struct(), false);
            }
        }

        return null;
    }

    private bool IsNearIssuer(Vector3 position, TerritoryType territory, float distance = 5f)
    {
        if (client.CurrentTerritoryId != territory.RowId)
        {
            return false;
        }

        return Vector3.Distance(player.GetPosition(), position) <= distance;
    }

    private bool IsInteracting()
    {
        return Svc.Condition.Any([ConditionFlag.OccupiedInEvent, ConditionFlag.OccupiedInQuestEvent, ConditionFlag.OccupiedInCutSceneEvent]);
    }
}
