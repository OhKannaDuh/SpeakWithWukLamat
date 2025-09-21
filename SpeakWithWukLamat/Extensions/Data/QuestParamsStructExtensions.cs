using System.Text.RegularExpressions;

namespace SpeakWithWukLamat.Extensions.Data;

public static class QuestParamsStructExtensions
{
    public static string GetInstructionKey(this QuestData.QuestParamsStruct param)
    {
        return Regex.Replace(param.ScriptInstruction.ExtractText(), @"\d+$", "");
    }
}
