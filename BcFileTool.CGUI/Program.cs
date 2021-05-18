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

            var mainController = new Bootstrapper()
                .SetUpDependencyInjection()
                .CreateMainController();

            Application.Run(mainController.View);
        }
    }
}
