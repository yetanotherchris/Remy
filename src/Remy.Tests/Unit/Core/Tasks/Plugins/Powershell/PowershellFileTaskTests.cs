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
	public class PowershellFileTaskTests
    {
		private PowershellRunnerMock _runnerMock;
	    private PowershellFileProviderMock _fileProviderMock;
		private StringBuilder _logStringBuilder;
		private Logger _logger;

	    [SetUp]
		public void Setup()
		{
			_runnerMock = new PowershellRunnerMock();
			_fileProviderMock = new PowershellFileProviderMock();

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
			var task = new PowershellFileTask(_fileProviderMock, _runnerMock);
			Assert.That(task.YamlName, Is.EqualTo("powershell-file"));
		}

        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
			// given
			var task = new PowershellFileTask(_fileProviderMock, _runnerMock);
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
		[TestCase("c:\\blah\\myscript.ps1", "c:\\blah\\myscript.ps1")]
		[TestCase("./myscript.ps1", "c:\\currentdir\\myscript.ps1")]
		[TestCase("c:\\myfolder\\myscript.ps1", "c:\\myfolder\\myscript.ps1")]
		[TestCase("myscript.ps1", "c:\\currentdir\\myscript.ps1")]
		[TestCase("file://c:/temp/foo/myscript.ps1", "c:\\temp\\foo\\myscript.ps1")]
		public void Run_should_parse_localfile_paths(string inputFilePath, string expectedFilePath)
		{
			// given
			_fileProviderMock.CurrentDirectory = "c:\\currentdir";
			var task = new PowershellFileTask(_fileProviderMock, _runnerMock);
			var config = new PowershellFileTaskConfig();

			var properties = new Dictionary<object, object>();
			properties["uri"] = inputFilePath;
			task.SetConfiguration(config, properties);

			// when
			task.Run(_logger);

			// then
			Assert.That(_runnerMock.ActualTempFilename, Is.EqualTo(expectedFilePath));
		}

		[Test]
		public void Run_should_parse_and_run_remotefiles()
		{
			// given
			_fileProviderMock.DownloadContent = "echo hello-world";
			_fileProviderMock.TempFilePath = "expected-tempfilename.ps1";
			var task = new PowershellFileTask(_fileProviderMock, _runnerMock);

			var config = new PowershellFileTaskConfig();

			var properties = new Dictionary<object, object>();
			properties["uri"] = "http://www.example.com/powershell.ps1";
			task.SetConfiguration(config, properties);

			// when
			task.Run(_logger);

			// then
			Assert.That(_runnerMock.ActualTempFilename, Is.EqualTo(_fileProviderMock.TempFilePath));
		}
	}
}