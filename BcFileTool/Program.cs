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
                    typeof(ConfigOptions),
                    typeof(ScanOptions));

                HandleAction(ref errorlevel, ref stopwatch, result);
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

        private static void HandleAction(ref int errorlevel, ref Stopwatch stopwatch, ParserResult<object> result)
        {
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
                } else
                if (parsed.Value is ScanOptions)
                {
                    command = DIContainer.Instance.ResolveKeyed<IBcCommand>("Scan");
                    ((ScanCommand)command).Options = (ScanOptions)parsed.Value;
                }

                if (command != null)
                {
                    command.Execute();
                }
            }
            else
            {
                Console.WriteLine("No real stuff to do.");
                errorlevel = ErrorLevelError;
            }
        }
    }
}
