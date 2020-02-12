using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Library.Enums
{
    public enum FileState
    {
        Discovered = 0,
        Unmatched,
        Matched,
        Processed,
        ProcessedWithError
    }
}
