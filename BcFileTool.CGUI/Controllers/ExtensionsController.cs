using BcFileTool.CGUI.Dialogs.ExtensionsEdit;
using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using BcFileTool.CGUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Controllers
{
    public class ExtensionsController : BaseController<ExtensionsView>
    {
        ExtensionsModel _model;
        DisplayService _displayService;

        public ExtensionsController(ExtensionsModel model, DisplayService displayService)
        {
            _model = model;
            _displayService = displayService;
        }

        internal bool AddNew()
        {
            var editDialog = new ExtensionsEditDialog(new FileExtensions());
            editDialog.ShowModal();

            if(!editDialog.Cancelled)
            {
                _model.Add(editDialog.Result);

                OnChange();

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

                OnChange();

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

                OnChange();

                return true;
            }

            return false;
        }

        public override IValidationResult ValidateModel()
        {
            var result = new ValidationResult();

            if(_model.Extensions.Count == 0)
            {
                result.AddIssue("No extension has been added to process");
            }

            return result;
        }
    }
}
