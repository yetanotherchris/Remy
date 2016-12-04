using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Core.Tasks.Plugins;
using Serilog;

namespace Remy.Tests.Unit.Core.Tasks.Plugins
{
    [TestFixture]
	public class PowershellFileTaskTests
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
        public void should_have_yaml_name()
        {
            Assert.That(new PowershellFileTask().YamlName, Is.EqualTo("powershell-file"));
        }

        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
            var task = new PowershellFileTask();
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

		// integration test

		[Test]
		public void Run_downloads_and_runs_remote_powershell_file()
		{
			// given
			var task = new PowershellFileTask();
			var config = new PowershellFileTaskConfig();

			var properties = new Dictionary<object, object>();
			properties["uri"] = "https://gist.githubusercontent.com/yetanotherchris/6825f9e1ba26e9ed4875e1646f143ed3/raw/7c918aa4369a00c87d9ddf086d3286d375e4c457/hello-world.ps1";
			task.SetConfiguration(config, properties);

			// when
			task.Run(_logger);

			// then
			Assert.That(_logStringBuilder.ToString(), Does.Match(@"Running powershell.exe \-File .*.tmp\.ps1"));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("hello world"));
		}
	}
}