using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using Remy.Core.Config;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks.Plugins;
using Remy.Core.Tasks.Plugins.Powershell;
using Serilog.Core;
using ILogger = Serilog.ILogger;

namespace Remy.Core.Tasks
{
    public class ServiceLocator : IServiceLocator
	{
	    private readonly ILogger _logger;
	    private readonly ContainerBuilder _builder;
	    public IContainer Container { get; set; }

	    public ServiceLocator(ILogger logger)
	    {
		    _logger = logger;
		    _builder = new ContainerBuilder();
		}

	    public void BuildContainer()
	    {
			Type type = typeof(ITask);
			List<Assembly> assemblies = new List<Assembly>();
			assemblies.Add(typeof(ITask).Assembly);

			string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
			if (Directory.Exists(pluginDirectory))
			{
				IEnumerable<string> assemblyPaths = Directory.EnumerateFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories);

				foreach (string path in assemblyPaths)
				{
					var fullPath = new FileInfo(path);
					if (fullPath.Directory.Name == "net461")
					{
						assemblies.Add(Assembly.LoadFrom(path));
					}
				}
			}

			// Plugins
			_builder.RegisterAssemblyTypes(assemblies.ToArray())
				.Where(t => type.IsAssignableFrom(t) && t.IsClass)
				.As<ITask>();

			// Logging
			_builder.Register(c => _logger).As<ILogger>();

			// Powershell
			_builder.RegisterType<PowershellRunner>().As<IPowershellRunner>();
			_builder.RegisterType<FileProvider>().As<IFileProvider>();

			// YAML
			_builder.RegisterType<ConfigFileReader>().As<IConfigFileReader>();
			_builder.Register(c =>
			{
				var configFileReader = c.Resolve<IConfigFileReader>();
				var tasks = TasksAsDictionary(c.Resolve<IEnumerable<ITask>>());

				return new YamlConfigParser(configFileReader, tasks, _logger);
			}).As<IYamlConfigParser>();


			_builder.RegisterAssemblyTypes();
		    Container = _builder.Build();
	    }

		public Dictionary<string, ITask> TasksAsDictionary(IEnumerable<ITask> allTaskInstances)
		{
			var registeredTasks = new Dictionary<string, ITask>();
			foreach (ITask task in allTaskInstances)
			{
				registeredTasks.Add(task.YamlName, task);
				_logger.Debug($"Found '{task.GetType().Name}' for '{task.YamlName}'");
			}

			return registeredTasks;
		}
	}
}