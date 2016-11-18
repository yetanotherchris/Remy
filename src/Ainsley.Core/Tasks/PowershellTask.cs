using System.Collections.Generic;
using System.Diagnostics;
using Ainsley.Core.Config;
using Serilog;

namespace Ainsley.Core.Tasks
{
    public class PowershellTask : ITask
    {
        private PowershellTaskConfig _config;
        public ITaskConfig Config => _config;

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new PowershellTaskConfig();
            _config.Description = config.Description;
            _config.Runner = config.Runner;

            _config.Commands = new List<string>();

            if (properties["commands"] != null)
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

        public void Run()
        {
            var logger = new LoggerConfiguration()
                .WriteTo
                .LiterateConsole()
                .CreateLogger();

            var runner = new PowershellRunner(logger);
            runner.RunCommands(_config.Commands.ToArray());
        }
    }
}
