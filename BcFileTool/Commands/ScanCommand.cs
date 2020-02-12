using BcFileTool.Library.Engine;
using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Model;
using BcFileTool.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcFileTool.Commands
{
    public class ScanCommand : IBcCommand
    {
        public ScanOptions Options { get; set; }

        ISerializationService _serializationService;
        public ScanCommand(ISerializationService serializationService)
        {
            _serializationService = serializationService;
        }
        public void Execute()
        {
            var configuration = _serializationService.Deserialize<Configuration>(Options.ConfigurationFile);
            var engine = new Engine(configuration, Options.Verbose, Options.SkipExistingFiles);

            var files = engine.GetAllFiles().AsParallel();
            engine.ProcessFiles(files);
        }
    }
}
