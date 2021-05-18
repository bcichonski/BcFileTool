using System;
using System.Linq;
using System.Text;
using System.Threading;
using Terminal.Gui;

namespace BcFileTool.CGUI.Dialogs.Progress
{
    class ProgressView : Window
    {
        private ProgressDialog _progressDialog;
        private ProgressBar _progressBar;
        private Button _closeButton;
        private TextView _textView;
        private StringBuilder _logMessages;
        volatile int _percentage;
        object _timerToken;

        public ProgressView(ProgressDialog progressDialog)
        {
            _progressDialog = progressDialog;
            _logMessages = new StringBuilder();
            _percentage = 0;

            Width = Dim.Percent(80);
            Height = Dim.Percent(80);
            X = Pos.Center();
            Y = Pos.Center();

            InitializeComponents();

            _timerToken = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), UpdateTimer);
        }

        private bool UpdateTimer(MainLoop arg)
        {
            _progressBar.Fraction = _percentage / 100f;
            _progressBar.SetNeedsDisplay();

            var messages = _logMessages.ToString();
            var lines = messages.ToCharArray().Where(x => x == '\n').Count();

            _textView.Text = messages;
            _textView.ScrollTo(lines);
            _textView.SetNeedsDisplay();

            return true;
        }

        private void InitializeComponents()
        {
            _progressBar = new ProgressBar();
            _progressBar.Y = 0;
            _progressBar.X = 0;
            _progressBar.Width = Dim.Fill();
            _progressBar.Fraction = 0f;

            _textView = new TextView();
            _textView.Y = 2;
            _textView.X = 0;
            _textView.Width = Dim.Fill();
            _textView.Height = Dim.Fill(2);
            _textView.ReadOnly = true;

            _closeButton = new Button("Close", true);
            _closeButton.Width = 12;
            _closeButton.X = Pos.AnchorEnd(12);
            _closeButton.Y = Pos.AnchorEnd(1);           
            _closeButton.Clicked += _closeButton_Clicked;

            Add(_progressBar, 
                _textView, 
                _closeButton);
        }

        internal void PercentageChanged(int percentage, int errors)
        {
            _percentage = percentage;
        }

        private void _closeButton_Clicked()
        {
            Application.MainLoop.RemoveTimeout(_timerToken);
            _progressDialog.Close();
        }

        internal void LogMessage(string message)
        {
            _logMessages.Append(message);
            _logMessages.Append('\n');
        }
    }
}
