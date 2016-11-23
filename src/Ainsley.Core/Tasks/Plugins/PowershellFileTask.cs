using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks.Runners;
using Serilog;

namespace Ainsley.Core.Tasks.Plugins
{
    public class PowershellFileTask : ITask
    {
        private PowershellFileTaskConfig _config;

        public ITaskConfig Config => _config;
        public string YamlName => "powershell";

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new PowershellFileTaskConfig();
            _config.Description = config.Description;
            _config.Runner = config.Runner;
	        _config.Uri = properties["uri"].ToString();
        }

        public void Run(ILogger logger)
        {
            if (string.IsNullOrEmpty(_config.Uri))
            {
                logger.Warning($"Skipping task '{_config.Description}' as no uri is set for the task.");
                return;
            }

            var runner = new PowershellRunner(logger);
            runner.RunCommands(_config.Commands.ToArray());
        }
    }
}
