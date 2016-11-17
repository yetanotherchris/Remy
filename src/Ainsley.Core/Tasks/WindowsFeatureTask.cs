using System.Collections.Generic;
using Ainsley.Core.Config;

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

        }
    }
}