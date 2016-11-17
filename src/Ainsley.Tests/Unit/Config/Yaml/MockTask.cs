using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks;

namespace Ainsley.Tests.Unit.Config.Yaml
{
    public class MockTask : ITask
    {
        private MockTaskConfig _config;
        public ITaskConfig Config => _config;

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new MockTaskConfig();
            _config.Description = config.Description;
            _config.Runner = config.Runner;
            _config.CustomProperty = properties["customProperty"].ToString();
            _config.Config = config.Config;
        }

        public void Run()
        {
        }
    }
}