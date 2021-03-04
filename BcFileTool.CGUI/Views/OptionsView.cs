using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
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

        Label _cbxActionLabel;
        ComboBox _cbxAction;
        CheckBox _cbVerbose;
        CheckBox _cbSkip;
        CheckBox _cbPreserveSubdirectories;
        CheckBox _cbDateDirectories;
        CheckBox _cbVerifyChecksum;

        Button _btnStart;

        public OptionsView(OptionsController controller, OptionsModel model) : base("Options")
        {
            _controller = controller;
            _model = model;

            CreateComponents();
        }

        private void CreateComponents()
        {
            var cbxActionOptions = Enum.GetValues<FileAction>()
                .Select(x => x.ToString())
                .ToList();

            _cbxActionLabel = new Label("Action");
            _cbxActionLabel.X = 0;
            _cbxActionLabel.Y = 0;

            _cbxAction = new ComboBox();
            _cbxAction.SetSource(cbxActionOptions);
            _cbxAction.X = Pos.Right(_cbxActionLabel) + 1;
            _cbxAction.Y = Pos.Y(_cbxActionLabel);
            _cbxAction.Width = Dim.Sized(cbxActionOptions.Max(x => x.Length) + 4);
            _cbxAction.Height = Dim.Sized(cbxActionOptions.Count) + 1;
            _cbxAction.Text = cbxActionOptions.FirstOrDefault(x => x == _model.Action.ToString()) ?? "";

            _cbDateDirectories = new CheckBox("Put files in date dirs", _model.DateDirectories);
            _cbDateDirectories.X = Pos.Left(_cbxActionLabel);
            _cbDateDirectories.Y = Pos.Bottom(_cbxActionLabel);

            _cbPreserveSubdirectories = new CheckBox("Preserve subdirectories", _model.PreserveSubdirectories);
            _cbPreserveSubdirectories.X = Pos.Left(_cbDateDirectories);
            _cbPreserveSubdirectories.Y = Pos.Bottom(_cbDateDirectories);

            _cbVerifyChecksum = new CheckBox("Verify checksum of files", _model.VerifyChecksum);
            _cbVerifyChecksum.X = Pos.Percent(50);
            _cbVerifyChecksum.Y = Pos.Y(_cbxActionLabel);

            _cbSkip = new CheckBox("Skip already existing files", _model.Skip);
            _cbSkip.X = Pos.Percent(50);
            _cbSkip.Y = Pos.Bottom(_cbVerifyChecksum);

            _cbVerbose = new CheckBox("Verbose mode", _model.Verbose);
            _cbVerbose.X = Pos.Percent(50);
            _cbVerbose.Y = Pos.Bottom(_cbSkip);

            _btnStart = new Button("Start", true);
            _btnStart.Y = Pos.Percent(100) - 1;
            _btnStart.X = Pos.Right(this) - 14;
            _btnStart.Height = 1;

            Add(_cbxActionLabel);
            Add(_cbxAction);
            Add(_cbDateDirectories);
            Add(_cbPreserveSubdirectories);
            Add(_cbVerifyChecksum);
            Add(_cbSkip);
            Add(_cbVerbose);
            Add(_btnStart);
        }

        public void ShowException(Exception e)
        {
            throw new NotImplementedException();
        }
    }
}
