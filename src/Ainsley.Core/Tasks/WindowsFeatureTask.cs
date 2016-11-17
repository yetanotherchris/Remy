using System.Collections.Generic;
using Ainsley.Core.Config;

namespace Ainsley.Core.Tasks
{
	public class WindowsFeatureTask : ITask
	{
		private WindowsFeatureTaskConfig _config;

		public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
		{
			_config = new WindowsFeatureTaskConfig();
			_config.Description = config.Description;
			_config.Runner = config.Runner;

			if (properties.ContainsKey("includeAllSubFeatures"))
				_config.IncludeAllSubFeatures = bool.Parse(properties["includeAllSubFeatures"].ToString());
		}

		public void Run()
		{

		}
	}
}