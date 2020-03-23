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
        IExifTagReaderService _exifTagReaderService;
        bool _verbose;
        bool _skip;
        bool _preserve;
        bool _datedir;
        bool _verify;

        public Engine(Configuration configuration, bool verbose, bool skip, bool preserve, bool datedir, bool verify)
        {
            _configuration = configuration;
            _matchingService = new RuleMatchingService();
            _exifTagReaderService = new ExifTagReaderService();
            _matchingService.Configure(configuration);
            _verbose = verbose;
            _skip = skip;
            _preserve = preserve;
            _datedir = datedir;
            _verify = verify;
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
                Console.WriteLine($"Enumerate files in path {dirpath}...");
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
                    Console.WriteLine($"Error enumerating files in {currentDir}: {ex.Message}");
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
                        Console.WriteLine($"Error enumerating subdirectories in {currentDir}: {ex.Message}");
                    }                    
                }
            }
           
            if (_verbose)
            {
                Console.WriteLine($"Done.");
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
            Console.Write($"Processed {_progress.Count} of {_total} 100% with {_error.Count} errors\r");
            Console.WriteLine();

            VerboseProcess(processedFiles);
        }

        private void ProgressTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int curr = _progress.Count;
            int err = _error.Count;
            Console.Write($"Processed {curr} of {_total} {(int)(1.0*curr/_total*100.0)}% with {err} errors\r");
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

        private static void WriteSummary(string summary, IEnumerable<IGrouping<string, FileEntry>> data, bool showDetails = false)
        {
            Console.WriteLine($"{data.Count()} {summary}");
            foreach (var group in data.OrderByDescending(d => d.Count()))
            {
                var key = group.Key;
                if (key.Length > ShortErrorDescriptorLength)
                {
                    key = "…" + key.Substring(key.Length - Math.Min(ShortErrorDescriptorLength - 1, key.Length), 
                        Math.Min(ShortErrorDescriptorLength - 1, key.Length));
                };
                
                Console.WriteLine($"  - {key,ShortErrorDescriptorLength} {group.Count()}");

                if(showDetails)
                {
                    foreach(var item in group)
                    {
                        Console.WriteLine($"    - {item.Exception.Message}");
                    }
                }
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
                        file.GetFileDetails();
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
