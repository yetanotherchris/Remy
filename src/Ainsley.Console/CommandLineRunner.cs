using System;
using System.Collections.Generic;
using System.IO;
using Ainsley.Core.Config.Yaml;
using Ainsley.Core.Tasks;
using CommandLine;
using Serilog;

namespace Ainsley.Console
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
				fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ainsley.yml");
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