using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Options
{
    [Verb(name: "config", HelpText = "Config manipulation")]
    public class ConfigOptions : GeneralOptions
    {
        [Option('c', "create", HelpText = "Creates example configuration file with set of rules.")]
        public bool Create { get; set; }
    }
}
