namespace SpeakWithWukLamat.Data.Quests;

public readonly record struct QuestId
{
    public ushort JournalId { get; }

    public uint RowId { get; }

    public QuestId(ushort journalId)
    {
        JournalId = journalId;
        RowId = (uint)(journalId | 0x10000);
    }

    public QuestId(uint id)
    {
        if (id < 0x10000)
        {
            JournalId = (ushort)id;
            RowId = (uint)(JournalId | 0x10000);
        }
        else
        {
            RowId = id;
            JournalId = (ushort)(id & 0xFFFF);
        }
    }

    public bool IsJournalId
    {
        get => RowId == (uint)(JournalId | 0x10000);
    }

    public override string ToString()
    {
        return $"QuestId(JournalId={JournalId}, RowId={RowId})";
    }
}
