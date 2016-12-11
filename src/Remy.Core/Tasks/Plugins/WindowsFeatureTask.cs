using System.Collections.Generic;
using Remy.Core.Config;
using Remy.Core.Tasks.Plugins.Powershell;
using Serilog;

namespace Remy.Core.Tasks.Plugins
{
    public class WindowsFeatureTask : ITask
    {
        private WindowsFeatureTaskConfig _config;
	    private IPowershellRunner _powershellRunner;

	    public ITaskConfig Config => _config;
        public string YamlName => "windows-feature";

		public WindowsFeatureTask(IPowershellRunner powershellRunner)
		{
			_powershellRunner = powershellRunner;
		}

		public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new WindowsFeatureTaskConfig();
            _config.Description = config.Description;

            if (properties.ContainsKey("includeAllSubFeatures"))
                _config.IncludeAllSubFeatures = bool.Parse(properties["includeAllSubFeatures"].ToString());

            _config.Features = new List<string>();

            if (properties.ContainsKey("features") && properties["features"] != null)
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

        public void Run(ILogger logger)
        {
            var commands = new List<string>();
            string includeAllSubfeature = (_config.IncludeAllSubFeatures) ? " -IncludeAllSubFeature" : "";
            foreach (string feature in _config.Features)
            {
                string command = $"Install-WindowsFeature {feature}{includeAllSubfeature}";
                commands.Add(command);
            }

			_powershellRunner.RunCommands(commands.ToArray());
        }
    }
}