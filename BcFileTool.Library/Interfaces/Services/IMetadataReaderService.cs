using System;

namespace BcFileTool.Library.Interfaces.Services
{
    public interface IMetadataReaderService
    {
        DateTime ReadCreationTags(string path);
        string GetFirstDigits(string tagString, int count);
        DateTime ParseDate(string str);
        bool IsDateValid(DateTime date);
    }
}