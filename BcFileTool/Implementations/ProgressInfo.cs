using BcFileTool.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcFileTool.Implementations
{
    public class ProgressInfo : IProgressInfo
    {
        int _percentage;
        public int Percentage { get => _percentage; set => SetValue(ref _percentage, value); }

        int _errors;
        public int Errors { get => _errors; set => SetValue(ref _errors, value); }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void SetValue(ref int variable, int value)
        {
            if(variable != value)
            {
                variable = value;

                ShowProgress();
            }
        }

        private void ShowProgress()
        {
            Console.Write($"\rProcessed {_percentage}% with {_errors} errors...");
        }
    }
}
