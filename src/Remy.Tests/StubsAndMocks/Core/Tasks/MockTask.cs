using System.Collections.Generic;
using Remy.Core.Config;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks.Core.Config;
using Serilog;

namespace Remy.Tests.StubsAndMocks.Core.Tasks
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
            _config.CustomProperty = properties["customProperty"].ToString();
            _config.Config = config.Config;
        }

        public void Run(ILogger logger)
        {
			HasRun = true;
	        logger.Debug("MockTask run debug log");
	        logger.Information("MockTask run info log");
        }
    }
}