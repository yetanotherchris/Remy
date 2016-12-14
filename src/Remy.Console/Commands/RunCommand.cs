using System;
using System.IO;
using Autofac;
using CommandLine;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Serilog;

namespace Remy.Console.Commands
{
	public class RunCommand : ICommand
	{
		[Option('c', "configfile", Required = false)]
		public string ConfigFile { get; set; }

		[Option('v', "verbose", Required = false)]
		public bool VerboseLogging { get; set; }

		public ILogger Logger { get; set; }
		internal string ConfigBaseDirectory { get; set; }

		public RunCommand()
		{
			ConfigBaseDirectory = Directory.GetCurrentDirectory();
		}

		public void Run(IServiceLocator serviceLocator)
		{
			var yamlParser = serviceLocator.Container.Resolve<IYamlConfigParser>();

			Uri uri = ParseConfigPath("", Logger);
			if (uri != null)
			{
				var tasks = yamlParser.Parse(uri);
				foreach (ITask task in tasks)
				{
					task.Run(Logger);
				}
			}
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