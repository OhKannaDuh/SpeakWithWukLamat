namespace SpeakWithWukLamat.Data.Quests;

public readonly struct QuestVar(byte value)
{
    private readonly byte Value = value;

    public int LowNibble
    {
        get => Value & 0x0F;
    }

    public int HighNibble
    {
        get => Value >> 4 & 0x0F;
    }

    public bool GetBitFlag(int index)
    {
        if ((uint)(index - 1) > 7)
        {
            return false;
        }

        return (Value >> index - 1 & 1) != 0;
    }

    public override string ToString()
    {
        return $"0x{Value:X2} ({Value})";
    }
}
