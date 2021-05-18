using BcFileTool.CGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BcFileTool.CGUI.Dialogs.ExtensionsEdit
{
    class ExtensionsEditView : Window
    {
        FileExtensions _model;
        ExtensionsEditDialog _controller;

        TextField _extensionsTextField;
        TextField _outputDirectoryTextField;
        Button _okButton;
        Button _cancelButton;

        public ExtensionsEditView(FileExtensions model, ExtensionsEditDialog controller) : base("Edit extensions")
        {
            _model = model;
            _controller = controller;

            this.Width = Dim.Sized(60);
            this.Height = Dim.Sized(8);
            this.X = Pos.Center();
            this.Y = Pos.Center();

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var extensionsLabel = new Label("Extensions:");
            _extensionsTextField = new TextField(_model.GetExtensions());
            _extensionsTextField.Y = Pos.Bottom(extensionsLabel);
            _extensionsTextField.Width = Dim.Fill();

            Add(extensionsLabel, _extensionsTextField);

            var outputLabel = new Label("Output directory:");
            outputLabel.Y = Pos.Bottom(_extensionsTextField);

            _outputDirectoryTextField = new TextField(_model.OutputSubdir);
            _outputDirectoryTextField.Y = Pos.Bottom(outputLabel);
            _outputDirectoryTextField.Width = Dim.Fill();

            Add(outputLabel, _outputDirectoryTextField);

            _cancelButton = new Button("Cancel");
            _cancelButton.Y = Pos.Bottom(outputLabel) + 2;
            _cancelButton.X = 1;
            _cancelButton.Clicked += _cancelButton_Clicked;

            _okButton = new Button("Ok", true);
            _okButton.Y = Pos.Bottom(outputLabel) + 2;
            _okButton.X = Pos.AnchorEnd(9);
            _okButton.Clicked += _okButton_Clicked;

            Add(_okButton, _cancelButton);
        }

        private void _okButton_Clicked()
        {
            _controller.Ok(_outputDirectoryTextField.Text.ToString(), 
                _extensionsTextField.Text.ToString());
        }

        private void _cancelButton_Clicked()
        {
            _controller.Cancel();
        }
    }
}
