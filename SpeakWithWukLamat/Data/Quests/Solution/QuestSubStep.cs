using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine;
using Lumina.Excel.Sheets;
using Ocelot.Chain;
using Ocelot.Chain.Extensions;
using Ocelot.Chain.Recipes;
using Ocelot.Services.Pathfinding;
using SpeakWithWukLamat.Data.Npcs;
using SpeakWithWukLamat.Extensions.Data;
using Aetheryte = SpeakWithWukLamat.Data.Objects.Aetheryte;

namespace SpeakWithWukLamat.Data.Quests.Solution;

public class QuestSubStep(Level level, Quest quest, byte sequence, byte step)
{
    private readonly Quest Quest = quest;

    private readonly byte Sequence = sequence;

    private readonly byte Step = step;

    public Level Level
    {
        get => level;
    }

    public List<QuestData.QuestParamsStruct> Params { get; private set; } = [];

    public List<QuestData.QuestListenerParamsStruct> Listeners { get; private set; } = [];

    public EventNpc? EventNpc { get; private set; }

    public BattleNpc? BattleNpc { get; private set; }

    public Aetheryte? Aetheryte { get; private set; }

    public void AddQuestParam(QuestData.QuestParamsStruct associateParam)
    {
        Params.Add(associateParam);
    }

    public void AddQuestParams(IEnumerable<QuestData.QuestParamsStruct> associatedParams)
    {
        Params.AddRange(associatedParams);
    }

    public void AddListener(QuestData.QuestListenerParamsStruct associateListenerParam)
    {
        Listeners.Add(associateListenerParam);
    }

    public void AddListeners(IEnumerable<QuestData.QuestListenerParamsStruct> associateListenerParam)
    {
        Listeners.AddRange(associateListenerParam);
    }

    public void WithEventNpc(EventNpc eventNpc)
    {
        EventNpc = eventNpc;
    }

    public void WithBattleNpc(BattleNpc battleNpc)
    {
        BattleNpc = battleNpc;
    }

    public void WithAetheryte(Aetheryte aetheryte)
    {
        Aetheryte = aetheryte;
    }

    public string GetDescription()
    {
        var type = (InstanceType)level.Type;
        switch (type)
        {
            case InstanceType.EventNpc:
                return $"Interact with {EventNpc?.Name ?? "Unknown EventNpc"}  ({level.RowId}:{EventNpc?.BaseData.RowId})";
            case InstanceType.Aetheryte:
                return $"Interact with Aetheryte {Aetheryte?.Data.RowId.ToString() ?? "Unknown"}  ({level.RowId}:{Aetheryte?.Data.RowId})";
            case InstanceType.QuestMarker:
                if (Params.Count == 0 && Listeners.Count == 0)
                {
                    break;
                }

                var sb = new StringBuilder();
                foreach (var param in Params)
                {
                    sb.AppendLine($"{param.ScriptInstruction}: {param.ScriptArg} ({type})");

                    var key = param.GetInstructionKey();
                    if (key == "ENEMY")
                    {
                        var listeners = Listeners.Where(l => l.Listener == param.ScriptArg);
                        var mobs = new List<BNpcName>();
                        foreach (var listener in listeners)
                        {
                            mobs.Add(Svc.Data.GetExcelSheet<BNpcName>().GetRow(listener.ConditionValue));
                        }

                        foreach (var mob in mobs)
                        {
                            sb.AppendLine("  - " + mob.Singular.ExtractText());
                        }
                    }
                }

                foreach (var listener in Listeners)
                {
                    sb.AppendLine($"{listener.Listener}: {listener.ConditionValue}");
                }

                return sb.ToString();
        }

        return $"{level.RowId}:{level.Object.RowId} {type} (Unknown)";
    }

    public IChain GetChain(IChainFactory chains)
    {
        var type = (InstanceType)level.Type;
        switch (type)
        {
            case InstanceType.EventNpc:
                if (EventNpc == null)
                {
                    throw new InvalidOperationException("Event Npc was null for a EventNpc quest Substep");
                }

                return chains.Create("Interact with Npc")
                    .Then<PathfindToChain, Path>(new Path(EventNpc.Position)
                    {
                        DistanceThreshold = 4f,
                    })
                    .Then<InteractChain, Func<IGameObject?>>(() => EventNpc.GameObject);

            case InstanceType.Aetheryte:

                if (Aetheryte == null)
                {
                    throw new InvalidOperationException("Aetheryte was null for a Aetheryte quest Substep");
                }

                return chains.Create("Interact with Aetheryte")
                    .Then<PathfindToChain, Path>(new Path(Aetheryte.Position)
                    {
                        DistanceThreshold = 6f,
                    })
                    .Then<InteractChain, Func<IGameObject?>>(() => Aetheryte.GameObject);

            case InstanceType.QuestMarker:
                var param = Params.FirstOrNull();
                if (param == null)
                {
                    throw new InvalidOperationException("Could not find param for QuestMarker Substep");
                }

                var listener = Listeners.FirstOrNull();
                if (listener == null)
                {
                    throw new InvalidOperationException("Could not find listener for QuestMarker Substep");
                }

                var pos = new Vector3(level.X * 0.16f, level.Y, level.Z);

                var chain = chains.Create("Quest Marker")
                    .Then<PathfindToChain, Path>(new Path(pos)
                    {
                        DistanceThreshold = level.Radius * 0.16f,
                        ShouldSnapToFloor = true,
                        FloorSnapExtents = 64f,
                    });

                var key = param.Value.GetInstructionKey();
                if (key == "ENEMY")
                {
                }

                return chain;
        }

        return chains.Create("Blank Chain").Wait(5000);
    }
}
