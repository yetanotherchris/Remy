using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Console;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks;
using Serilog;
using Serilog.Core;

namespace Remy.Tests.Integration
{
    [TestFixture]
    public class CommandLineRunnerTests
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
        public void should_run_tasks_using_ainsley_yml_as_fallback()
        {
            // given
			var mockTask = new MockTask();
			var yamlConfigReader = new YamlConfigParserMock();
			yamlConfigReader.ExpectedTasks = new List<ITask>()
			{
				mockTask
			};

			var runner = new CommandLineRunner(_logger, yamlConfigReader);

			// when
			runner.Run(new string[0]);

			// then
			Assert.That(mockTask.HasRun, Is.True);
        }

		[Test]
		public void should_run_tasks_for_alternative_config()
		{
			// given
			var mockTask = new MockTask();
			var yamlConfigReader = new YamlConfigParserMock();
			yamlConfigReader.ExpectedTasks = new List<ITask>()
			{
				mockTask
			};

			var runner = new CommandLineRunner(_logger, yamlConfigReader);

			// when
			runner.Run(new string[] {"-c", "test-config.yml" });

			// then
			Assert.That(mockTask.HasRun, Is.True);
		}

		[Test]
		public void should_log_file_not_format()
		{
			// given
			var mockTask = new MockTask();
			var yamlConfigReader = new YamlConfigParserMock();
			yamlConfigReader.ExpectedTasks = new List<ITask>()
			{
				mockTask
			};

			var runner = new CommandLineRunner(_logger, yamlConfigReader);

			// when
			runner.Run(new string[] { "-c", "doesnt-exist.yml" });

			// then
			Assert.That(mockTask.HasRun, Is.False);
			Assert.That(_logStringBuilder.ToString(), Does.Contain("The Yaml config file"));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("doesnt-exist.yml' does not exist"));
		}
	}
}