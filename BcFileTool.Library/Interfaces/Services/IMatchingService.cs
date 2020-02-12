using BcFileTool.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Library.Interfaces.Services
{
    interface IMatchingService
    {
        void Configure(Configuration configuration);

        bool TryMatch(string file, out Rule matchedRule);
    }
}
