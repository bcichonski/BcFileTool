using BcFileTool.CGUI.Models;
using Terminal.Gui;

namespace BcFileTool.CGUI.Views
{
    public class MainView : Window
    {
        public MainView(SourcesView sourcesView, 
            ExtensionsView extensionsView,
            OptionsView optionsView) : base("BcFileTool - Console Graphical User Interface")
        {
            sourcesView.Width = Dim.Percent(50);
            sourcesView.Height = Dim.Percent(70);

            Add(sourcesView);

            extensionsView.X = Pos.Right(sourcesView);
            extensionsView.Width = Dim.Percent(50);
            extensionsView.Height = Dim.Percent(70);
            Add(extensionsView);

            optionsView.Y = Pos.Bottom(sourcesView);
            optionsView.Width = Dim.Fill();
            optionsView.Height = Dim.Fill();
            Add(optionsView);
        }
    }
}