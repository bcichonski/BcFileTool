using BcFileTool.CGUI.Models;
using BcFileTool.Library.Engine;
using BcFileTool.Library.Interfaces;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BcFileTool.CGUI.Dialogs.Progress
{
    public class ProgressDialog : IProgressInfo
    {
        ProgressView _view;
        Engine _engine;
        MainModel _model;

        int _percentage;
        public int Percentage { get => _percentage; set => SetValue(ref _percentage, value); }

        int _errors;
        public int Errors { get => _errors; set => SetValue(ref _errors, value); }

        public ProgressDialog(MainModel model)
        {
            _view = new ProgressView(this);
            _model = model;
        }

        public void ShowModal()
        {
            Task.Run(() =>
            {
                _engine = new Engine(_model.ToConfiguration(),
                    this,
                    _model.Options.Verbose,
                    _model.Options.Skip,
                    _model.Options.PreserveSubdirectories,
                    _model.Options.DateDirectories,
                    _model.Options.VerifyChecksum);

                var files = _engine.GetAllFiles();
                _engine.ProcessFiles(files);
            });
            Application.Run(_view);
        }

        public void Close()
        {
            Application.RequestStop();
        }

        public void Log(string message)
        {
            lock (_view)
            {
                _view.LogMessage(message);
            }
        }

        private void SetValue(ref int variable, int value)
        {
            if (variable != value)
            {
                variable = value;
                _view.PercentageChanged(_percentage, _errors);
            }
        }

        public bool Indeterminate => _model.Options.Action == Library.Enums.FileAction.Info;
    }
}
