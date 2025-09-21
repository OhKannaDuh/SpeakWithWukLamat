using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FFXIVClientStructs.FFXIV.Application.Network.WorkDefinitions;

namespace SpeakWithWukLamat.Data.Quests;

public readonly struct QuestVars(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5)
{
    private readonly Dictionary<string, QuestVar> Vars = new()
    {
        ["UI8AL"] = new QuestVar(b0),
        ["UI8BH"] = new QuestVar(b1),
        ["UI8BL"] = new QuestVar(b2),
        ["UI8CH"] = new QuestVar(b3),
        ["UI8CL"] = new QuestVar(b4),
        ["UI8DH"] = new QuestVar(b5),
    };

    public QuestVar UI8AL
    {
        get => Vars["UI8AL"];
    }

    public QuestVar UI8BH
    {
        get => Vars["UI8BH"];
    }

    public QuestVar UI8BL
    {
        get => Vars["UI8BL"];
    }

    public QuestVar UI8CH
    {
        get => Vars["UI8CH"];
    }

    public QuestVar UI8CL
    {
        get => Vars["UI8CL"];
    }

    public QuestVar UI8DH
    {
        get => Vars["UI8DH"];
    }

    public static unsafe QuestVars FromWork(in QuestWork work)
    {
        var p = (byte*)Unsafe.AsPointer(ref Unsafe.AsRef(in work));
        return new QuestVars(
            *(p + 0x0C + 0),
            *(p + 0x0C + 1),
            *(p + 0x0C + 2),
            *(p + 0x0C + 3),
            *(p + 0x0C + 4),
            *(p + 0x0C + 5)
        );
    }
}
