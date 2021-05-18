using BcFileTool.CGUI.Models;
using Terminal.Gui;

namespace BcFileTool.CGUI.Dialogs.ExtensionsEdit
{
    public class ExtensionsEditDialog
    {
        ExtensionsEditView _view;
        FileExtensions _model;
        public bool Cancelled { get; set; }
        public FileExtensions Result { get; internal set; }

        public ExtensionsEditDialog(FileExtensions model)
        {
            _view = new ExtensionsEditView(model, this);
            _model = model;
        }

        public void ShowModal()
        {
            Application.Run(_view);
        }

        public void Ok(string outputDir, string extensions)
        {
            _model.OutputSubdir = outputDir;
            _model.Reconcile(extensions);
            Result = _model;
            Application.RequestStop();
        }

        public void Cancel()
        {
            Cancelled = true;
            Application.RequestStop();
        }
    }
}
