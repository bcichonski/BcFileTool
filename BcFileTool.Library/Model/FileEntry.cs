using BcFileTool.Library.Constants;
using BcFileTool.Library.Enums;
using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Streams;
using System;
using System.IO;

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
        public string Checksum { get; set; }
        public Rule MatchedRule { get; set; }

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

        internal FileEntry Process(IMetadataReaderService tagReader, string baseOutPath, bool skip, bool datedir, bool verify)
        {
            try
            {
                var fullInPath = Path.Combine(InputBasePath, InputPath);
                var date = tagReader.ReadCreationTags(fullInPath);
                switch (MatchedRule.Action)
                {
                    case FileAction.Info:
                        Console.WriteLine($"{fullInPath}");
                        break;
                    case FileAction.Copy:
                    case FileAction.Move:
                        HandleAction(tagReader, baseOutPath, fullInPath, skip, datedir, verify);
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

        private void HandleAction(IMetadataReaderService tagReader, string baseOutPath, string fullInPath, bool skip, bool datedir, bool verify)
        {
            var fullOutPath = baseOutPath;

            if (datedir)
            {
                if (tagReader.IsDateValid(CreationTimestamp))
                {//kind of fallback
                    GuessCreationTimestamp(tagReader, fullInPath);
                }

                var dirsubpath = "unknown";
                if (tagReader.IsDateValid(CreationTimestamp))
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

            if (!verify)
            {
                if (MatchedRule.Action == FileAction.Copy)
                {
                    File.Copy(fullInPath, fullOutPath);
                }
                else
                {
                    File.Move(fullInPath, fullOutPath);
                }
            }
            else
            {
                MoveAndVerifyChecksum(fullInPath, fullOutPath, MatchedRule.Action);
            }

            SetOutputCreationDate(tagReader, fullOutPath);
        }

        

        private void SetOutputCreationDate(IMetadataReaderService tagReader, string fullOutPath)
        {
            if (tagReader.IsDateValid(CreationTimestamp))
            {
                File.SetCreationTime(fullOutPath, CreationTimestamp);
            }
        }

        private void MoveAndVerifyChecksum(string fullInPath, string fullOutPath, FileAction action)
        {
            //copy file to destination and calc checksum

            using (var inputFileStream = new FileStream(fullInPath, FileMode.Open, FileAccess.Read))
            {
                using (var md5Stream = new MD5Stream(inputFileStream))
                {
                    using (var outputFileStream = new FileStream(fullOutPath, FileMode.Create, FileAccess.Write))
                    {
                        inputFileStream.CopyTo(outputFileStream);
                    }
                    Checksum = md5Stream.Hash;
                }
            }

            string newchecksum = null;
            //calc desc file checksum again
            using (var inputFileStream = new FileStream(fullInPath, FileMode.Open, FileAccess.Read))
            {
                using (var md5Stream = new MD5Stream(inputFileStream))
                {
                    using (var nullStream = new NullStream())
                    {
                        inputFileStream.CopyTo(nullStream);
                    }
                    newchecksum = md5Stream.Hash;
                }
            }

            if (Checksum == newchecksum)
            {
                if (action == FileAction.Move)
                {
                    File.Delete(fullInPath);
                }
            }
            else
            {
                throw new Exception($"File has invalid checksum after copying or moving to output location.");
            }
        }

        private void GuessCreationTimestamp(IMetadataReaderService tagReader, string fullinPath)
        {
            DateTime date;
            if (this.FileName.Length > Const.DateFormat.Length)
            {
                var datestr = tagReader.GetFirstDigits(this.FileName, Const.DateFormat.Length);
                if (datestr.Length == Const.DateFormat.Length)
                {
                    date = tagReader.ParseDate(datestr);
                    if (tagReader.IsDateValid(date))
                    {
                        CreationTimestamp = date;
                    }
                }
                else
                {
                    date = tagReader.ReadCreationTags(fullinPath);
                    if (tagReader.IsDateValid(date))
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
