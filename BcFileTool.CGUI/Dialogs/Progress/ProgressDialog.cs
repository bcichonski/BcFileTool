using BcFileTool.CGUI.Models;
using BcFileTool.Library.Engine;
using BcFileTool.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BcFileTool.CGUI.Dialogs.Progress
{
    public class ProgressDialog : IProgressInfo
    {
        ProgressView _view;
        Engine _engine;

        int _percentage;
        public int Percentage { get => _percentage; set => SetValue(ref _percentage, value); }

        int _errors;
        public int Errors { get => _errors; set => SetValue(ref _errors, value); }

        public ProgressDialog(MainModel model)
        {
            _view = new ProgressView(this);

            _engine = new Engine(model.ToConfiguration(),
                this,
                model.Options.Verbose,
                model.Options.Skip,
                model.Options.PreserveSubdirectories,
                model.Options.DateDirectories,
                model.Options.VerifyChecksum);
        }

        public void ShowModal()
        {
            Task.Run(() =>
            {
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
            lock(_view)
            {
                _view.LogMessage(message);
            }
        }

        private void SetValue(ref int variable, int value)
        {
            if(variable != value)
            {
                variable = value;
                _view.PercentageChanged(_percentage, _errors);
            }
        }
    }
}
