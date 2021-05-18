using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using System;
using Terminal.Gui;

namespace BcFileTool.CGUI.Views
{
    public class SourcesView : FrameView, IHandleExceptions
    {
        DisplayService _displayService;

        ListView _sourcesListView;
        Button _removeButton;
        Button _addButton;

        SourcesController _controller;
        SourcesModel _model;

        public SourcesView(SourcesModel model, SourcesController controller, DisplayService displayService) : base("Source directories")
        {
            _displayService = displayService;
            _controller = controller;
            _model = model;

            CreateComponents();
        }

        private void CreateComponents()
        {
            _sourcesListView = new ListView(_model.Sources);
            _sourcesListView.AllowsMarking = true;
            _sourcesListView.Width = Dim.Fill();
            _sourcesListView.Height = Dim.Fill() - 1;
            _sourcesListView.KeyPress += _sourcesListView_KeyPress;

            _removeButton = new Button("Remove");
            _removeButton.Y = Pos.Bottom(_sourcesListView);
            _removeButton.X = 1;
            _removeButton.HotKey = Key.ControlR;
            _removeButton.Clicked += _removeButton_Clicked;

            _addButton = new Button("Add");
            _addButton.Y = Pos.Bottom(_sourcesListView);
            _addButton.X = Pos.Right(_sourcesListView) - 8;
            _addButton.HotKey = Key.ControlA;
            _addButton.Clicked += _addButton_Clicked;

            Add(_sourcesListView);
            Add(_removeButton);
            Add(_addButton);
        }

        private void _addButton_Clicked()
        {
            var path = _displayService.DirectoryDialog("Add source", "Please select a directory");

            if(!string.IsNullOrWhiteSpace(path))
            {
                _controller.Add(new Source(path));
                _sourcesListView.SetNeedsDisplay();
            }
        }

        private void _sourcesListView_KeyPress(KeyEventEventArgs obj)
        {
            if (obj.KeyEvent.IsCtrl && obj.KeyEvent.Key == Key.ControlR)
            {
                _removeButton_Clicked();
                obj.Handled = true;
            }
            else if(obj.KeyEvent.IsCtrl && obj.KeyEvent.Key == Key.ControlA)
            {
                _addButton_Clicked();
                obj.Handled = true;
            }
        }

        private void _removeButton_Clicked()
        {
            _controller.Remove(_sourcesListView.SelectedItem);
            _sourcesListView.SetNeedsDisplay();
        }

        public void ShowException(Exception e)
        {
            _displayService.ShowException(e);
        }
    }
}
