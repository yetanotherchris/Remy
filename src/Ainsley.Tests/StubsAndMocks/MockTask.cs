using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks;
using Serilog;

namespace Ainsley.Tests.StubsAndMocks
{
	public class MockTask : ITask
	{
		public string YamlName => "mock-task";
		public ITaskConfig Config { get; private set; }
		public bool HasRun { get; set; }

		public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
		{
			Config = config;
		}

		public void Run(ILogger logger)
		{
			HasRun = true;
		}
	}
}