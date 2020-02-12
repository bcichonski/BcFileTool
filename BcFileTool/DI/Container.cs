using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using BcFileTool.Commands;
using BcFileTool.Library.Services;
using BcFileTool.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.DI
{
    public sealed class DIContainer
    {
        public static IContainer Instance => Configure();

        public static IContainer Configure()
        {
            // add the framework services
            var services = new ServiceCollection();

            // add actual Container
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            // register additional services
            containerBuilder.RegisterType<YamlService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            containerBuilder.RegisterType<OptionsProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();
            containerBuilder.RegisterType<ConfigCommand>()
                .SingleInstance()
                .Named<IBcCommand>("Config");
            containerBuilder.RegisterType<ScanCommand>()
                .SingleInstance()
                .Named<IBcCommand>("Scan");

            return containerBuilder.Build();
        }
    }
}
