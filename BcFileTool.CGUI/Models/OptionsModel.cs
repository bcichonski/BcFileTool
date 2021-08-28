using BcFileTool.Library.Enums;
using BcFileTool.Library.Interfaces;
using YamlDotNet.Serialization;

namespace BcFileTool.CGUI.Models
{
    public class OptionsModel : IScanOptions
    {
        public string OutputDirectory { get; set; }
        public FileAction Action { get; set; }
        public bool Verbose { get; set; }
        [YamlMember(Alias = "skip")]
        public bool SkipExistingFiles { get; set; }
        public bool PreserveSubdirectories { get; set; }

        public bool DateDirectories { get; set; }

        [YamlMember(Alias = "verifyChecksum")]
        public bool VerifyChecksums { get; set; }

        [YamlIgnore]
        public string ConfigurationFile => throw new System.NotImplementedException();
    }
}
