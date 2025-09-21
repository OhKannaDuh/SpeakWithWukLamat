using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ocelot.Services.Data;

namespace SpeakWithWukLamat.Data.Quests;

public class QuestPatchRepository(IEnumerable<IQuestPatch> patches) : IDataRepository<QuestId, IQuestPatch>
{
    private readonly Dictionary<QuestId, IQuestPatch> data = patches.ToDictionary(p => p.Id, p => p);
    
    public IEnumerable<QuestId> GetKeys()
    {
        return data.Keys;
    }

    public void Add(QuestId key, IQuestPatch model)
    {
        data.Add(key, model);
    }

    public bool TryAdd(QuestId key, IQuestPatch model)
    {
        return data.TryAdd(key, model);
    }

    public IQuestPatch Get(QuestId key)
    {
        return data[key];
    }

    public IEnumerable<IQuestPatch> GetAll()
    {
        return data.Values;
    }

    public IEnumerable<IQuestPatch> Where(Expression<Func<IQuestPatch, bool>> predicate)
    {
        return data.Values.Where(predicate.Compile());
    }

    public bool Remove(QuestId key)
    {
        return data.Remove(key);
    }

    public bool ContainsKey(QuestId key)
    {
        return data.ContainsKey(key);
    }
}
