using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Models
{
    public class FileExtensions : IEquatable<FileExtensions>
    {
        public List<string> ExtensionList { get; set; }
        public string OutputSubdir { get; set; }

        public void Add(string extension)
        {
            if (!ExtensionList.Contains(extension))
            {
                ExtensionList.Add(extension);
            }
        }

        public void Remove(string extension)
        {
            ExtensionList.Remove(extension);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileExtensions);
        }

        public bool Equals(FileExtensions other)
        {
            return other != null &&
                   EqualityComparer<List<string>>.Default.Equals(ExtensionList, other.ExtensionList);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ExtensionList);
        }

        public override string ToString()
        {
            return string.Join(", ", ExtensionList);
        }
    }
}
