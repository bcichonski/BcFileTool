using System;

namespace BcFileTool.Library.Interfaces.Services
{
    public interface IExifTagReaderService
    {
        DateTime ReadCreationTags(string path);
        string GetFirstDigits(string tagString, int count);
        DateTime ParseDate(string str);
    }
}