using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Options
{
    [Verb(name: "scan", HelpText = "Scan files and apply rules")]
    public class ScanOptions : GeneralOptions
    {
        [Option('c', "config", HelpText = "Configuration file")]
        public string ConfigurationFile { get; set; } = "config.yml";

        [Option('s', "skip", HelpText = "Skip files that exists in output directory")]
        public bool SkipExistingFiles { get; set; }

        [Option('p', "preserve", HelpText = "Preserves subdirectories from input source")]
        public bool PreserveSubdirectories { get; set; }
    }
}
