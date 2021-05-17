using BcFileTool.CGUI.Bootstrap;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Views;
using BcFileTool.Library.Interfaces.Services;

namespace BcFileTool.CGUI.Controllers
{
    public class MainController : BaseController<MainView>
    {
        MainModel _mainModel;
        ISerializationService _serializationService;

        public MainView View => this._view;

        public MainController(MainModel mainModel,
            ExtensionsController extensionsController,
            OptionsController optionsController,
            SourcesController sourcesController,
            MainView mainView, 
            ISerializationService serializationService)
        {
            extensionsController.SetOnChange(this);
            optionsController.SetOnChange(this);
            sourcesController.SetOnChange(this);

            _serializationService = serializationService;
            _mainModel = mainModel;
            SetView(mainView);
        }

        private void SaveModel()
        {
            _serializationService.Serialize(Bootstrapper.SettingsFile, _mainModel);
        }

        public override void OnChange()
        {
            SaveModel();
        }
    }
}
