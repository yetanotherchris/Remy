using System.Collections.Generic;
using Remy.Core.Config;
using Remy.Core.Tasks.Runners;
using Serilog;

namespace Remy.Core.Tasks.Plugins
{
    public class InstallChocolateyTask : ITask
    {
        public ITaskConfig Config { get; private set; }
        public string YamlName => "install-chocolatey";

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            Config = config;
        }

        public void Run(ILogger logger)
        {
			var runner = new PowershellRunner(logger);
	        runner.RunCommands(new string[] { "iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex" });
        }
    }
}
