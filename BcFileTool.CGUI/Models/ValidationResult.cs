using BcFileTool.CGUI.Interfaces;
using System.Collections.Generic;

namespace BcFileTool.CGUI.Models
{
    class ValidationResult : IValidationResult
    {
        public bool IsValid => _issues.Count == 0;

        List<string> _issues = new List<string>();
        public IEnumerable<string> Issues => _issues;

        public void AddIssue(string issue) => _issues.Add(issue);

        public void Merge(IValidationResult result)
        {
            _issues.AddRange(result.Issues);
        }
    }
}
