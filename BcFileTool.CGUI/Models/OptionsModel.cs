using BcFileTool.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Models
{
    public class OptionsModel
    {
        public FileAction Action { get; set; }
        public bool Verbose { get; set; }
        public bool Skip { get; set; }
        public bool PreserveSubdirectories { get; set; }
        public bool DateDirectories { get; set; }
        public bool VerifyChecksum { get; set; }
    }
}
