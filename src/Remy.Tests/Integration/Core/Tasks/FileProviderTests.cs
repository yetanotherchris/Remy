using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remy.Core.Tasks.Plugins;
using Remy.Core.Tasks.Plugins.Powershell;
using Serilog;

namespace Remy.Tests.Integration.Core.Tasks
{
    [TestFixture]
	public class FileProviderTests
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
				.CreateLogger();
		}

		[Test]
		public void Download_should_get_remote_content()
		{
			// given
			var provider = new FileProvider(_logger);
			Uri uri = new Uri("http://www.example.com");
			
			// when
			string content = provider.Download(uri);

			// then
			Assert.That(content, Does.Match(@"Example Domain"));
		}

		[Test]
		public void GetDefaultDirectory_should_return_current_directory()
		{
			// given
			var provider = new FileProvider(_logger);
			string expectedDirectory = Directory.GetCurrentDirectory();

			// when
			string actualDirectory = provider.GetCurrentDirectory();

			// then
			Assert.That(actualDirectory, Is.EqualTo(expectedDirectory));
		}

		[Test]
		public void GetTemporaryFilePath_should_write_to_disk_with_ps1_extension()
		{
			// given
			var provider = new FileProvider(_logger);
			string tempPath = Path.GetDirectoryName(Path.GetTempFileName());

			// when
			string tempFilePath = provider.WriteTemporaryFile("echo 'a test'");

			// then
			Assert.That(tempFilePath, Does.EndWith(".ps1"));
			Assert.That(tempFilePath, Does.StartWith(tempPath));
			Assert.That(File.ReadAllText(tempFilePath), Is.EqualTo("echo 'a test'"));
		}
	}
}