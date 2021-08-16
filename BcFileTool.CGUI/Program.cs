using BcFileTool.CGUI.Bootstrap;
using System;
using System.Threading;
using Terminal.Gui;

namespace BcFileTool.CGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = false;
            try
            {
                Application.UseSystemConsole = true;
                Application.Init();

                Console.WriteLine($"Running using {Application.Driver}.");
                Thread.Sleep(2000);

                var mainController = new Bootstrapper()
                    .SetUpDependencyInjection()
                    .CreateMainController();

                run = true;
                Application.Run(mainController.View);
            }
            catch { 
                throw; 
            }
            finally
            {
                Console.Clear();
                Console.WriteLine("Bye!");
            }
        }
    }
}
