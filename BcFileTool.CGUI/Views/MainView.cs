using BcFileTool.CGUI.Models;
using Terminal.Gui;

namespace BcFileTool.CGUI.Views
{
    public class MainView : Window
    {
        public MainView(SourcesView sourcesView) : base("BcFileTool - Console Graphical User Interface")
        {
            Add(sourcesView);
        }
    }
}