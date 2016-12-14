using System;
using CommandLine;
using Remy.Console.Commands;
using Remy.Core.Tasks;
using Serilog;
using Serilog.Core;

namespace Remy.Console
{
	//
	// TODO: readme for usage
	// TODO: readme for plugin authoring
	//
	public class Program
    {
        public static void Main(string[] args)
        {
			if (args[0] == "-h" || args[0] == "--help" || args[0] == "help" || args[0] == "/?")
			{
				RunHelpCommand();
				return;
			}

			var levelSwitch = new LoggingLevelSwitch();
	        levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;

			var logger = new LoggerConfiguration()
								.MinimumLevel.ControlledBy(levelSwitch)
                                .WriteTo
                                .LiterateConsole()
                                .CreateLogger();

			var iocBuilder = new ServiceLocator(logger);
			iocBuilder.BuildContainer();

			try
			{
				var options = new Options();
				object invokedVerbInstance = null;
				var parser = new Parser(config => config.HelpWriter = null);

				bool ok = parser.ParseArguments(args, options, (verb, subOptions) =>
				{
					invokedVerbInstance = subOptions;
				});

				var command = invokedVerbInstance as ICommand;
				if (command != null)
				{
					if (command.VerboseLogging)
					{
						levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
					}

					command.Logger = logger;
					command.Run(iocBuilder);
				}
			}
			catch (Exception e)
	        {
		        logger.Error($"Unhandled error: {e.Message}");
	        }
        }

		private static void RunHelpCommand()
		{
			string helpText = Help.GetHelpText();
			System.Console.Write(helpText);
		}
	}
}