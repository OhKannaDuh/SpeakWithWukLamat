// using System.Collections.Generic;
// using System.Linq;
// using FFXIVClientStructs.FFXIV.Client.LayoutEngine;
// using Lumina.Excel.Sheets;
//
// namespace SpeakWithWukLamat.Data.Quests;
//
// public sealed class QuestFlow
// {
//     public Dictionary<int, QuestSequence> Sequences { get; } = [];
//     
//     // Orphaned Listeners
// }
//
// public sealed class QuestSequence
// {
//     public Dictionary<byte, QuestStep> Steps { get; } = [];
// }
//
// public sealed class QuestStep(byte step)
// {
//     public string Text { get; init; } = "";
//
//     public byte CountableNum { get; init; }
//
//     public byte ToDoQty { get; init; }
//
//     public IEnumerable<Level> Levels { get; init; } = [];
//
//     public QuestTodoArguments GetTodoArgs(Quest quest)
//     {
//         return quest.GetTodoArgs(step);
//     }
//
//     public bool IsTodoChecked(Quest quest)
//     {
//         return quest.IsTodoChecked(step);
//     }
// }
//
// public sealed class AssociatedListener
// {
//     
// }
//
// public static class QuestFlowBuilder
// {
//     public static QuestFlow Build(Quest quest)
//     {
//         var flow = new QuestFlow();
//
//         byte step = 0;
//         foreach (var todo in quest.Data.TodoParams)
//         {
//             if (todo.ToDoCompleteSeq == 0)
//             {
//                 continue;
//             }
//
//             flow.Sequences.TryAdd(todo.ToDoCompleteSeq, new QuestSequence());
//
//             var levels = todo.ToDoLocation.Where(l => l.IsValid).Select(l => l.Value).ToList();
//             var text = string.Join(", ", levels.Select(l => ((InstanceType)l.Type).ToString()));
//
//             flow.Sequences[todo.ToDoCompleteSeq].Steps.Add(step, new QuestStep(step)
//             {
//                 Text = text,
//                 CountableNum = todo.CountableNum,
//                 ToDoQty = todo.ToDoQty,
//                 Levels = levels,
//             });
//
//             step++;
//         }
//
//         return flow;
//     }
// }


