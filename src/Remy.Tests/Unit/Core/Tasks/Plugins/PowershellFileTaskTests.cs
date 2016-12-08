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
	public class PowershellFileTaskTests
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
            Assert.That(new PowershellFileTask(_powershellMock).YamlName, Is.EqualTo("powershell-file"));
        }

        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
            var task = new PowershellFileTask(_powershellMock);
            var config = new PowershellFileTaskConfig();

            var properties = new Dictionary<object, object>();
	        properties["uri"] = "http://www.example.com/powershell.ps1";

            // when
            task.SetConfiguration(config, properties);

            // then
            var actualconfig = task.Config as PowershellFileTaskConfig;
            Assert.That(actualconfig, Is.Not.Null);

            Assert.That(actualconfig.Uri, Is.EqualTo("http://www.example.com/powershell.ps1"));
        }

		[Test]
		public void Run_should_parse_localfile()
		{
			// given
			var task = new PowershellFileTask(_powershellMock);
			task.DownloadFunc = (url) => "echo hello-world";

			var config = new PowershellFileTaskConfig();

			var properties = new Dictionary<object, object>();
			properties["uri"] = "./powershell.ps1";
			task.SetConfiguration(config, properties);

			// when
			task.Run(_logger);

			// then
			Assert.That(_powershellMock.TempFilename, Is.EqualTo("powershell.ps1"));
		}

		[Test]
		public void Run_should_parse_remotefile()
		{
			// given
			var task = new PowershellFileTask(_powershellMock);
			task.DownloadFunc = (url) => "echo hello-world";

			var config = new PowershellFileTaskConfig();

			var properties = new Dictionary<object, object>();
			properties["uri"] = "http://www.example.com/powershell.ps1";
			task.SetConfiguration(config, properties);

			// when
			task.Run(_logger);

			// then
			Assert.That(_logStringBuilder.ToString(), Does.Contain("hello-world"));
		}
	}
}