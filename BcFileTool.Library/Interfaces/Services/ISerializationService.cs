using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Library.Interfaces.Services
{
    public interface ISerializationService
    {
        T Deserialize<T>(string path);
        void Serialize<T>(string path, T @object);
    }
}
