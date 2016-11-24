using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac;
using Serilog;

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
		        var pluginAssemblies = Directory.EnumerateFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories)
												.Select(Assembly.LoadFrom);
				
				assemblies.AddRange(pluginAssemblies);
	        }


			Type type = typeof(ITask);
			var builder = new ContainerBuilder();

	        builder.RegisterAssemblyTypes(assemblies.ToArray())
		        .Where(t => type.IsAssignableFrom(t) && t.IsClass)
		        .As<ITask>();

	        builder.RegisterAssemblyTypes();		
			var container = builder.Build();

			var registeredTasks = new Dictionary<string, ITask>();
			var allTaskInstances = container.Resolve<IEnumerable<ITask>>();
			foreach (ITask task in allTaskInstances)
			{
				registeredTasks.Add(task.YamlName, task);

				logger.Information($"Found '{task.GetType().Name}' for '{task.YamlName}'");
			}

	        return registeredTasks;
        }

		void old()
	    {
			//IEnumerable<Type> taskTypes = type.Assembly
			//	.GetTypes()
			//	.Where(x => type.IsAssignableFrom(x) && x.IsClass);

			//var registeredTasks = new Dictionary<string, ITask>();
			//foreach (Type taskType in taskTypes)
			//{
			//	var instance = Activator.CreateInstance(taskType);

			//	ITask taskInstance = instance as ITask;
			//	registeredTasks.Add(taskInstance.YamlName, taskInstance);

			//	logger.Information($"Found '{taskType.Name}' for '{taskInstance.YamlName}'");
			//}

			//return registeredTasks;
		}
    }
}