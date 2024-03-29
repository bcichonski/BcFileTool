﻿using BcFileTool.Library.Interfaces;
using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Model;
using BcFileTool.Library.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace BcFileTool.Library.Engine
{
    public class Engine
    {
        Configuration _configuration;
        IMatchingService _matchingService;
        IMetadataReaderService _exifTagReaderService;
        IProgressInfo _progressInfo;
        bool _verbose;
        bool _skip;
        bool _preserve;
        bool _datedir;
        bool _verify;

        public Engine(EngineConfiguration engineConfiguration)
        {
            _configuration = engineConfiguration.Configuration;
            _progressInfo = engineConfiguration.ProgressInfo;
            _matchingService = new RuleMatchingService();
            _exifTagReaderService = new MetadataReaderService();
            _matchingService.Configure(_configuration);
            _verbose = engineConfiguration.ScanOptions.Verbose;
            _skip = engineConfiguration.ScanOptions.SkipExistingFiles;
            _preserve = engineConfiguration.ScanOptions.PreserveSubdirectories;
            _datedir = engineConfiguration.ScanOptions.DateDirectories;
            _verify = engineConfiguration.ScanOptions.VerifyChecksums;
        }

        public IEnumerable<FileEntry> GetAllFiles()
        {
            return
                _configuration.InputRootPaths
                .SelectMany(dirpath => EnumeratePath(dirpath));
        }

        HashSet<string> _verbosedEnumeratedPaths = new HashSet<string>();

        private IEnumerable<FileEntry> EnumeratePath(string dirpath)
        {
            if(_verbose && !_verbosedEnumeratedPaths.Contains(dirpath))
            {
                _progressInfo.Log($"Enumerating files in path {dirpath}...");
                _verbosedEnumeratedPaths.Add(dirpath);
            }
            Queue<string> directoryQueue = new Queue<string>();
            directoryQueue.Enqueue(dirpath);
            IEnumerable<FileEntry> result = Enumerable.Empty<FileEntry>();

            while (directoryQueue.Count > 0)
            {
                var currentDir = directoryQueue.Dequeue();
                bool errorShown = false;
                try
                {
                    var files = Directory.GetFiles(currentDir, "*", SearchOption.TopDirectoryOnly)
                        .Select(filepath => new FileEntry(filepath, _preserve ? dirpath : currentDir))
                        .ToList();//force errors here
                    result = result.Concat(files);
                }
                catch (Exception ex)
                {
                    _progressInfo.Log($"Error enumerating files in {currentDir}: {ex.Message}");
                    errorShown = true;
                }
                try
                {
                    var directories = Directory.EnumerateDirectories(currentDir, "*", SearchOption.TopDirectoryOnly);
                    foreach(var dir in directories)
                    {
                        directoryQueue.Enqueue(dir);
                    }
                }
                catch (Exception ex)
                {
                    if(!errorShown)
                    {
                        _progressInfo.Log($"Error enumerating subdirectories in {currentDir}: {ex.Message}");
                    }                    
                }
            }
            return result;
        }

        long _total = 0;
        ConcurrentBag<int> _progress;
        ConcurrentBag<int> _error;

        public void ProcessFiles(IEnumerable<FileEntry> files)
        {
            Verbose(files, "discovered");

            var filesToProcess = files
                .AsParallel()
                .GroupBy(entry => entry)
                .Select(group => Match(group));

            if(_skip)
            {
                if (!Directory.Exists(_configuration.OutputRootPath))
                {
                    Directory.CreateDirectory(_configuration.OutputRootPath);
                }
                var filesInTargetDir = EnumeratePath(_configuration.OutputRootPath).ToHashSet();
                var excludedFiles = filesToProcess
                    .AsParallel()
                    .Where(fg => filesInTargetDir.Contains(fg.Key))
                    .Select(fg => fg.Key)
                    .ToList();
                filesToProcess = filesToProcess.Where(fg => !filesInTargetDir.Contains(fg.Key));
                Verbose(excludedFiles, "skipped due to presence in target directory");
            }

            var unmatchedFiles = filesToProcess
                .AsParallel()
                .Where(group => group.Key.State == Enums.FileState.Unmatched)
                .SelectMany(group => group)
                .ToList();

            var matchedFiles = filesToProcess
                .AsParallel()
                .Where(group => group.Key.State == Enums.FileState.Matched)
                .SelectMany(group => SelectFiles(group))
                .ToList();

            Verbose(unmatchedFiles, "not matched");
            Verbose(matchedFiles, "matched");

            _total = matchedFiles.LongCount();
            _progress = new ConcurrentBag<int>();
            _error = new ConcurrentBag<int>();
            Timer progressTimer = null;

            if (_verbose)
            {
                progressTimer = new Timer(1000.0);
                progressTimer.Elapsed += ProgressTimer_Elapsed;
                progressTimer.AutoReset = true;
                progressTimer.Start();
            }

            var processedFiles = matchedFiles
                .AsParallel()
                .Select(file =>
                {
                    var ret = file.Process(
                        _exifTagReaderService,
                        _configuration.OutputRootPath,
                        _skip,
                        _datedir,
                        _verify);
                    _progress.Add(1);
                    if(ret.Exception != null)
                    {
                        _error.Add(1);
                    }
                    return ret;
                })
                .ToList();//force query to evaluate

            progressTimer?.Stop();
            
            _progressInfo.Percentage = 100;
            _progressInfo.Errors = _error.Count;
            _progressInfo.Log("");

            VerboseProcess(processedFiles);
        }

        private void ProgressTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _progressInfo.Percentage = (int)(1.0 * _progress.Count / _total * 100.0);
            _progressInfo.Errors = _error.Count;
        }

        private void VerboseProcess(List<FileEntry> files)
        {
            var summary = files.GroupBy(file => file.State.ToString());
            WriteSummary($"files processed", summary);

            if (_verbose)
            {
                var errorSummary = files.Where(file => file.Exception != null)
                    .GroupBy(file => file.Exception.Message);

                WriteSummary($"files have errors", errorSummary, _verbose);
            }
        }

        private void Verbose(IEnumerable<FileEntry> files, string action)
        {
            if(_verbose)
            {
                var summary = files.GroupBy(file => Path.GetExtension(file.FileName));
                WriteSummary($"file types {action}", summary);
            }
        }

        const int ShortErrorDescriptorLength = 30;

        private void WriteSummary(string summary, IEnumerable<IGrouping<string, FileEntry>> data, bool showDetails = false)
        {
            _progressInfo.Log($"{data.Count()} {summary}");
            foreach (var group in data.OrderByDescending(d => d.Count()))
            {
                var key = group.Key;
                if (key.Length > ShortErrorDescriptorLength)
                {
                    key = "…" + key.Substring(key.Length - Math.Min(ShortErrorDescriptorLength - 1, key.Length), 
                        Math.Min(ShortErrorDescriptorLength - 1, key.Length));
                };

                _progressInfo.Log($"  - {key,ShortErrorDescriptorLength} {group.Count()}");

                if(showDetails)
                {
                    foreach(var item in group)
                    {
                        _progressInfo.Log($"    - {item.Exception.Message}");
                    }
                }
            }
            _progressInfo.Log("");
        }

        private IEnumerable<FileEntry> SelectFiles(IGrouping<FileEntry, FileEntry> group)
        {
            foreach (var file in group)
            {
                try
                {
                    file.GetFileDetails();
                }
                catch (Exception e)
                {
                    file.Exception = e;
                }
            }

            if (group.Count() == 1)
                return group;

            var rule = group.Key.MatchedRule;
            if (rule.RemoveDuplicates)
            {             
                return group.OrderByDescending(file => file.CreationTimestamp).Take(1);
            }
            else
            {
                return group;
            }
        }

        private IGrouping<FileEntry, FileEntry> Match(IGrouping<FileEntry, FileEntry> group)
        {
            //since files has same name, we can test it once
            var first = group.Key;

            first.MatchRules(_matchingService);

            foreach (var item in group)
            {
                item.MatchedRule = first.MatchedRule;
                item.State = first.State;
            }
            return group;
        }
    }
}
