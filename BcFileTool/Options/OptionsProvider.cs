using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BcFileTool.Options
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Late bound through DI")]
    internal class OptionsProvider : IOptionsProvider
    {
        object _value;
        Type _valueType;

        public T GetOptions<T>()
        {
            if(_value is T)
            {
                return (T)_value;
            }
            throw new Exception("Value has different type than type that was requested.");
        }

        public void SetOptions<T>(T options)
        {
            _value = options;
            _valueType = typeof(T);
        }
    }
}
