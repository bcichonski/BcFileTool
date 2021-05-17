using BcFileTool.CGUI.Dialogs.ExtensionsEdit;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
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
        DisplayService _displayService;

        public ExtensionsController(ExtensionsModel model, DisplayService displayService)
        {
            _model = model;
            _displayService = displayService;
        }

        public void LoadExtensions()
        {
            _model.Add(new FileExtensions()
            {
                ExtensionList = new List<string> { ".jpg", ".jpeg" },
                OutputSubdir = "pictures",
                IsNew = false
            });
        }

        internal bool AddNew()
        {
            var editDialog = new ExtensionsEditDialog(new FileExtensions());
            editDialog.ShowModal();

            if(!editDialog.Cancelled)
            {
                _model.Add(editDialog.Result);
                return true;
            }

            return false;
        }

        internal bool Remove(int selectedItem)
        {
            var text = _model.Extensions[selectedItem].ToString();
            if (_displayService.ShowConfirmation($"Are you sure to remove\n{text}?"))
            {
                _model.RemoveAt(selectedItem);
                return true;
            }
            return false;
        }

        internal bool Edit(int selectedItem)
        {
            var editDialog = new ExtensionsEditDialog(new FileExtensions(_model.Extensions[selectedItem]));
            editDialog.ShowModal();

            if (!editDialog.Cancelled)
            {
                _model.Reconcile(editDialog.Result);
                return true;
            }

            return false;
        }
    }
}
