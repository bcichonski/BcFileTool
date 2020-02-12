using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace BcFileTool.Library.Model
{
    public class Configuration
    {
        [YamlMember(Alias = "input", ApplyNamingConventions = false)]
        public List<string> InputRootPaths { get; set; }

        [YamlMember(Alias = "output", ApplyNamingConventions = false)]
        public string OutputRootPath { get; set; }

        public List<Rule> Rules { get; set; }

        public Configuration()
        {
            Rules = new List<Rule>();
        }
    }
}
