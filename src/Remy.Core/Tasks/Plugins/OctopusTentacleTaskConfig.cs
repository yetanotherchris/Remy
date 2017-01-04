using System.Collections.Generic;
using Remy.Core.Config;

namespace Remy.Core.Tasks.Plugins
{
	public class OctopusTentacleTaskConfig : ITaskConfig
	{
		public string Description { get; set; }
		public Dictionary<string, object> Config { get; set; }

		public string OctopusServer { get; set; }
		public string Environment { get; set; }
		public string Role { get; set; }
		public string ApiKey { get; set; }
		public string Thumbprint { get; set; }
		public string TentacleExePath { get; set; }
		public string ConfigPath { get; set; }
		public string CertText { get; set; }
		public string Port { get; set; }
		public string HomeDirectory { get; set; }
		public string AppDirectory { get; set; }
		public string TentacleName { get; set; }

		public OctopusTentacleTaskConfig()
		{
			HomeDirectory = @"C:\Octopus";
			AppDirectory = @"C:\Octopus\Applications";
			ConfigPath = @"C:\Octopus\Tentacle\Tentacle.config";
			TentacleExePath = @"C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe";
			Port = "10933";
		}
	}
}