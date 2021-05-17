using System;

namespace BcFileTool.CGUI.Models
{
    [Serializable]
    public class MainModel
    {
        public SourcesModel Sources { get; set; }
        public ExtensionsModel Extensions { get; set; }
        public OptionsModel Options { get; set; }

        public MainModel()
        {
            Sources = new SourcesModel();
            Extensions = new ExtensionsModel();
            Options = new OptionsModel();
        }
    }
}
