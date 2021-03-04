using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using BcFileTool.CGUI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Bootstrap
{
    public class Bootstrapper
    {
        IServiceProvider _serviceProvider;

        public Bootstrapper SetUpDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            return this;
        }

        public MainView CreateMainView()
        {
            var mainView = _serviceProvider.GetRequiredService<MainView>();

            return mainView;
        }

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
            serviceCollection.AddSingleton<SourcesModel>();
            serviceCollection.AddSingleton<ExtensionsModel>();
            serviceCollection.AddSingleton<OptionsModel>();
        }

        private void ConfigureCommonServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DisplayErrorService>();
        }
    }
}
