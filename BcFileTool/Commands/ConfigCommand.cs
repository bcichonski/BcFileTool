﻿using Autofac;
using BcFileTool.DI;
using BcFileTool.Library.Enums;
using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Model;
using BcFileTool.Library.Services;
using BcFileTool.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BcFileTool.Commands
{
    public class ConfigCommand : IBcCommand
    {
        ISerializationService _serializationService;

        public ConfigOptions Options { get; set; }

        public ConfigCommand(ISerializationService serializationService)
        {
            _serializationService = serializationService;
        }

        public void Execute()
        {
            if (Options.Create)
            {
                var config = new Configuration();
                config.InputRootPath = Environment.CurrentDirectory;
                config.OutputRootPath = Path.Combine(Environment.CurrentDirectory, "output");

                config.Rules.Add(new Rule() {
                    Extension = "*.png",
                    OutputSubPath = "images",
                    Action = FileAction.Copy
                });

                config.Rules.Add(new Rule()
                {
                    RegEx = "[.]jp(g|eg)",
                    OutputSubPath = "pictures",
                    Action = FileAction.Move
                });

                config.Rules.Add(new Rule()
                {
                    Extension = "*.exe",
                    OutputSubPath = "dummy",
                    Action = FileAction.Info
                });

                _serializationService.Serialize("config.yml", config);
            }
        }
    }
}
