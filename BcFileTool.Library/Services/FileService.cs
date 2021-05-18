using BcFileTool.Library.Interfaces.Services;
using System.IO;

namespace BcFileTool.Library.Services
{
    public class FileService : IFileService
    {
        public bool FileExists(string path) =>
            File.Exists(path);

        public string ReadAllText(string path) =>
            File.ReadAllText(path);

        public void WriteAllText(string path, string content) =>
            File.WriteAllText(path, content);
    }
}
