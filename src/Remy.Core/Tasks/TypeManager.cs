using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac;
using Remy.Core.Tasks.Runners;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Remy.Core.Tasks
{
    public class TypeManager
    {
        public static Dictionary<string, ITask> GetRegisteredTaskInstances(ILogger logger)
        {
			List<Assembly> assemblies = new List<Assembly>();
	        assemblies.Add(typeof(ITask).Assembly);

			string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
	        if (Directory.Exists(pluginDirectory))
	        {
		        IEnumerable<string> assemblyPaths = Directory.EnumerateFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories);

				// .Select(Assembly.LoadFrom);
		        foreach (string path in assemblyPaths)
		        {
			        var fullPath = new FileInfo(path);
			        if (fullPath.Directory.Name == "net461")
			        {
						assemblies.Add(Assembly.LoadFrom(path));
					}
				}
	        }

			Type type = typeof(ITask);
			var builder = new ContainerBuilder();

			// Plugins
	        builder.RegisterAssemblyTypes(assemblies.ToArray())
		        .Where(t => type.IsAssignableFrom(t) && t.IsClass)
		        .As<ITask>();

			// Logging
	        builder.Register(c => logger).As<ILogger>();

			// Powershell
	        builder.RegisterType<PowershellRunner>().As<IPowershellRunner>();

	        builder.RegisterAssemblyTypes();		
			var container = builder.Build();

			var registeredTasks = new Dictionary<string, ITask>();
			var allTaskInstances = container.Resolve<IEnumerable<ITask>>();
			foreach (ITask task in allTaskInstances)
			{
				registeredTasks.Add(task.YamlName, task);

				logger.Information($"Registered '{task.GetType().Name}' for '{task.YamlName}'");
			}

	        return registeredTasks;
        }
    }
}