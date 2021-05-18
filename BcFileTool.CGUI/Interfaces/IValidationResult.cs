using System.Collections.Generic;

namespace BcFileTool.CGUI.Interfaces
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        IEnumerable<string> Issues { get; }

        void Merge(IValidationResult result);
    }
}
