using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Options
{
    public class GeneralOptions
    {
        [Option('t', "time", HelpText = "Measures program execution time")]
        public bool MeasureTime { get; set; }

        [Option('v', "verbose", HelpText = "Display what is going on")]
        public bool Verbose { get; set; }
    }
}
