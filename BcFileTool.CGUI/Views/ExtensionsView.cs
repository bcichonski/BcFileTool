using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
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
            _extensionsListView.KeyPress += _extensionsListView_KeyPress;

            _removeButton = new Button("Rem_ove");
            _removeButton.Y = Pos.Bottom(_extensionsListView);
            _removeButton.X = 1;
            _removeButton.HotKeySpecifier = '_';
            _removeButton.Clicked += _removeButton_Clicked;

            _addButton = new Button("A_dd");
            _addButton.Y = Pos.Bottom(_extensionsListView);
            _addButton.X = Pos.Center();
            _addButton.Clicked += _addButton_Clicked;
            _addButton.HotKeySpecifier = '_';

            _editButton = new Button("Edit");
            _editButton.Y = Pos.Bottom(_extensionsListView);
            _editButton.X = Pos.Right(_extensionsListView) - 9;
            _editButton.Clicked += _editButton_Clicked;

            Add(_extensionsListView);
            Add(_removeButton);
            Add(_addButton);
            Add(_editButton);
        }

        private void _editButton_Clicked()
        {
            if(_controller.Edit(_extensionsListView.SelectedItem))
            {
                _extensionsListView.SetNeedsDisplay();
            }
        }

        private void _removeButton_Clicked()
        {
            if(_controller.Remove(_extensionsListView.SelectedItem))
            {
                _extensionsListView.SetNeedsDisplay();
            }
        }

        private void _extensionsListView_KeyPress(KeyEventEventArgs obj)
        {
            if (obj.KeyEvent.IsCtrl && obj.KeyEvent.Key == Key.ControlO)
            {
                //_removeButton_Clicked();
                obj.Handled = true;
            }
            else if (obj.KeyEvent.IsCtrl && obj.KeyEvent.Key == Key.ControlD)
            {
                _addButton_Clicked();
                obj.Handled = true;
            }
        }

        private void _addButton_Clicked()
        {
            _controller.AddNew();
            _extensionsListView.SetNeedsDisplay();
        }
    }
}
