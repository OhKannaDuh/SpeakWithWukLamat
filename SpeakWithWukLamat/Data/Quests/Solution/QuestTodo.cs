namespace SpeakWithWukLamat.Data.Quests.Solution;

/// <param name="Argument">Misc argument for resolving items and recipes that are related to the step.</param>
public readonly record struct QuestTodo(uint Progress, uint Max, uint Argument)
{
    public float Percentage
    {
        get => Progress / (float)Max;
    }

    public string Text
    {
        get => $"{Progress}/{Max}";
    }
}
