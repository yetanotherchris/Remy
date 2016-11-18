using System.Collections.Generic;
using Ainsley.Core.Config;
using Serilog;

namespace Ainsley.Core.Tasks
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
        }
    }
}
