using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Core.Tasks.Plugins.Powershell;
using Remy.Tests.StubsAndMocks.Core.Tasks.Plugins.Powershell;
using Serilog;
using Serilog.Core;

namespace Remy.Tests.Unit.Core.Tasks.Plugins.Powershell
{
    [TestFixture]
	public class PowershellTaskTests
    {
		private PowershellRunnerMock _powershellMock;
		private StringBuilder _logStringBuilder;
		private Logger _logger;

		[SetUp]
		public void Setup()
		{
			_powershellMock = new PowershellRunnerMock();

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
        public void should_have_yaml_name()
        {
            Assert.That(new PowershellTask(_powershellMock).YamlName, Is.EqualTo("powershell"));
        }

        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
            var task = new PowershellTask(_powershellMock);
            var config = new PowershellTaskConfig();
			var commands = new List<object>(){ "ls", "echo hello" };

			var properties = new Dictionary<object, object>();
	        properties["commands"] = commands;

			// when
			task.SetConfiguration(config, properties);

            // then
            var actualconfig = task.Config as PowershellTaskConfig;
            Assert.That(actualconfig, Is.Not.Null);

			Assert.That(actualconfig.Commands.Count, Is.EqualTo(2));
			Assert.That(actualconfig.Commands[0], Is.EqualTo("ls"));
			Assert.That(actualconfig.Commands[1], Is.EqualTo("echo hello"));
		}

		[Test]
		public void Run_should_set_powershell_runner_commands()
		{
			// given
			var task = new PowershellTask(_powershellMock);
			var config = new PowershellTaskConfig();
			var commands = new List<object>() { "ls", "echo hello" };

			var properties = new Dictionary<object, object>();
			properties["commands"] = commands;
			task.SetConfiguration(config, properties);

			// when
			task.Run(_logger);

			// then
			Assert.That(_powershellMock.Commands.Length, Is.EqualTo(2));
			Assert.That(_powershellMock.Commands[0], Is.EqualTo(commands[0]));
			Assert.That(_powershellMock.Commands[1], Is.EqualTo(commands[1]));
		}
	}
}