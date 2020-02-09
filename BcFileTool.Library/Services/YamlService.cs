using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BcFileTool.Library.Services
{
    public class YamlService : ISerializationService
    {
        static readonly IDeserializer _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

        static readonly ISerializer _serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

        public T Deserialize<T>(string path)
        {
            return _deserializer.Deserialize<T>(File.ReadAllText(path));
        }

        public void Serialize<T>(string path, T @object)
        {
            File.WriteAllText(path, _serializer.Serialize(@object));
        }
    }
}
