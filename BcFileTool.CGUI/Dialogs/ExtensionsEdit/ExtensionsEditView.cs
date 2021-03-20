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
        Label _extensionsListLabel;
        Button _okButton;
        Button _cancelButton;

        public ExtensionsEditView(FileExtensions model, ExtensionsEditDialog controller) : base("Edit extensions", 5)
        {
            _model = model;
            _controller = controller;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var extensionsLabel = new Label("Extensions:");
            _extensionsTextField = new TextField(_model.ToString());
            _extensionsTextField.Y = Pos.Right(extensionsLabel);
            _extensionsTextField.Width = Dim.Fill();

            Add(extensionsLabel, _extensionsTextField);

            var outputLabel = new Label("Output directory:");
            outputLabel.Y = Pos.Bottom(extensionsLabel) + 1;

            _outputDirectoryTextField = new TextField(_model.OutputSubdir);
            _outputDirectoryTextField.Y = Pos.Y(outputLabel);
            _outputDirectoryTextField.Width = Dim.Fill();

            Add(outputLabel, _outputDirectoryTextField);
        }
    }
}
