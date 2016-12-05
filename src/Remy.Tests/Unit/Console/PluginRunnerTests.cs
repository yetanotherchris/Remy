using System.IO;
using System.Text;
using NuGet;
using NUnit.Framework;
using Remy.Console;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Remy.Tests.Unit.Console
{
	[TestFixture]
	public class PluginRunnerTests
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
		public void Run_should_parse_list_command()
		{
			// Arrange
			var parser = new PluginRunner(_logger);
			var pluginManager = new PluginManagerMock();
			pluginManager.Packages.Add(new DataServicePackage()
			{
				Id = "SilverBack",
				Tags = "remy-plugin",
				Description = "This plugin gives the Silverback framework a banana.",
				IsLatestVersion = true
			});
			pluginManager.Packages.Add(new DataServicePackage()
			{
				Id = "BumsOnATrain",
				Tags = "remy-plugin",
				Description = "Choo choo.",
				IsLatestVersion = true,
				Version = "1.0.1"
			});

			string[] args = {"plugins", "list"};

			// Act
			parser.Run(pluginManager, args);

			// Assert
			Assert.That(_logStringBuilder.ToString(), Does.Contain("Plugins available (tagged 'remy-plugin' on nuget.org):"));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("SilverBack - This plugin gives the Silverback framework a banana."));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("BumsOnATrain 1.0.1 - Choo choo."));
		}

		[Test]
		public void Run_should_parse_install_command()
		{
			// Arrange
			var parser = new PluginRunner(_logger);
			var pluginManager = new PluginManagerMock();
			pluginManager.Packages.Add(new DataServicePackage()
			{
				Id = "DonkeyKong",
				Tags = "remy-plugin",
				IsLatestVersion = true
			});

			string[] args = { "plugins", "install", "DonkeyKong" };

			// Act
			parser.Run(pluginManager, args);

			// Assert
			Assert.That(_logStringBuilder.ToString(), Does.Contain("Downloading plugin 'DonkeyKong'"));
			Assert.True(pluginManager.Downloaded);
		}

		[Test]
		public void GetNugetSource_should_default_to_nuget_repo()
		{
			// Arrange
			var parser = new PluginRunner(_logger);
			var pluginManager = new PluginManagerMock();

			string[] args = { "plugins", "install", "ThePackage" };

			// Act
			string nugetSource = parser.GetNugetSource(args);

			// Assert
			Assert.That(nugetSource, Is.EqualTo("https://packages.nuget.org/api/v2"));
		}

		[Test]
		public void GetNugetSource_should_parse_nuget_source_arg()
		{
			// Arrange
			var parser = new PluginRunner(_logger);
			var pluginManager = new PluginManagerMock();

			string[] args = { "plugins", "install", "ThePackage", "--source=http://www.example.com" };

			// Act
			string nugetSource = parser.GetNugetSource(args);

			// Assert
			Assert.That(nugetSource, Is.EqualTo("http://www.example.com"));
		}
	}
}