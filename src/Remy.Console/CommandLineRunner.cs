using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Serilog;

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
			if (args.Length == 0)
				return;

			if (args[0] == "plugins")
			{
				ParsePluginsCommand(args);
				return;
			}

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

		private void ParsePluginsCommand(string[] args)
		{
			if (args.Length == 1 || args[1] == "list")
			{
				// remy plugins list
			}
			else
			{
				if (args[1] == "source")
				{
					// remy plugins source add {url}
					// remy plugins source remove {url}
					if (args[2] == "add")
					{
						
					}
					else if (args[2] == "remove")
					{

					}
				}
				else if (args[1] == "install")
				{
					// remy plugins install {NugetId}
				}
			}
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
	}
}