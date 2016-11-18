using System.Collections.Generic;
using Ainsley.Core.Config;

namespace Ainsley.Core.Tasks
{
    public class WindowsFeatureTask : ITask
    {
        public ITaskConfig Config { get; private set; }

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            var taskConfig = new WindowsFeatureTaskConfig();
            taskConfig.Description = config.Description;
            taskConfig.Runner = config.Runner;

            if (properties.ContainsKey("includeAllSubFeatures"))
                taskConfig.IncludeAllSubFeatures = bool.Parse(properties["includeAllSubFeatures"].ToString());

            taskConfig.Features = new List<string>();

            if (properties["features"] != null)
            {
                var features = properties["features"] as List<object>;

                if (features != null)
                {
                    foreach (object feature in features)
                    {
                        taskConfig.Features.Add(feature.ToString());
                    }
                }
            }

            Config = taskConfig;
        }

        public void Run()
        {

        }
    }
}