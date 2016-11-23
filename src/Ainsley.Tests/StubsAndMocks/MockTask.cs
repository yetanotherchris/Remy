using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks;
using Serilog;

namespace Ainsley.Tests.StubsAndMocks
{
    public class MockTask : ITask
    {
        private MockTaskConfig _config;
        public ITaskConfig Config => _config;
        public string YamlName => "mock-task";

	    public bool HasRun { get; set; }

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new MockTaskConfig();
            _config.Description = config.Description;
            _config.Runner = config.Runner;
            _config.CustomProperty = properties["customProperty"].ToString();
            _config.Config = config.Config;
        }

        public void Run(ILogger logger)
        {
			HasRun = true;
        }
    }
}