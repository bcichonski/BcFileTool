using BcFileTool.CGUI.Dialogs.Progress;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using BcFileTool.CGUI.Views;
using BcFileTool.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace BcFileTool.CGUI.Controllers
{
    public class OptionsController : BaseController<OptionsView>
    {
        OptionsModel _model;
        SourcesModel _sourcesModel;
        DisplayService _displayService;
        ProgressDialog _progressDialog;

        public OptionsController(OptionsModel model, DisplayService displayService, ProgressDialog progressDialog, SourcesModel sourcesModel)
        {
            _model = model;
            _sourcesModel = sourcesModel;
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

        internal void OnVerboseToggled(bool obj)
        {
            SetValue(_model.Verbose, obj, x => _model.Verbose = obj);
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
            var message = _sourcesModel
                .Sources
                .Where(x => x.Selected)
                .Select(x => x.Name)
                .ToList();
            message.Insert(0, "Shall we?");

            if (_displayService.ShowConfirmation(string.Join(Environment.NewLine, message)))
            {
                _progressDialog.ShowModal();
            }
        }

        internal void OnOutputDirChanged(string path)
        {
            _model.OutputDirectory = path;

            OnChange();
        }

        internal void OnExitClicked()
        {
            Application.RequestStop();
            Environment.Exit(0);
        }
    }
}
