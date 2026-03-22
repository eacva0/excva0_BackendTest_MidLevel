using System;
using System.Collections.Generic;

namespace ACPD.Models;

public partial class MyOffice_ExcuteionLog
{
    public long DeLog_AutoID { get; set; }

    public string DeLog_StoredPrograms { get; set; } = null!;

    public Guid DeLog_GroupID { get; set; }

    public bool DeLog_isCustomDebug { get; set; }

    public string DeLog_ExecutionProgram { get; set; } = null!;

    public string? DeLog_ExecutionInfo { get; set; }

    public bool? DeLog_verifyNeeded { get; set; }

    public DateTime DeLog_ExDateTime { get; set; }
}
