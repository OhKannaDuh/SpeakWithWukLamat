using System.Collections.Generic;
using SpeakWithWukLamat.Data.Quests;

namespace SpeakWithWukLamat.Data.QuestLists;

public class QuestList
{
    public IEnumerable<QuestId> Quests { get; init; } = [];
}
