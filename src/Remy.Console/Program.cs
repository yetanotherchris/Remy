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
using ILogger = Serilog.ILogger;

namespace Remy.Console
{
	//
	// TODO: verbose mode
	// TODO: readme for usage
	// TODO: readme for plugin authoring
	// TODO: refactor program.cs and defaultrunner 
	//
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                                .WriteTo
                                .LiterateConsole()
                                .CreateLogger();

	        try
			{
				if (args.Length == 1)
				{
					if (args[0] == "-h" || args[0] == "--help" || args[0] == "help" || args[0] == "/?")
					{
						System.Console.Write(Help.GetHelpText());
						return;
					}
					else if (args[0] == "init")
					{
						RunInitCommand(logger);
						return;
					}
				}
				else if (args.Length >= 1 && args[0] == "plugins")
				{
					RunPluginsCommand(args, logger);
					return;
				}
				else
				{
					RunMainCommands(args, logger);
					return;
				}
			}
			catch (Exception e)
	        {
		        logger.Error($"Unhandled error: {e.Message}");
	        }
        }

		private static void RunMainCommands(string[] args, Serilog.Core.Logger logger)
		{
			// Parse "remy.exe -c <file>" and defaults
			Dictionary<string, ITask> registeredTasks = TypeManager.GetRegisteredTaskInstances(logger);
			var configReader = new ConfigFileReader();
			var parser = new YamlConfigParser(configReader, registeredTasks, logger);

			var defaultArgsParser = new DefaultRunner(logger, parser);
			defaultArgsParser.Run(args);
		}

		private static void RunPluginsCommand(string[] args, Serilog.Core.Logger logger)
		{
			// Parse "remy.exe plugins <command>"
			var argsParser = new PluginRunner(logger);
			string repositoryUrl = argsParser.GetNugetSource(args);

			PluginManager pluginManager = CreatePluginManager(logger, repositoryUrl);
			argsParser.Run(pluginManager, args);
		}

		private static void RunInitCommand(Serilog.Core.Logger logger)
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