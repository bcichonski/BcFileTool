using BcFileTool.CGUI.Dialogs.Progress;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using BcFileTool.CGUI.Views;
using BcFileTool.Library.Enums;
using System;
using System.Linq;

namespace BcFileTool.CGUI.Controllers
{
    public class OptionsController : BaseController<OptionsView>
    {
        OptionsModel _model;
        DisplayService _displayService;
        ProgressDialog _progressDialog;

        public OptionsController(OptionsModel model, DisplayService displayService, ProgressDialog progressDialog)
        {
            _model = model;
            _displayService = displayService;
            _progressDialog = progressDialog;
        }

        internal void OnActionChanged(FileAction fileAction)
        {
            SetValue(_model.Action, fileAction, x => _model.Action = x);
        }

        internal void OnDateDirectioriesToggled(bool obj)
        {
            SetValue(_model.DateDirectories, obj, x => _model.DateDirectories = obj);
        }

        internal void OnPreserveSubdirectioriesToggled(bool obj)
        {
            SetValue(_model.PreserveSubdirectories, obj, x => _model.PreserveSubdirectories = obj);
        }

        internal void OnVerifyChecksumToggled(bool obj)
        {
            SetValue(_model.VerifyChecksum, obj, x => _model.VerifyChecksum = obj);
        }

        internal void OnSkipToggled(bool obj)
        {
            SetValue(_model.Skip, obj, x => _model.Skip = obj);
        }

        private void SetValue<T>(T oldValue, T newValue, Action<T> setter)
        {
            if (!oldValue.Equals(newValue))
            {
                setter(newValue);

                OnChange();
            }
        }

        internal void OnStart()
        {
            var validationResult = Validate();

            if (validationResult.IsValid)
            {
                ConfirmAndRun();
            }
            else
            {
                _displayService.ShowInformation($"Some issues were found:\n {string.Join('\n', validationResult.Issues.Select(x => " * " + x))}");
            }
        }

        private void ConfirmAndRun()
        {
            if (_displayService.ShowConfirmation("Shall we?"))
            {
                _progressDialog.ShowModal();
            }
        }
    }
}
