using BcFileTool.Library.Enums;

namespace BcFileTool.CGUI.Models
{
    public class OptionsModel
    {
        public string OutputDirectory { get; set; }
        public FileAction Action { get; set; }
        public bool Verbose { get; set; }
        public bool Skip { get; set; }
        public bool PreserveSubdirectories { get; set; }
        public bool DateDirectories { get; set; }
        public bool VerifyChecksum { get; set; }
    }
}
