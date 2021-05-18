namespace BcFileTool.Library.Interfaces
{
    public interface IProgressInfo
    {
        int Percentage { get; set; }
        int Errors { get; set; }
        void Log(string message);
    }
}
