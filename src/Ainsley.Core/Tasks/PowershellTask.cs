using System.Collections.Generic;
using System.Diagnostics;
using Ainsley.Core.Config;

namespace Ainsley.Core.Tasks
{
    public class PowershellTask : ITask
    {
        public ITaskConfig Config { get; private set; }
        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            var taskConfig = new PowershellTaskConfig();
            taskConfig.Description = config.Description;
            taskConfig.Runner = config.Runner;
            
            taskConfig.Commands = new List<string>();

            if (properties["commands"] != null)
            {
                var commands = properties["commands"] as List<object>;

                if (commands != null)
                {
                    foreach (object command in commands)
                    {
                        taskConfig.Commands.Add(command.ToString());
                    }
                }
            }

            Config = taskConfig;
        }

        public void Run()
        {
            var processInfo = new ProcessStartInfo();
        }
    }
}
