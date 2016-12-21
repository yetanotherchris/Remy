using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using NuGet;
using Remy.Core.Tasks;
using ILogger = Serilog.ILogger;

namespace Remy.Console.Commands
{
	public class PluginsCommand : ICommand
	{
		private IPluginManager _pluginManager;

		[Option('v', "verbose", Required = false)]
		public bool VerboseLogging { get; set; }

		[Option("list", Required = false)]
		public bool List { get; set; }

		[Option("install", Required = false)]
		public string Install { get; set; }

		[Option("source", Required = false)]
		public string Source { get; set; }

		public ILogger Logger { get; set; }

		public PluginsCommand()
		{
		}

		internal PluginsCommand(IPluginManager pluginManager)
		{
			_pluginManager = pluginManager;
		}

		public void Run(IServiceLocator serviceLocator)
		{
			if (string.IsNullOrEmpty(Source))
			{
				Source = "https://packages.nuget.org/api/v2";
			}

			Logger.Information($"Using nuget repository '{Source}'");

			if (_pluginManager == null)
				_pluginManager = CreatePluginManager(Logger, Source);

			if (List)
			{
				//
				// remy.exe plugins --list
				//
				Logger.Information("Plugins available (tagged 'remy-plugin'):");

				IEnumerable<IPackage> plugins = _pluginManager.List();
				foreach (IPackage package in plugins)
				{
					string packageName = $"{package.Id}";

					if (package.Version != null)
						packageName += " " + package.Version;

					Logger.Information($"{packageName} - {package.Description}");
				}
			}
			else if (!string.IsNullOrEmpty(Install))
			{
				//
				// remy.exe plugins --install=MyId
				//
				Logger.Information($"Downloading plugin '{Install}'");
				_pluginManager.DownloadAndUnzip(Install);
			}
		}

		private PluginManager CreatePluginManager(ILogger logger, string repositoryUrl)
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