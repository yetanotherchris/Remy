using System.IO;
using System.Text;
using NuGet;
using NUnit.Framework;
using Remy.Console.Commands;
using Remy.Tests.StubsAndMocks.Core.Tasks;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Remy.Tests.Unit.Console.Commands
{
	[TestFixture]
	public class PluginCommandTests
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
			var locator = new ServiceLocatorMock();

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

			var command = new PluginsCommand(pluginManager);
			command.Logger = _logger;
			command.List = true;

			// Act
			command.Run(locator);

			// Assert
			Assert.That(_logStringBuilder.ToString(), Does.Contain("Plugins available (tagged 'remy-plugin'):"));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("SilverBack - This plugin gives the Silverback framework a banana."));
			Assert.That(_logStringBuilder.ToString(), Does.Contain("BumsOnATrain 1.0.1 - Choo choo."));
		}

		[Test]
		public void Run_should_parse_install_command()
		{
			// Arrange
			var locator = new ServiceLocatorMock();

			var pluginManager = new PluginManagerMock();
			pluginManager.Packages.Add(new DataServicePackage()
			{
				Id = "DonkeyKong",
				Tags = "remy-plugin",
				IsLatestVersion = true
			});

			var command = new PluginsCommand(pluginManager);
			command.Logger = _logger;
			command.Install = "DonkeyKong";

			// Act
			command.Run(locator);

			// Assert
			Assert.That(_logStringBuilder.ToString(), Does.Contain("Downloading plugin 'DonkeyKong'"));
			Assert.True(pluginManager.Downloaded);
		}

		[Test]
		public void GetNugetSource_should_default_to_nuget_repo()
		{
			// Arrange
			var locator = new ServiceLocatorMock();
			var pluginManager = new PluginManagerMock();

			var command = new PluginsCommand(pluginManager);
			command.Logger = _logger;
			command.Install = "ThePackage";

			// Act
			command.Run(locator);

			// Assert
			Assert.That(command.Source, Is.EqualTo("https://packages.nuget.org/api/v2"));
		}
	}
}