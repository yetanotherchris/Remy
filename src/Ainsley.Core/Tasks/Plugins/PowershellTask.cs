using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks.Runners;
using Serilog;

namespace Ainsley.Core.Tasks.Plugins
{
    public class PowershellTask : ITask
    {
        private PowershellTaskConfig _config;

        public ITaskConfig Config => _config;
        public string YamlName => "powershell";

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new PowershellTaskConfig();
            _config.Description = config.Description;
            _config.Runner = config.Runner;

            _config.Commands = new List<string>();

            if (properties.ContainsKey("commands") && properties["commands"] != null)
            {
                var commands = properties["commands"] as List<object>;

                if (commands != null)
                {
                    foreach (object command in commands)
                    {
                        _config.Commands.Add(command.ToString());
                    }
                }
            }
        }

        public void Run(ILogger logger)
        {
            if (_config.Commands.Count == 0)
            {
                logger.Warning($"Skipping task '{_config.Description}' as no commands set for task.");
                return;
            }

            var runner = new PowershellRunner(logger);
            runner.RunCommands(_config.Commands.ToArray());
        }
    }
}
