using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Dialogs.ExtensionsEdit;
using BcFileTool.CGUI.Dialogs.Progress;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using BcFileTool.CGUI.Views;
using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BcFileTool.CGUI.Bootstrap
{
    public class Bootstrapper
    {
        public const string SettingsFile = @".\bft.settings.yaml";

        IServiceProvider _serviceProvider;
        ISerializationService _serializationService;
        IFileService _fileService;

        public Bootstrapper SetUpDependencyInjection()
        {
            
            _serializationService = new YamlService();
            _fileService = new FileService();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            return this;
        }

        public MainController CreateMainController() => 
            _serviceProvider.GetRequiredService<MainController>();

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            ConfigureCommonServices(serviceCollection);

            ConfigureModels(serviceCollection);
            ConfigureViews(serviceCollection);
            ConfigureControllers(serviceCollection);
        }

        private void ConfigureControllers(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SourcesController>();
            serviceCollection.AddSingleton<ExtensionsController>();
            serviceCollection.AddSingleton<OptionsController>();
            serviceCollection.AddSingleton<MainController>();
        }

        private void ConfigureViews(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SourcesView>();
            serviceCollection.AddSingleton<MainView>();
            serviceCollection.AddSingleton<ExtensionsView>();
            serviceCollection.AddSingleton<OptionsView>();
        }

        private void ConfigureModels(ServiceCollection serviceCollection)
        {
            var mainModel = LoadModel();

            serviceCollection.AddSingleton<SourcesModel>(_ => mainModel.Sources);
            serviceCollection.AddSingleton<ExtensionsModel>(_ => mainModel.Extensions);
            serviceCollection.AddSingleton<OptionsModel>(_ => mainModel.Options);
            serviceCollection.AddSingleton<MainModel>(_ => mainModel);
        }

        private void ConfigureCommonServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DisplayService>();
            serviceCollection.AddSingleton<ExtensionsEditDialog>();
            serviceCollection.AddSingleton<ProgressDialog>();
            serviceCollection.AddSingleton<ISerializationService>(_ => _serializationService);
            serviceCollection.AddSingleton<IFileService>(_ => _fileService);
        }

        private MainModel LoadModel()
        {
            if(!_fileService.FileExists(SettingsFile))
            {
                return new MainModel();
                
            }
            return _serializationService.Deserialize<MainModel>(SettingsFile);
        }
    }
}
