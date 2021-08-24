using BcFileTool.CGUI.Interfaces;
using System;

namespace BcFileTool.CGUI.Models
{
    public class Source : IEquatable<Source>, IAmSelectable
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public bool Selected { get; set; }

        public Source(string path)
        {
            Path = path;
            Name = path;
        }

        public Source()
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Source);
        }

        public bool Equals(Source other)
        {
            return other != null &&
                   Path == other.Path;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path);
        }

        internal void FlipSelection()
        {
            Selected = !Selected;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
