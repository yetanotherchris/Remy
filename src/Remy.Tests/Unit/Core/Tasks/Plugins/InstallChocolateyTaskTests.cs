using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Core.Tasks.Plugins;
using Remy.Tests.StubsAndMocks.Core.Config;
using Remy.Tests.StubsAndMocks.Core.Tasks.Runners;
using Serilog;
using Serilog.Core;

namespace Remy.Tests.Unit.Core.Tasks.Plugins
{
    [TestFixture]
	public class InstallChocolateyTaskTests
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
            Assert.That(new InstallChocolateyTask(_powershellMock).YamlName, Is.EqualTo("install-chocolatey"));
        }

        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
	        var config = new MockTaskConfig();
            var task = new InstallChocolateyTask(_powershellMock);
	        var properties = new Dictionary<object, object>();

            // when
            task.SetConfiguration(config, properties);

            // then
            Assert.That(task.Config, Is.Not.Null);
        }

		[Test]
		public void Run_should_set_powershell_runner_commands()
		{
			// given
			var task = new InstallChocolateyTask(_powershellMock);

			// when
			task.Run(_logger);

			// then
			Assert.That(_powershellMock.Commands.Length, Is.EqualTo(1));
			Assert.That(_powershellMock.Commands[0], Is.EqualTo("iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex"));
		}
	}
}