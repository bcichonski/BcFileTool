using BcFileTool.CGUI.Bootstrap;
using System;
using System.Linq;
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
                Application.UseSystemConsole = (args.Any(a => a.ToLowerInvariant() == "--safe-mode"));
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
