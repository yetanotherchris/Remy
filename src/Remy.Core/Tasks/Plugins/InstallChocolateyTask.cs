using System.Collections.Generic;
using Remy.Core.Config;
using Remy.Core.Tasks.Runners;
using Serilog;

namespace Remy.Core.Tasks.Plugins
{
    public class InstallChocolateyTask : ITask
    {
	    private readonly IPowershellRunner _powershellRunner;
	    public ITaskConfig Config { get; private set; }
        public string YamlName => "install-chocolatey";

		public InstallChocolateyTask(IPowershellRunner powershellRunner)
		{
			_powershellRunner = powershellRunner;
		}

	    public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            Config = config;
        }

        public void Run(ILogger logger)
        {
	        _powershellRunner.RunCommands(new string[] { "iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex" });
        }
    }
}
