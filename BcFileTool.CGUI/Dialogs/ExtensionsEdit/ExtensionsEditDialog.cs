using BcFileTool.CGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Result = _model;
        }
    }
}
