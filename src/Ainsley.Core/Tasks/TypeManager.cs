using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace Ainsley.Core.Tasks
{
    public class TypeManager
    {
        public static Dictionary<string, ITask> GetRegisteredTaskInstances(ILogger logger)
        {
            Type type = typeof(ITask);
            IEnumerable<Type> taskTypes = type.Assembly
                .GetTypes()
                .Where(x => type.IsAssignableFrom(x) && x.IsClass);

            var registeredTasks = new Dictionary<string, ITask>();
            foreach (Type taskType in taskTypes)
            {
                var instance = Activator.CreateInstance(taskType);

                ITask taskInstance = instance as ITask;
                registeredTasks.Add(taskInstance.YamlName, taskInstance);

                logger.Information($"Found '{taskType.Name}' for '{taskInstance.YamlName}'");
            }

            return registeredTasks;
        }
    }
}