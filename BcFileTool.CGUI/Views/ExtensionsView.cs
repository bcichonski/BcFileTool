using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BcFileTool.CGUI.Views
{
    public class ExtensionsView : FrameView
    {
        ListView _extensionsListView;
        ExtensionsModel _model;
        ExtensionsController _controller;

        Button _addButton;
        Button _editButton;
        Button _removeButton;

        public ExtensionsView(ExtensionsModel model, ExtensionsController controller) : base("Extensions")
        {
            _controller = controller;
            _model = model;

            _controller.LoadExtensions();
            CreateComponents();
        }

        private void CreateComponents()
        {
            _extensionsListView = new ListView(_model.Extensions);

            Width = Dim.Percent(50);
            Height = Dim.Percent(70);

            _extensionsListView.AllowsMarking = false;
            _extensionsListView.Width = Dim.Fill();
            _extensionsListView.Height = Dim.Fill() - 1;

            _removeButton = new Button("Remove");
            _removeButton.Y = Pos.Bottom(_extensionsListView);
            _removeButton.X = 1;

            _addButton = new Button("Add");
            _addButton.Y = Pos.Bottom(_extensionsListView);
            _addButton.X = Pos.Center();
            _addButton.Clicked += _addButton_Clicked;

            _editButton = new Button("Edit");
            _editButton.Y = Pos.Bottom(_extensionsListView);
            _editButton.X = Pos.Right(_extensionsListView) - 9;

            Add(_extensionsListView);
            Add(_removeButton);
            Add(_editButton);
            Add(_addButton);
        }

        private void _addButton_Clicked()
        {
            _controller.AddNew();
        }
    }
}
