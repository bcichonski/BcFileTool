using BcFileTool.CGUI.Controllers;
using BcFileTool.CGUI.Interfaces;
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
    public class SourcesView : FrameView, IHandleExceptions
    {
        DisplayErrorService _displayErrorService;

        ListView _sourcesListView;

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
            Width = Dim.Percent(50);
            Height = Dim.Percent(70);

            _sourcesListView = new ListView(_model.Sources);
            _sourcesListView.AllowsMarking = true;
            _sourcesListView.Width = Dim.Fill();
            _sourcesListView.Height = Dim.Fill();
            
            Add(_sourcesListView);
        }

        public void ShowException(Exception e)
        {
            _displayErrorService.ShowException(e);
        }
    }
}
