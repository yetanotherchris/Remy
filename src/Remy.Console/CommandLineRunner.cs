using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using NuGet;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using ILogger = Serilog.ILogger;

namespace Remy.Console
{
	public class CommandLineRunner
	{
		private readonly ILogger _logger;
		private readonly IYamlConfigParser _yamlParser;

		public CommandLineRunner(ILogger logger, IYamlConfigParser yamlParser)
		{
			_logger = logger;
			_yamlParser = yamlParser;
		}

		public void Run(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(options =>
				{
					Uri uri = ParseConfigPath(options.ConfigFile, _logger);
					if (uri != null)
					{
						var tasks = _yamlParser.Parse(uri);
						foreach (ITask task in tasks)
						{
							task.Run(_logger);
						}
					}

					System.Console.WriteLine("");
				});
		}

		private static Uri ParseConfigPath(string configPath, ILogger logger)
		{
			string fullPath = configPath;
			if (string.IsNullOrEmpty(fullPath))
			{
				fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "remy.yml");
			}

			try
			{
				if (!fullPath.StartsWith("http"))
				{
					if (!fullPath.StartsWith("/") && !fullPath.StartsWith("./"))
					{
						fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullPath);
					}

					fullPath = "file://" + fullPath;
				}

				Uri uri = new Uri(fullPath);
				if (uri.IsAbsoluteUri && !File.Exists(uri.LocalPath))
				{
					logger.Error($"The Yaml config file '{uri.LocalPath}' does not exist");
					return null;
				}

				return uri;
			}
			catch (FormatException)
			{
				logger.Error($"The Yaml config file '{configPath}' is not a valid path or url.");
				return null;
			}
		}

		public static void ParsePluginsCommandLine(ILogger logger, string[] args)
		{
			string pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
			string repositoryUrl = "https://packages.nuget.org/api/v2";

			IPackageRepository packageRepository = PackageRepositoryFactory.Default.CreateRepository(repositoryUrl);
			var packageManager = new PackageManager(packageRepository, pluginsPath);

			var pluginManager = new PluginManager(packageRepository, packageManager, logger);
			pluginManager.EnsurePluginDirectoryExists(pluginsPath);

			if (args.Length == 1 || args[1] == "list")
			{
				// remy.exe plugins list
				logger.Information("Plugins available (tagged 'remy-plugin' on nuget.org):");

				IEnumerable<IPackage> plugins = pluginManager.List();
				foreach (IPackage package in plugins)
				{
					logger.Information($"{package.Id} - {package.Description}");
				}
			}
			else if (args[1] == "install" && args.Length > 2)
			{
				// remy.exe plugins install {NugetId}
				string nugetId = args[2];
				logger.Information($"Downloading plugin '{nugetId}'");

				pluginManager.DownloadAndUnzip(args[2]);
			}
		}
	}
}