using BcFileTool.Library.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace BcFileTool.Library.Model
{
    public class Rule
    {
        [YamlMember(Alias = "ext", ApplyNamingConventions = false)]
        public string Extension { get; set; }

        [YamlMember(Alias = "regex", ApplyNamingConventions = false)]
        public string RegEx { get; set; }

        public FileAction Action { get; set; }

        [YamlMember(Alias = "out", ApplyNamingConventions = false)]
        public string OutputSubPath { get; set; }
    }
}
