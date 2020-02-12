using BcFileTool.Library.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace BcFileTool.Library.Model
{
    public class Rule
    {
        const char Separator = '|';

        [YamlMember(Alias = "exts", ApplyNamingConventions = false)]
        public string Extensions
        {
            get
            {
                return string.Join(Separator, _extensions);
            }
            set
            {
                _extensions = value.Split(new char[] { Separator }, 
                    StringSplitOptions.RemoveEmptyEntries);
            }
        }

        [YamlMember(Alias = "deduplicate", ApplyNamingConventions = false)]
        public bool RemoveDuplicates { get; set; }

        public FileAction Action { get; set; }

        [YamlMember(Alias = "out", ApplyNamingConventions = false)]
        public string OutputSubPath { get; set; }

        [YamlIgnore]
        internal string[] _extensions;

        internal bool Test(string file)
        {
            var fileExt = Path.GetExtension(file);
            return _extensions.Any(ext => ext == fileExt);
        }
    }
}
