using System.Collections.Generic;
using System.Diagnostics;
using Ainsley.Core.Config;

namespace Ainsley.Core.Tasks
{
    public class InstallChocolateyTask : ITask
    {
        public ITaskConfig Config { get; private set; }
        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            Config = config;
        }

        public void Run()
        {
            var processInfo = new ProcessStartInfo();
        }
    }
}
