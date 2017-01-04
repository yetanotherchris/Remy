using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Remy.Core.Config;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks.Plugins;
using Remy.Core.Tasks.Plugins.Powershell;
using Serilog.Core;
using StructureMap;
using StructureMap.Graph;
using ILogger = Serilog.ILogger;

namespace Remy.Core.Tasks
{
	public class ServiceLocator : IServiceLocator
	{
		private readonly ILogger _logger;
		public IContainer Container { get; set; }

		public ServiceLocator(ILogger logger)
		{
			_logger = logger;
		}

		public void BuildContainer()
		{
			Container = new Container(c =>
			{
				c.AddRegistry(new PluginsRegistry(_logger));
			});
		}

		public Dictionary<string, ITask> TasksAsDictionary(IEnumerable<ITask> allTaskInstances)
		{
			return Container.GetInstance<Dictionary<string, ITask>>();
		}
	}

	public class PluginsRegistry : Registry
	{
		private readonly ILogger _logger;

		public PluginsRegistry(ILogger logger)
		{
			_logger = logger;
			Scan(ScanTypes);
		}

		private void ScanTypes(IAssemblyScanner scanner)
		{
			scanner.AssembliesFromApplicationBaseDirectory();
			scanner.AddAllTypesOf<ITask>();

			// Logging
			For<ILogger>().Use(x => _logger);

			// Powershell
			For<IPowershellRunner>().Use<PowershellRunner>();
			For<IFileProvider>().Use<FileProvider>();

			// YAML
			For<IConfigFileReader>().Use<ConfigFileReader>();
			For<Dictionary<string, ITask>>().Use("TasksAsDictionary", context =>
			{
				return TasksAsDictionary(context.GetAllInstances<ITask>());
			});
			For<IYamlConfigParser>().Use("IYamlConfigParser", context =>
			{
				var configFileReader = context.GetInstance<IConfigFileReader>();
				var tasks = context.GetInstance<Dictionary<string, ITask>>();

				return new YamlConfigParser(configFileReader, tasks, _logger);
			});
		}

		public Dictionary<string, ITask> TasksAsDictionary(IEnumerable<ITask> allTaskInstances)
		{
			var registeredTasks = new Dictionary<string, ITask>();
			foreach (ITask task in allTaskInstances)
			{
				registeredTasks.Add(task.YamlName, task);

				Type taskType = task.GetType();
				string assemblyVersion = taskType.Name +","+ taskType.Assembly.GetName().Name + " "+ taskType.Assembly.GetName().Version;
				_logger.Debug($"  - Registered '{task.YamlName}' ({taskType.Name}, {assemblyVersion})");
			}

			return registeredTasks;
		}
	}
}