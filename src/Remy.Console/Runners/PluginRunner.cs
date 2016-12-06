using System.Collections.Generic;
using System.Text.RegularExpressions;
using NuGet;
using Remy.Core.Tasks;
using ILogger = Serilog.ILogger;

namespace Remy.Console.Runners
{
	public class PluginRunner
	{
		private readonly ILogger _logger;

		public PluginRunner(ILogger logger)
		{
			_logger = logger;
		}

		public string GetNugetSource(string[] args)
		{
			string repositoryUrl = "https://packages.nuget.org/api/v2";

			string allArgs = string.Join(" ", args);
			if (allArgs.Contains("--source="))
			{
				Regex regex = new Regex("--source=(.+)");
				if (regex.IsMatch(allArgs))
				{
					repositoryUrl = regex.Matches(allArgs)[0].Groups[1].Value;
					_logger.Information($"Set nuget repository to '{repositoryUrl}'");
				}
			}

			return repositoryUrl;
		}

		public void Run(IPluginManager pluginManager, string[] args)
		{
			if (args[1] == "list")
			{
				// remy.exe plugins list
				_logger.Information("Plugins available (tagged 'remy-plugin' on nuget.org):");

				IEnumerable<IPackage> plugins = pluginManager.List();
				foreach (IPackage package in plugins)
				{
					string packageName = $"{package.Id}";

					if (package.Version != null)
						packageName += " " +package.Version;

					_logger.Information($"{packageName} - {package.Description}");
				}
			}
			else if (args[1] == "install" && args.Length > 2)
			{
				// remy.exe plugins install {NugetId}
				string nugetId = args[2];
				_logger.Information($"Downloading plugin '{nugetId}'");

				pluginManager.DownloadAndUnzip(args[2]);
			}
		}
	}
}