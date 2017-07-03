using System.Collections.Generic;
using Remy.Core.Config;
using Remy.Core.Tasks.Plugins.Powershell;
using Serilog;

namespace Remy.Core.Tasks.Plugins
{
	public class WindowsFeatureTask : ITask
	{
		private WindowsFeatureTaskConfig _config;
		private readonly IPowershellRunner _powershellRunner;

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

			if (properties.ContainsKey("windows10"))
				_config.IsWindows10 = bool.Parse(properties["windows10"].ToString());

			if (properties.ContainsKey("remove"))
				_config.ShouldRemove = bool.Parse(properties["remove"].ToString());

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
			foreach (string feature in _config.Features)
			{
				string command = "";
				string extraArgs = "";

				if (_config.IsWindows10)
				{
					extraArgs = (_config.IncludeAllSubFeatures) ? " -All" : "";

					if (_config.ShouldRemove)
					{
						command = $"Disable-WindowsOptionalFeature -Online{extraArgs} -Featurename {feature}";
					}
					else
					{
						command = $"Enable-WindowsOptionalFeature -Online{extraArgs} -Featurename {feature}";
					}
				}
				else
				{
					extraArgs = (_config.IncludeAllSubFeatures) ? " -IncludeAllSubFeature" : "";
					if (_config.ShouldRemove)
					{
						command = $"Uninstall-WindowsFeature {feature}{extraArgs}";
					}
					else
					{
						command = $"Install-WindowsFeature {feature}{extraArgs}";
					}
				}

				commands.Add(command);
			}

			_powershellRunner.RunCommands(commands.ToArray());
		}
	}
}