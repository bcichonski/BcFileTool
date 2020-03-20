using BcFileTool.Library.Constants;
using BcFileTool.Library.Enums;
using BcFileTool.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private const int AssumedMinimalYearValue = 1900;
        Exception _exception;
        public Exception Exception
        {
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

        internal FileEntry Process(IExifTagReaderService tagReader, string baseOutPath, bool skip, bool datedir)
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
                        HandleAction(tagReader, baseOutPath, fullInPath, skip, datedir);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Exception = e;
            }
            return this;
        }

        

        private void HandleAction(IExifTagReaderService tagReader, string baseOutPath, string fullInPath, bool skip, bool datedir)
        {
            var fullOutPath = baseOutPath;

            if (datedir)
            {
                if (CreationTimestamp.Year < AssumedMinimalYearValue)
                {//kind of fallback
                    GuessCreationTimestamp(tagReader, fullInPath);
                }

                var dirsubpath = "unknown";
                if (CreationTimestamp.Year > AssumedMinimalYearValue)
                {
                    dirsubpath = string.Format("{0:yyyy}\\{0:MM}", CreationTimestamp);
                }

                fullOutPath = Path.Combine(fullOutPath, dirsubpath);
            }

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

        private void GuessCreationTimestamp(IExifTagReaderService tagReader, string fullinPath)
        {
            DateTime date;
            if (this.FileName.Length > Const.DateFormat.Length)
            {
                var datestr = tagReader.GetFirstDigits(this.FileName, Const.DateFormat.Length);
                if(datestr.Length == Const.DateFormat.Length)
                {
                    date = tagReader.ParseDate(datestr);
                    if(date.Year > AssumedMinimalYearValue)
                    {
                        CreationTimestamp = date;
                    }
                }
                else
                {
                    date = tagReader.ReadCreationTags(fullinPath);
                    if(date.Year > AssumedMinimalYearValue)
                    {
                        CreationTimestamp = date;
                    }
                }
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
            if (matchingService.TryMatch(FileName, out Rule matchedRule))
            {
                State = FileState.Matched;
                MatchedRule = matchedRule;
            }
            else
            {
                State = FileState.Unmatched;
            }
        }
    }
}
