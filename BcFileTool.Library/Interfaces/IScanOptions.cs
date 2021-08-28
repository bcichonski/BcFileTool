namespace BcFileTool.Library.Interfaces
{
    public interface IScanOptions
    {
        string ConfigurationFile { get; }

        bool SkipExistingFiles { get; }

        bool PreserveSubdirectories { get; }

        bool DateDirectories { get; }

        bool VerifyChecksums { get; }

        bool Verbose { get; }
    }
}
