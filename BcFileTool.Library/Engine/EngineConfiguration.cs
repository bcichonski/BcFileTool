using BcFileTool.Library.Interfaces;
using BcFileTool.Library.Model;

namespace BcFileTool.Library.Engine
{
    public class EngineConfiguration
    {
        public Configuration Configuration { get; set; }
        public IProgressInfo ProgressInfo { get; set; }
        public IScanOptions ScanOptions {get;set;}
    }
}
