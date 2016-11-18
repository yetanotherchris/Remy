using System.Collections.Generic;
using Ainsley.Core.Config;
using Serilog;

namespace Ainsley.Core.Tasks
{
    public class WindowsFeatureTask : ITask
    {
        private WindowsFeatureTaskConfig _config;
        public ITaskConfig Config => _config;

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new WindowsFeatureTaskConfig();
            _config.Description = config.Description;
            _config.Runner = config.Runner;

            if (properties.ContainsKey("includeAllSubFeatures"))
                _config.IncludeAllSubFeatures = bool.Parse(properties["includeAllSubFeatures"].ToString());

            _config.Features = new List<string>();

            if (properties["features"] != null)
            {
                var features = properties["features"] as List<object>;

                if (features != null)
                {
                    foreach (object feature in features)
                    {
                        _config.Features.Add(feature.ToString());
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

            var commands = new List<string>();
            string includeAllSubfeature = (_config.IncludeAllSubFeatures) ? " -IncludeAllSubFeature" : "";
            foreach (string feature in _config.Features)
            {
                string command = $"Install-WindowsFeature {feature}{includeAllSubfeature}";
                commands.Add(command);
            }

            runner.RunCommands(commands.ToArray());
        }
    }
}