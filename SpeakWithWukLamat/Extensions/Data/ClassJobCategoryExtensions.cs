using System.Collections.Generic;
using System.Linq;
using Lumina.Excel.Sheets;
using Ocelot.Services.Data;

namespace SpeakWithWukLamat.Extensions.Data;

public static class ClassJobCategoryExtensions
{
    public static bool IsForJob(this ClassJobCategory category, ClassJob job)
    {
        return job.RowId switch
        {
            1 => category.GLA,
            2 => category.PGL,
            3 => category.MRD,
            4 => category.LNC,
            5 => category.ARC,
            6 => category.CNJ,
            7 => category.THM,
            8 => category.CRP,
            9 => category.BSM,
            10 => category.ARM,
            11 => category.GSM,
            12 => category.LTW,
            13 => category.WVR,
            14 => category.ALC,
            15 => category.CUL,
            16 => category.MIN,
            17 => category.BTN,
            18 => category.FSH,
            19 => category.PLD,
            20 => category.MNK,
            21 => category.WAR,
            22 => category.DRG,
            23 => category.BRD,
            24 => category.WHM,
            25 => category.BLM,
            26 => category.ACN,
            27 => category.SMN,
            28 => category.SCH,
            29 => category.ROG,
            30 => category.NIN,
            31 => category.MCH,
            32 => category.DRK,
            33 => category.AST,
            34 => category.SAM,
            35 => category.RDM,
            36 => category.BLU,
            37 => category.GNB,
            38 => category.DNC,
            39 => category.RPR,
            40 => category.SGE,
            41 => category.VPR,
            42 => category.PCT,
            _ => false,
        };
    }

    public static IEnumerable<ClassJob> GetJobs(this ClassJobCategory category, IDataRepository<ClassJob> jobs)
    {
        return jobs.GetAll().Where(job => category.IsForJob(job));
    }
}
