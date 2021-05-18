using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using BcFileTool.Library.Enums;
using System;
using System.Linq;
using Terminal.Gui;

namespace BcFileTool.CGUI.Views
{
    public class OptionsView : FrameView, IHandleExceptions
    {
        OptionsController _controller;
        OptionsModel _model;
        DisplayService _displayService;

        Label _cbxActionLabel;
        ComboBox _cbxAction;
        CheckBox _cbVerbose;
        CheckBox _cbSkip;
        CheckBox _cbPreserveSubdirectories;
        CheckBox _cbDateDirectories;
        CheckBox _cbVerifyChecksum;
        Label _outputDirLabel;
        TextField _outputDirText;
        Button _outputDirButton;

        Button _btnStart;

        public OptionsView(OptionsController controller, OptionsModel model, DisplayService displayService) : base("Options")
        {
            _controller = controller;
            _model = model;
            _displayService = displayService;

            CreateComponents();
        }

        private void CreateComponents()
        {
            _outputDirLabel = new Label("Output directory:");
            _outputDirLabel.X = 0;
            _outputDirLabel.Y = 0;

            _outputDirButton = new Button("...");
            _outputDirButton.Width = 7;
            _outputDirButton.Y = Pos.Y(_outputDirLabel);
            _outputDirButton.X = Pos.AnchorEnd(7) - 1;
            _outputDirButton.Clicked += _outputDirButton_Clicked;

            _outputDirText = new TextField(_model.OutputDirectory ?? "");
            _outputDirText.Y = Pos.Y(_outputDirLabel);
            _outputDirText.X = Pos.Right(_outputDirLabel) + 1;
            _outputDirText.Width = Dim.Fill(9);
            _outputDirText.TextChanged += _outputDirText_TextChanged;

            var cbxActionOptions = Enum.GetValues<FileAction>()
                .Select(x => x.ToString())
                .ToList();

            _cbxActionLabel = new Label("Action");
            _cbxActionLabel.X = 0;
            _cbxActionLabel.Y = Pos.Bottom(_outputDirLabel) + 1;

            _cbxAction = new ComboBox();
            _cbxAction.SetSource(cbxActionOptions);
            _cbxAction.X = Pos.Right(_cbxActionLabel) + 1;
            _cbxAction.Y = Pos.Y(_cbxActionLabel);
            _cbxAction.Width = Dim.Sized(cbxActionOptions.Max(x => x.Length) + 4);
            _cbxAction.Height = Dim.Sized(cbxActionOptions.Count) + 1;
            _cbxAction.Text = cbxActionOptions.FirstOrDefault(x => x == _model.Action.ToString()) ?? "";
            _cbxAction.SelectedItemChanged += _cbxAction_SelectedItemChanged;

            _cbDateDirectories = new CheckBox("Put files in date dirs", _model.DateDirectories);
            _cbDateDirectories.X = Pos.Left(_cbxActionLabel);
            _cbDateDirectories.Y = Pos.Bottom(_cbxActionLabel);
            _cbDateDirectories.Checked = _model.DateDirectories;
            _cbDateDirectories.Toggled += _cbDateDirectories_Toggled;

            _cbPreserveSubdirectories = new CheckBox("Preserve subdirectories", _model.PreserveSubdirectories);
            _cbPreserveSubdirectories.X = Pos.Left(_cbDateDirectories);
            _cbPreserveSubdirectories.Y = Pos.Bottom(_cbDateDirectories);
            _cbPreserveSubdirectories.Toggled += _cbPreserveSubdirectories_Toggled;
            _cbPreserveSubdirectories.Checked = _model.PreserveSubdirectories;

            _cbVerifyChecksum = new CheckBox("Verify checksum of files", _model.VerifyChecksum);
            _cbVerifyChecksum.X = Pos.Percent(50);
            _cbVerifyChecksum.Y = Pos.Y(_cbxActionLabel);
            _cbVerifyChecksum.Toggled += _cbVerifyChecksum_Toggled;
            _cbVerifyChecksum.Checked = _model.VerifyChecksum;

            _cbSkip = new CheckBox("Skip already existing files", _model.Skip);
            _cbSkip.X = Pos.Percent(50);
            _cbSkip.Y = Pos.Bottom(_cbVerifyChecksum);
            _cbSkip.Toggled += _cbSkip_Toggled;
            _cbSkip.Checked = _model.Skip;

            _cbVerbose = new CheckBox("Verbose mode", _model.Verbose);
            _cbVerbose.X = Pos.Percent(50);
            _cbVerbose.Y = Pos.Bottom(_cbSkip);
            _cbVerbose.Checked = _model.Verbose;
            _cbVerbose.Toggled += _cbVerbose_Toggled;

            _btnStart = new Button("Start", true);
            _btnStart.Y = Pos.Percent(100) - 1;
            _btnStart.X = Pos.Right(this) - 14;
            _btnStart.Height = 1;
            _btnStart.Clicked += _btnStart_Clicked;

            Add(_outputDirLabel);
            Add(_outputDirText);
            Add(_outputDirButton);
            Add(_cbxActionLabel);
            Add(_cbxAction);
            Add(_cbDateDirectories);
            Add(_cbPreserveSubdirectories);
            Add(_cbVerifyChecksum);
            Add(_cbSkip);
            Add(_cbVerbose);
            Add(_btnStart);
        }

        private void _cbVerbose_Toggled(bool obj)
        {
            _controller.OnVerboseToggled(!obj);
        }

        private void _outputDirText_TextChanged(NStack.ustring obj)
        {
            _controller.OnOutputDirChanged(obj.ToString());
        }

        private void _outputDirButton_Clicked()
        {
            var path = _displayService.DirectoryDialog("Set output", "Please select a directory");

            if (!string.IsNullOrWhiteSpace(path))
            {
                _outputDirText.Text = path;
            }
        }

        private void _btnStart_Clicked()
        {
            _controller.OnStart();
        }

        private void _cbSkip_Toggled(bool obj)
        {
            _controller.OnSkipToggled(!obj);
        }

        private void _cbVerifyChecksum_Toggled(bool obj)
        {
            _controller.OnVerifyChecksumToggled(!obj);
        }

        private void _cbPreserveSubdirectories_Toggled(bool obj)
        {
            _controller.OnPreserveSubdirectioriesToggled(!obj);
        }

        private void _cbDateDirectories_Toggled(bool obj)
        {
            _controller.OnDateDirectioriesToggled(!obj);
        }

        private void _cbxAction_SelectedItemChanged(ListViewItemEventArgs obj)
        {
            _controller.OnActionChanged(Enum.Parse<FileAction>(obj.Value.ToString(), true));
        }

        public void ShowException(Exception e)
        {
            _displayService.ShowException(e);
        }
    }
}
