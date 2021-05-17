namespace BcFileTool.Library.Interfaces.Services
{
    public interface IFileService
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string content);
        bool FileExists(string path);
    }
}
