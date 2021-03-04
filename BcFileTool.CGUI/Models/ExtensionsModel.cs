using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Models
{
    public class ExtensionsModel
    {
        public List<FileExtensions> Extensions { get; }

        public ExtensionsModel()
        {
            Extensions = new List<FileExtensions>();
        }

        public void Add(FileExtensions fileExtensions)
        {
            if(!Extensions.Contains(fileExtensions))
            {
                Extensions.Add(fileExtensions);
            }
        }
        
    }
}
