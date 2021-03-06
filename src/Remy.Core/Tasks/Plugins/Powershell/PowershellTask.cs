﻿using System.Collections.Generic;
using Remy.Core.Config;
using Serilog;

namespace Remy.Core.Tasks.Plugins.Powershell
{
    public class PowershellTask : ITask
    {
        private PowershellTaskConfig _config;
	    private readonly IPowershellRunner _powershellRunner;

	    public ITaskConfig Config => _config;
        public string YamlName => "powershell";

		public PowershellTask(IPowershellRunner powershellRunner)
		{
			_powershellRunner = powershellRunner;
		}

		public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new PowershellTaskConfig();
            _config.Description = config.Description;

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

			_powershellRunner.RunCommands(_config.Commands.ToArray());
        }
    }
}
