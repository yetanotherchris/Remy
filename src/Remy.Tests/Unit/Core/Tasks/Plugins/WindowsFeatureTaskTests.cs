using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Core.Tasks.Plugins;
using Remy.Tests.StubsAndMocks.Core.Tasks.Runners;
using Serilog;
using Serilog.Core;

namespace Remy.Tests.Unit.Core.Tasks.Plugins
{
    [TestFixture]
	public class WindowsFeatureTaskTests
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
            Assert.That(new WindowsFeatureTask(_powershellMock).YamlName, Is.EqualTo("windows-feature"));
        }

        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
            var task = new WindowsFeatureTask(_powershellMock);
            var config = new WindowsFeatureTaskConfig();

            var properties = new Dictionary<object, object>();
            properties["includeAllSubFeatures"] = true;
            properties["features"] = new List<object>()
            {
                "NET-Framework-Core",
                "Web-Server"
            };

            // when
            task.SetConfiguration(config, properties);

            // then
            var actualconfig = task.Config as WindowsFeatureTaskConfig;
            Assert.That(actualconfig, Is.Not.Null);

            Assert.That(actualconfig.IncludeAllSubFeatures, Is.True);
            Assert.That(actualconfig.Features.Count, Is.EqualTo(2));
            Assert.That(actualconfig.Features[0], Is.EqualTo("NET-Framework-Core"));
            Assert.That(actualconfig.Features[1], Is.EqualTo("Web-Server"));
        }

		[Test]
		public void Run_should_set_powershell_runner_commands()
		{
			// given
			var task = new WindowsFeatureTask(_powershellMock);
			var config = new WindowsFeatureTaskConfig();

			var properties = new Dictionary<object, object>();
			properties["includeAllSubFeatures"] = true;
			properties["features"] = new List<object>()
			{
				"NET-Framework-Core",
				"Web-Server"
			};
			task.SetConfiguration(config, properties);

			// when
			task.Run(_logger);

			// then
			Assert.That(_powershellMock.Commands.Length, Is.EqualTo(2));
			Assert.That(_powershellMock.Commands[0], Is.EqualTo("Install-WindowsFeature NET-Framework-Core -IncludeAllSubFeature"));
			Assert.That(_powershellMock.Commands[1], Is.EqualTo("Install-WindowsFeature Web-Server -IncludeAllSubFeature"));
		}
	}
}