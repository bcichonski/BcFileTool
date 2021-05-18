using BcFileTool.Library.Model;
using System;
using System.Linq;

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

        internal Configuration ToConfiguration()
        {
            var config = new Configuration();

            config.InputRootPaths = Sources.Sources
                .Where(x => x.Selected)
                .Select(x => x.Path)
                .ToList();

            config.OutputRootPath = "";

            config.Rules = Extensions.Extensions
                .Select(x => new Rule() { 
                    Action = Options.Action, 
                    Extensions = x.ToExtensionsConfig(), 
                    OutputSubPath = x.OutputSubdir, 
                    RemoveDuplicates = true })
                .ToList();

            return config;
        }
    }
}
