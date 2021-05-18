using BcFileTool.CGUI.Bootstrap;
using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Views;
using BcFileTool.Library.Interfaces.Services;

namespace BcFileTool.CGUI.Controllers
{
    public class MainController : BaseController<MainView>
    {
        MainModel _mainModel;
        ISerializationService _serializationService;
        ExtensionsController _extensionsController;
        OptionsController _optionsController;
        SourcesController _sourcesController;

        public MainView View => this._view;

        public MainController(MainModel mainModel,
            ExtensionsController extensionsController,
            OptionsController optionsController,
            SourcesController sourcesController,
            MainView mainView, 
            ISerializationService serializationService)
        {
            extensionsController.SetOnChange(this);
            extensionsController.SetValidationParent(this);
            optionsController.SetOnChange(this);
            optionsController.SetValidationParent(this);
            sourcesController.SetOnChange(this);
            sourcesController.SetValidationParent(this);

            _serializationService = serializationService;
            _mainModel = mainModel;
            _extensionsController = extensionsController;
            _optionsController = optionsController;
            _sourcesController = sourcesController;
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

        public override IValidationResult Validate()
        {
            var result = _extensionsController.ValidateModel();
            result.Merge(_optionsController.ValidateModel());
            result.Merge(_sourcesController.ValidateModel());

            return result;
        }
    }
}
