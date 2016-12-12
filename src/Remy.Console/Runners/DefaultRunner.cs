using System;
using System.IO;
using System.Text;
using CommandLine;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using ILogger = Serilog.ILogger;

namespace Remy.Console.Runners
{
	public class DefaultRunner
	{
		private readonly ILogger _logger;
		private readonly IYamlConfigParser _yamlParser;
		public string ConfigBaseDirectory { get; set; }

		public DefaultRunner(ILogger logger, IYamlConfigParser yamlParser)
		{
			_logger = logger;
			_yamlParser = yamlParser;
			ConfigBaseDirectory = Directory.GetCurrentDirectory();
		}

		public void Run(string[] args)
		{
			var parser = new Parser(config => config.HelpWriter = null);

			parser.ParseArguments<Options>(args)
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

		private Uri ParseConfigPath(string configPath, ILogger logger)
		{
			string fullPath = configPath;
			if (string.IsNullOrEmpty(fullPath))
			{
				fullPath = Path.Combine(ConfigBaseDirectory, "remy.yml");
			}

			try
			{
				if (!fullPath.StartsWith("http"))
				{
					if (!fullPath.StartsWith("/") && !fullPath.StartsWith("./"))
					{
						fullPath = Path.Combine(ConfigBaseDirectory, fullPath);
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