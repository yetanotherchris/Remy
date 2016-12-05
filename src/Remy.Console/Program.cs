using System;
using System.Collections.Generic;
using System.IO;
using NuGet;
using Remy.Core.Config;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Remy.Console
{
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
				if (args.Length >= 1 && args[0] == "plugins")
				{
					// Parse "remy.exe plugins <command>"
					var argsParser = new PluginRunner(logger);
					string repositoryUrl = argsParser.GetNugetSource(args);

					PluginManager pluginManager = CreatePluginManager(logger, repositoryUrl);
					argsParser.Run(pluginManager, args);

					return;
				}

				// Parse "remy.exe -c <file>" and defaults
				Dictionary<string, ITask> registeredTasks = TypeManager.GetRegisteredTaskInstances(logger);
				var configReader = new ConfigFileReader();
				var parser = new YamlConfigParser(configReader, registeredTasks, logger);

				var defaultArgsParser = new DefaultRunner(logger, parser);
				defaultArgsParser.Run(args);
	        }
	        catch (Exception e)
	        {
		        logger.Error($"Unhandled error: {e.Message}");
	        }
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