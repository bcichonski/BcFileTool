using System;
using Terminal.Gui;
using BcFileTool.CGUI.Views;
using BcFileTool.CGUI.Bootstrap;

namespace BcFileTool.CGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            var mainView = new Bootstrapper()
                .SetUpDependencyInjection()
                .CreateMainView();

            Application.Run(mainView);
        }
    }
}
