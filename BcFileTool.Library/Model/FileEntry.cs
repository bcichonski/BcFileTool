using BcFileTool.Library.Enums;
using BcFileTool.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BcFileTool.Library.Model
{
    public class FileEntry : IEquatable<FileEntry>
    {
        public FileState State { get; set; }
        public string InputPath { get; set; }
        public string InputBasePath { get; set; }
        public string FileName { get; set; }
        public long? Length { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public DateTime ModificationTimestamp { get; set; }
        public ulong? CRC32 { get; set; }
        public Rule MatchedRule { get; set; }

        Exception _exception;
        public Exception Exception { 
            get
            {
                return _exception;
            }
            set
            {
                _exception = value;
                State = FileState.ProcessedWithError;
            }
        }

        public FileEntry(string path, string basePath)
        {
            InputBasePath = basePath;
            InputPath = Path.GetRelativePath(basePath, path);
            FileName = Path.GetFileName(path).ToLowerInvariant();
        }

        internal void GetFileDetails()
        {
            var fileInfo = new FileInfo(Path.Combine(InputBasePath, InputPath));
            Length = fileInfo.Length;
            CreationTimestamp = fileInfo.CreationTime;
            ModificationTimestamp = fileInfo.LastWriteTime;
        }

        void GetFileCRC32()
        {
            throw new NotImplementedException();
        }

        internal FileEntry Process(string baseOutPath, bool skip)
        {
            try
            {
                var fullInPath = Path.Combine(InputBasePath, InputPath);

                switch (MatchedRule.Action)
                {
                    case FileAction.Info:
                        Console.WriteLine($"{fullInPath}");
                        break;
                    case FileAction.Copy:
                    case FileAction.Move:
                        HandleAction(baseOutPath, fullInPath, skip);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } catch (Exception e)
            {
                Exception = e;
            }
            return this;
        }

        private void HandleAction(string baseOutPath, string fullInPath, bool skip)
        {
            var fullOutPath = baseOutPath;
            if (!string.IsNullOrWhiteSpace(MatchedRule.OutputSubPath))
            {
                fullOutPath = Path.Combine(fullOutPath, MatchedRule.OutputSubPath);
            }
            fullOutPath = Path.Combine(fullOutPath, InputPath);

            var outPath = Path.GetDirectoryName(fullOutPath);
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            if (skip)
            {
                if (File.Exists(fullOutPath))
                {
                    return;
                }
            }
            if (MatchedRule.Action == FileAction.Copy)
            {               
                File.Copy(fullInPath, fullOutPath);
            }
            else
            {
                File.Move(fullInPath, fullOutPath);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileEntry);
        }

        public bool Equals(FileEntry other)
        {
            return other != null &&
                   FileName == other.FileName;
        }

        public override int GetHashCode()
        {
            var hashCode = -219294424;
            hashCode = hashCode * -1521134295 + FileName.GetHashCode();
            return hashCode;
        }

        internal void MatchRules(IMatchingService matchingService)
        {
            if(matchingService.TryMatch(FileName, out Rule matchedRule))
            {
                State = FileState.Matched;
                MatchedRule = matchedRule;
            } else
            {
                State = FileState.Unmatched;
            }
        }
    }
}
