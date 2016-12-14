using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Console.Commands;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks.Core.Config.Yaml;
using Remy.Tests.StubsAndMocks.Core.Tasks;
using Serilog;

namespace Remy.Tests.Unit.Console.Commands
{
    [TestFixture]
    public class RunCommandTests
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

	        var locator = new ServiceLocatorMock();
	        locator.Register<IYamlConfigParser>(yamlConfigReader);
	        locator.Register<ILogger>(_logger);

			var command = new RunCommand();
			command.Logger = _logger;
			command.ConfigBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// when
			command.Run(locator);

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

			var locator = new ServiceLocatorMock();
			locator.Register<IYamlConfigParser>(yamlConfigReader);
			locator.Register<ILogger>(_logger);

			var command = new RunCommand();
			command.Logger = _logger;
			command.ConfigFile = "test-config.yml";
			command.ConfigBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// when
			command.Run(locator);

			// then
			Assert.That(mockTask.HasRun, Is.True);
			Assert.That(_logStringBuilder.ToString(), Does.Contain("MockTask run info log"));
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

			var locator = new ServiceLocatorMock();
			locator.Register<IYamlConfigParser>(yamlConfigReader);
			locator.Register<ILogger>(_logger);

			var command = new RunCommand();
			command.Logger = _logger;
			command.ConfigFile = "doesnt-exist.yml";
			command.ConfigBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// when
			command.Run(locator);

			// then
			Assert.That(mockTask.HasRun, Is.False);
			Assert.That(_logStringBuilder.ToString(), Does.Contain("The Yaml config file"));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("doesnt-exist.yml' does not exist"));
		}
	}
}