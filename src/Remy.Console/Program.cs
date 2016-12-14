using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NuGet;
using Remy.Console.Runners;
using Remy.Core.Config;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Serilog;
using Serilog.Core;
using ILogger = Serilog.ILogger;

namespace Remy.Console
{
	//
	// TODO: readme for usage
	// TODO: readme for plugin authoring
	// TODO: refactor program.cs to use command pattern and https://github.com/fschwiet/ManyConsole
	//
	public class Program
    {
        public static void Main(string[] args)
        {
			var levelSwitch = new LoggingLevelSwitch();
	        levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;

			var logger = new LoggerConfiguration()
								.MinimumLevel.ControlledBy(levelSwitch)
                                .WriteTo
                                .LiterateConsole()
                                .CreateLogger();

	        try
			{
				if (args.Length >= 1)
				{
					string allArgs = string.Join(" ", args);

					if (allArgs.Contains("-v") || allArgs.Contains("--verbose"))
					{
						levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
					}

					if (args[0] == "-h" || args[0] == "--help" || args[0] == "help" || args[0] == "/?")
					{
						RunHelpCommand();
						return;
					}
					else if (args[0] == "init")
					{
						RunInitCommand(logger);
						return;
					}
					else if (args[0] == "plugins")
					{
						RunPluginsCommand(args, logger);
						return;
					}
				}

				RunConfigCommand(args, logger);
				return;
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

		private static void RunConfigCommand(string[] args, ILogger logger)
		{
			// Temporary fix until we have Commands
			string allArgs = "";
			if (args.Length > 0)
			{
				allArgs = string.Join(" ", args);
			}

			// Parse "remy.exe -c <file>" and defaults
			Dictionary<string, ITask> registeredTasks = TypeManager.GetRegisteredTaskInstances(logger);
			var configReader = new ConfigFileReader();
			var parser = new YamlConfigParser(configReader, registeredTasks, logger);

			var defaultArgsParser = new DefaultRunner(logger, parser);

			if (allArgs.Contains("-c") || allArgs.Contains("--config"))
			{
				defaultArgsParser.Run(args);
			}
			else
			{
				defaultArgsParser.RunWithNoArgs();
			}
		}

		private static void RunPluginsCommand(string[] args, ILogger logger)
		{
			// Parse "remy.exe plugins <command>"
			var argsParser = new PluginRunner(logger);
			string repositoryUrl = argsParser.GetNugetSource(args);

			PluginManager pluginManager = CreatePluginManager(logger, repositoryUrl);
			argsParser.Run(pluginManager, args);
		}

		private static void RunInitCommand(ILogger logger)
		{
			var stringbuilder = new StringBuilder();
			stringbuilder.AppendLine("name: \"Example file\"");
			stringbuilder.AppendLine("tasks:");
			stringbuilder.AppendLine("  -");
			stringbuilder.AppendLine("    description: \"Says hello in powershell\"");
			stringbuilder.AppendLine("    runner: powershell");
			stringbuilder.AppendLine("    commands:");
			stringbuilder.AppendLine("      - echo 'Hello from Remy!'");


			string path = Path.Combine(Directory.GetCurrentDirectory(), "remy.yml");
			File.WriteAllText(path, stringbuilder.ToString());
			logger.Information("Example remy.yml file written. Now try running remy.exe again.");
		}

		private static PluginManager CreatePluginManager(ILogger logger, string repositoryUrl)
		{
			string pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");

			IPackageRepository packageRepository = PackageRepositoryFactory.Default.CreateRepository(repositoryUrl);
			var packageManager = new PackageManager(packageRepository, pluginsPath);

			var pluginManager = new PluginManager(packageRepository, packageManager, logger);
			pluginManager.EnsurePluginDirectoryExists(pluginsPath);

			return pluginManager;
		}
	}
}