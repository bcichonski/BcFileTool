using BcFileTool.Implementations;
using BcFileTool.Library.Engine;
using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Model;
using BcFileTool.Options;

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

            var engineConfiguration = new EngineConfiguration()
            {
                Configuration = configuration,
                ProgressInfo = new ProgressInfo(),
                ScanOptions = Options
            };

            var engine = new Engine(engineConfiguration);

            var files = engine.GetAllFiles();
            engine.ProcessFiles(files);
        }
    }
}
