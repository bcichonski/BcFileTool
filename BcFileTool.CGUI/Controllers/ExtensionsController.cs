using BcFileTool.CGUI.Dialogs.ExtensionsEdit;
using BcFileTool.CGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Controllers
{
    public class ExtensionsController
    {
        ExtensionsModel _model;

        public ExtensionsController(ExtensionsModel model)
        {
            _model = model;
        }

        public void LoadExtensions()
        {
            _model.Add(new FileExtensions()
            {
                ExtensionList = new List<string> { ".jpg", ".jpeg" },
                OutputSubdir = "pictures"
            });
        }

        internal void AddNew()
        {
            var editDialog = new ExtensionsEditDialog(new FileExtensions());
            editDialog.ShowModal();

            if(!editDialog.Cancelled)
            {
                _model.Add(editDialog.Result);
            }
        }
    }
}
