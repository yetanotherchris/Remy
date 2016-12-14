using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Console.Commands;
using Remy.Tests.StubsAndMocks.Core.Tasks;
using Remy.Tests.StubsAndMocks.Core.Tasks.Plugins.Powershell;
using Serilog;

namespace Remy.Tests.Unit.Console.Commands
{
	[TestFixture]
    public class InitCommandTests
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
        public void Run_should_write_example_yaml()
        {
            // given
	        var fileProvider = new FileProviderMock();
	        fileProvider.CurrentDirectory = "mypath/";
	        string expectedPath = Path.Combine(fileProvider.GetCurrentDirectory(), "remy.yml");

	        var locator = new ServiceLocatorMock();
			var command = new InitCommand(fileProvider);
			command.Logger = _logger;

			// when
			command.Run(locator);

			// then
	        Assert.That(fileProvider.FileWriteContent, Is.Not.Null);
			Assert.That(fileProvider.FileWritePath, Is.Not.Null);

			Assert.That(fileProvider.FileWriteContent, Does.Contain("echo 'Hello from Remy!'"));
	        Assert.That(fileProvider.FileWritePath, Is.EqualTo(expectedPath));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("Example remy.yml file written"));
		}
	}
}