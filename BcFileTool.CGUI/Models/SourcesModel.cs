using BcFileTool.CGUI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Models
{
    public class SourcesModel
    {
        public List<Source> Sources { get; }

        public SourcesModel()
        {
            Sources = new List<Source>();
        }

        public void Add(Source source)
        {
            if(Sources.Any(x => x == source))
            {
                throw new ModelException($"Source {source.Name} already exists");
            }
            
            Sources.Add(source);
        }

        internal void SourceSelectionFlipped(int selectedItem)
        {
            Sources[selectedItem].FlipSelection();
        }

        internal void RemoveAt(int selectedItem)
        {
            if(Sources.Count > selectedItem)
            {
                Sources.RemoveAt(selectedItem);
            }
        }
    }
}
