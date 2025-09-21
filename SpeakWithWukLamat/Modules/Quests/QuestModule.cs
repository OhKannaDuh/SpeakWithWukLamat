using System;
using System.Linq;
using System.Numerics;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine;
using Lumina.Extensions;
using Ocelot.Graphics;
using Ocelot.Lifecycle;
using Ocelot.Services.OverlayRenderer;
using Ocelot.Windows;
using SpeakWithWukLamat.Modules.Automator;
using SpeakWithWukLamat.Services.QuestManager;

namespace SpeakWithWukLamat.Modules.Quests;

public class QuestModule(
    IOverlayRenderer overlay,
    IAutomatorContext context,
    IQuestManager quests,
    IMainWindow? main = null
) : IOnStart, IOnRender
{
    public void OnStart()
    {
        main?.Toggle();
    }

    public void Render()
    {
        var id = context.GetQuest();
        if (!context.IsActive || id == null)
        {
            return;
        }

        var quest = quests.GetActiveQuests().FirstOrDefault(q => q.Id == id);
        var step = quest?.GetCurrentSubStep();
        if (step == null)
        {
            return;
        }
        
        var type = (InstanceType)step.Level.Type;
        switch (type)
        {
            case InstanceType.QuestMarker:
                var param = step.Params.FirstOrNull();
                if (param == null)
                {
                    throw new InvalidOperationException("Could not find param for QuestMarker Substep");   
                }

                var listener = step.Listeners.FirstOrNull();
                if (listener == null)
                {
                    throw new InvalidOperationException("Could not find listener for QuestMarker Substep");   
                }
                    
                var pos = new Vector3(step.Level.X, step.Level.Y, step.Level.Z);
                pos = pos with { Y = Player.Position.Y };

                var distance = Player.DistanceTo(pos);

                var radius = step.Level.Radius * 0.16;
                
                overlay.StrokeCircle(pos, (float)radius, Color.Red);
                break;
        }
    }
}
