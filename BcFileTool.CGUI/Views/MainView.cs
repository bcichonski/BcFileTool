using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Services;
using System;
using Terminal.Gui;

namespace BcFileTool.CGUI.Views
{
    public class MainView : Window, IHandleExceptions
    {
        DisplayService _displayService;
        SourcesView _sourcesView;
        ExtensionsView _extensionsView;
        OptionsView _optionsView;

        public MainView(SourcesView sourcesView, 
            ExtensionsView extensionsView,
            OptionsView optionsView,
            DisplayService displayService) : base("BcFileTool - Console Graphical User Interface")
        {
            _displayService = displayService;
            _sourcesView = sourcesView;
            _extensionsView = extensionsView;
            _optionsView = optionsView;

            CreateComponents();
        }

        private void CreateComponents()
        {
            _sourcesView.Width = Dim.Percent(50);
            _sourcesView.Height = Dim.Percent(70);

            Add(_sourcesView);

            _extensionsView.X = Pos.Right(_sourcesView);
            _extensionsView.Width = Dim.Percent(50);
            _extensionsView.Height = Dim.Percent(70);
            Add(_extensionsView);

            _optionsView.Y = Pos.Bottom(_sourcesView);
            _optionsView.Width = Dim.Fill();
            _optionsView.Height = Dim.Fill();
            Add(_optionsView);
        }

        public void ShowException(Exception e)
        {
            _displayService.ShowException(e);
        }
    }
}