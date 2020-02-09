using Autofac;
using BcFileTool.Commands;
using BcFileTool.DI;
using BcFileTool.Options;
using CommandLine;
using System;
using System.Diagnostics;

namespace BcFileTool
{
    class Program
    {
        const int ErrorLevelOk = 0;
        const int ErrorLevelError = 1;

        static int Main(string[] args)
        {
            int errorlevel = ErrorLevelOk;
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();

                var result = CommandLine.Parser.Default.ParseArguments(args,
                    typeof(ConfigOptions));

                if (result.Tag == CommandLine.ParserResultType.Parsed)
                {
                    var parsed = (Parsed<object>)result;

                    if (!((GeneralOptions)parsed.Value).MeasureTime)
                    {
                        stopwatch = null;
                    }
                   
                    IBcCommand command = null;
                    if (parsed.Value is ConfigOptions)
                    {                       
                        command = DIContainer.Instance.ResolveKeyed<IBcCommand>("Config");
                        ((ConfigCommand)command).Options = (ConfigOptions)parsed.Value;
                    }

                    if(command != null)
                    {
                        command.Execute();
                    }
                }
                else
                {
                    Console.WriteLine("Have no idea whatya want.");
                    errorlevel = ErrorLevelError;
                }
            }
            catch
            {
                Environment.ExitCode = ErrorLevelError;
                throw;
            }
            finally
            {
                if (stopwatch != null)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
                }

                Console.WriteLine("That's all for now.");
            }
            return errorlevel;
        }
    }
}
