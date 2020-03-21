using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BcFileTool.Library.Streams
{
    public class MD5Stream : HashStream
    {
        public MD5Stream(Stream sourceStream) : base(sourceStream, MD5.Create())
        {
        }
    }
}
