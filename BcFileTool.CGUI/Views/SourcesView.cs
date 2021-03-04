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
        DisplayErrorService _displayErrorService;

        ListView _sourcesListView;
        Button _removeButton;
        Button _addButton;

        SourcesController _controller;
        SourcesModel _model;

        public SourcesView(SourcesModel model, SourcesController controller, DisplayErrorService displayErrorService) : base("Source directories")
        {
            _displayErrorService = displayErrorService;
            _controller = controller;
            _model = model;

            _controller.LoadSources();
            CreateComponents();
        }

        private void CreateComponents()
        {
            _sourcesListView = new ListView(_model.Sources);
            _sourcesListView.AllowsMarking = true;
            _sourcesListView.Width = Dim.Fill();
            _sourcesListView.Height = Dim.Fill() - 1;

            _removeButton = new Button("Remove");
            _removeButton.Y = Pos.Bottom(_sourcesListView);
            _removeButton.X = 1;

            _addButton = new Button("Add");
            _addButton.Y = Pos.Bottom(_sourcesListView);
            _addButton.X = Pos.Right(_sourcesListView) - 8;

            Add(_sourcesListView);
            Add(_removeButton);
            Add(_addButton);
        }

        public void ShowException(Exception e)
        {
            _displayErrorService.ShowException(e);
        }
    }
}
