using System;
using CommandLine;
using Remy.Console.Commands;
using Remy.Core.Tasks;
using Serilog;
using Serilog.Core;

namespace Remy.Console
{
	public class Program
    {
        public static void Main(string[] args)
        {
			if (args.Length == 0 || args[0] == "-h" || args[0] == "--help" || args[0] == "help" || args[0] == "/?")
			{
				ShowHelp();
				return;
			}

			var levelSwitch = new LoggingLevelSwitch();
	        levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;

			var logger = new LoggerConfiguration()
								.MinimumLevel.ControlledBy(levelSwitch)
                                .WriteTo
                                .LiterateConsole()
                                .CreateLogger();


			try
			{
				var options = new Options();
				object invokedVerbInstance = null;
				var parser = new Parser(config => config.HelpWriter = null);

				// Parse the command line arguments
				bool parseSuccess = parser.ParseArguments(args, options, (verb, verbInstance) =>
				{
					invokedVerbInstance = verbInstance;
				});

				if (!parseSuccess)
				{
					ShowHelp();
					return;
				}

				// Execute the ICommand that the CommandLineParser found
				var command = invokedVerbInstance as ICommand;
				if (command != null)
				{
					if (command.VerboseLogging)
					{
						levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
					}

					var serviceLocator = new ServiceLocator(logger);
					serviceLocator.BuildContainer();

					command.Logger = logger;
					command.Run(serviceLocator);
				}
			}
			catch (Exception e)
	        {
		        logger.Error($"Unhandled error: {e.Message}");
	        }
        }

		private static void ShowHelp()
		{
			string helpText = Help.GetHelpText();
			System.Console.Write(helpText);
		}
	}
}