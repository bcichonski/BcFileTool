using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Options
{
    class OptionsProvider : IOptionsProvider
    {
        object _value;
        Type _valueType;

        public T Get<T>()
        {
            if(_value is T)
            {
                return (T)_value;
            }
            throw new ArgumentNullException();
        }

        public void Set<T>(T options)
        {
            _value = options;
            _valueType = typeof(T);
        }
    }
}
