using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Options
{
    public interface IOptionsProvider
    {
        void SetOptions<T>(T options);
        T GetOptions<T>();
    }
}
