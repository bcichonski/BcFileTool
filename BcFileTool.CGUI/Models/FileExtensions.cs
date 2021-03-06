﻿using BcFileTool.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace BcFileTool.CGUI.Models
{
    public class FileExtensions : IEquatable<FileExtensions>
    {
        [YamlIgnore]
        public bool IsNew { get; set; }
        public List<string> ExtensionList { get; set; }
        public string OutputSubdir { get; set; }

        [YamlIgnore]
        public int Id { get; set; }

        public FileExtensions()
        {
            ExtensionList = new List<string>();
            OutputSubdir = ".";
            IsNew = true;
        }

        public FileExtensions(FileExtensions fileExtension)
        {
            ExtensionList = fileExtension.ExtensionList;
            OutputSubdir = fileExtension.OutputSubdir;
            IsNew = fileExtension.IsNew;
            Id = fileExtension.GetHashCode();
        }

        public void Add(string extension)
        {
            if (!ExtensionList.Contains(extension))
            {
                ExtensionList.Add(extension);
            }
        }

        internal string ToExtensionsConfig()
        {
            return string.Join(Rule.Separator, ExtensionList);
        }

        public void Remove(string extension)
        {
            ExtensionList.Remove(extension);
        }

        static readonly char[] _separators = new char[] { ',', ';', '|' };

        public void Reconcile(string extensionString)
        {
            var tokens = extensionString.Split(_separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => !x.StartsWith('.') ? "."+x : x)
                .Select(x => x.ToLowerInvariant())
                .Distinct();

            var toremove = ExtensionList.Except(tokens);
            var toadd = tokens.Except(ExtensionList);

            foreach(var token in toremove)
            {
                Remove(token);
            }

            foreach (var token in toadd)
            {
                Add(token);
            }
        }

        public override bool Equals(object obj) 
            => Equals(obj as FileExtensions);

        public bool Equals(FileExtensions other) 
            => other != null &&
                   EqualityComparer<List<string>>.Default.Equals(ExtensionList, other.ExtensionList);

        public override int GetHashCode() 
            => HashCode.Combine(ExtensionList);

        public string GetExtensions()
            => string.Join("|", ExtensionList);

        public override string ToString() 
            => string.Join("|", ExtensionList)+" -> "+OutputSubdir;
        
    }
}
