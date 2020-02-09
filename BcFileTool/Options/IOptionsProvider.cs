using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Options
{
    public interface IOptionsProvider
    {
        void Set<T>(T options);
        T Get<T>();
    }
}
