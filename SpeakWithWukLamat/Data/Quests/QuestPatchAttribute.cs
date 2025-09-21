using System;

namespace SpeakWithWukLamat.Data.Quests;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class QuestPatchAttribute(string author, string patch) : Attribute
{
    public string Author { get; } = author;
    
    public string GamePatch { get; } = patch;
}
