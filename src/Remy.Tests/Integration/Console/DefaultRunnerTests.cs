using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Console;
using Remy.Console.Runners;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks;
using Remy.Tests.StubsAndMocks.Core.Config.Yaml;
using Remy.Tests.StubsAndMocks.Core.Tasks;
using Serilog;

namespace Remy.Tests.Integration.Console
{
	// While these tests use mocks, they are integration tests are they are testing the 
	// DefaultRunner's config path lookups.
    [TestFixture]
    public class DefaultRunnerTests
	{
		private ILogger _logger;
		private StringBuilder _logStringBuilder;

		[SetUp]
		public void Setup()
		{
			_logStringBuilder = new StringBuilder();
			var logMessages = new StringWriter(_logStringBuilder);

			_logger = new LoggerConfiguration()
				.WriteTo
				.TextWriter(logMessages)
				.WriteTo
				.LiterateConsole()
				.CreateLogger();
		}

		[Test]
        public void Run_should_use_remy_yml_by_default()
        {
            // given
			var mockTask = new MockTask();
			var yamlConfigReader = new YamlConfigParserMock();
			yamlConfigReader.ExpectedTasks = new List<ITask>()
			{
				mockTask
			};

			var runner = new DefaultRunner(_logger, yamlConfigReader);
			runner.ConfigBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// when
			runner.Run(new string[0]);

			// then
			Assert.That(mockTask.HasRun, Is.True);
        }

		[Test]
		public void Run_should_execute_tasks_from_custom_config()
		{
			// given

			var mockTask = new MockTask();
			var yamlConfigReader = new YamlConfigParserMock();
			yamlConfigReader.ExpectedTasks = new List<ITask>()
			{
				mockTask
			};

			var runner = new DefaultRunner(_logger, yamlConfigReader);
			runner.ConfigBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// when
			runner.Run(new string[] {"-c", "test-config.yml" });

			// then
			Assert.That(mockTask.HasRun, Is.True);
		}

		[Test]
		public void Run_should_log_file_not_found()
		{
			// given
			var mockTask = new MockTask();
			var yamlConfigReader = new YamlConfigParserMock();
			yamlConfigReader.ExpectedTasks = new List<ITask>()
			{
				mockTask
			};

			var runner = new DefaultRunner(_logger, yamlConfigReader);
			runner.ConfigBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// when
			runner.Run(new string[] { "-c", "doesnt-exist.yml" });

			// then
			Assert.That(mockTask.HasRun, Is.False);
			Assert.That(_logStringBuilder.ToString(), Does.Contain("The Yaml config file"));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("doesnt-exist.yml' does not exist"));
		}
	}
}