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
        bool _verbose;
        bool _skip;

        public Engine(Configuration configuration, bool verbose, bool skip)
        {
            _configuration = configuration;
            _matchingService = new RuleMatchingService();
            _matchingService.Configure(configuration);
            _verbose = verbose;
            _skip = skip;
        }

        public ParallelQuery<FileEntry> GetAllFiles()
        {
            return Directory.GetFiles(_configuration.InputRootPath, "*", SearchOption.AllDirectories)
                .AsParallel()
                .Select(path => new FileEntry(path, _configuration.InputRootPath));
        }

        long _total = 0;
        ConcurrentBag<int> _progress;

        public void ProcessFiles(ParallelQuery<FileEntry> files)
        {
            Verbose(files, "discovered");

            var filesToProcess = files.GroupBy(entry => entry)
                .Select(group => Match(group));

            var unmatchedFiles = filesToProcess
                .Where(group => group.Key.State == Enums.FileState.Unmatched)
                .SelectMany(group => group);

            Verbose(unmatchedFiles, "not matched");

            var matchedFiles = filesToProcess
                .Where(group => group.Key.State == Enums.FileState.Matched)
                .SelectMany(group => SelectFiles(group));

            Verbose(matchedFiles, "matched");

            _total = matchedFiles.LongCount();
            _progress = new ConcurrentBag<int>();
            Timer progressTimer = null;

            if (_verbose)
            {
                progressTimer = new Timer(1000.0);
                progressTimer.Elapsed += ProgressTimer_Elapsed;
                progressTimer.AutoReset = true;
                progressTimer.Start();
            }

            var processedFiles = matchedFiles
                .Select(file => {
                    var ret = file.Process(_configuration.InputRootPath, 
                        _configuration.OutputRootPath,
                        _skip);
                    _progress.Add(1);
                    return ret;
                    });

            progressTimer?.Stop();
            Console.WriteLine();

            VerboseProcess(processedFiles);
        }

        private void ProgressTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int curr = _progress.Count;
            Console.Write($"Processed {curr} of {_total} {(int)(1.0*curr/_total*100.0)}%\r");
        }

        private void VerboseProcess(ParallelQuery<FileEntry> files)
        {
            var summary = files.GroupBy(file => file.State.ToString());
            WriteSummary($"files were processed", summary);

            if (_verbose)
            {
                var errorSummary = files.Where(file => file.Exception != null)
                    .GroupBy(file => file.Exception.Message);

                WriteSummary($"files have errors", errorSummary);
            }
        }

        private ParallelQuery<FileEntry> Verbose(ParallelQuery<FileEntry> files, string action)
        {
            if(_verbose)
            {
                var summary = files.GroupBy(file => Path.GetExtension(file.FileName));
                WriteSummary($"files were {action}", summary);
            }
            return files;
        }

        private static void WriteSummary(string summary, ParallelQuery<IGrouping<string, FileEntry>> data)
        {
            Console.WriteLine($"{data.Count()} {summary}");
            foreach (var group in data)
            {
                var key = group.Key;
                if (key.Length > 20)
                {
                    key = "…" + key.Substring(key.Length - Math.Min(19, key.Length), Math.Min(19, key.Length));
                };
                
                Console.WriteLine($"  - {key,20} {group.Count()}");
            }
            Console.WriteLine();
        }

        private IEnumerable<FileEntry> SelectFiles(IGrouping<FileEntry, FileEntry> group)
        {
            if (group.Count() == 1)
                return group;

            var rule = group.Key.MatchedRule;
            if (rule.RemoveDuplicates)
            {
                foreach(var file in group)
                {
                    try
                    {
                        file.GetFileDetails(_configuration.InputRootPath);
                    }
                    catch (Exception e)
                    {
                        file.Exception = e;
                    }
                }
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
