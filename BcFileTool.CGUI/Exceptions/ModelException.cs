using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Exceptions
{
    public class ModelException : ApplicationException
    {
        public ModelException()
        {
        }

        public ModelException(string message) : base(message)
        {
        }

        public ModelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
