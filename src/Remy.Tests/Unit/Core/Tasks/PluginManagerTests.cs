using System.Collections.Generic;
using System.Linq;
using NuGet;
using NUnit.Framework;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Remy.Tests.Unit.Core.Tasks
{
	[TestFixture]
	public class PluginManagerTests
	{
		private ILogger _logger;

		[SetUp]
		public void Setup()
		{
			_logger = new LoggerConfiguration()
				.WriteTo
				.LiterateConsole()
				.CreateLogger();
		}

		[Test]
		public void List_should_only_return_packages_for_remy_plugin_tags()
		{
			// given
			var repository = new PackageRepositoryMock();
			var packageManager = new PackageManagerMock();

			repository.AddPackage("id-1");
			repository.AddPackage("id-2");
			repository.AddPackage("id-3");
			repository.AddPackage("not remy", "other-tag");
			repository.AddPackage("not latest version", null, false);

			var pluginInstaller = new PluginManager(repository, packageManager, _logger);

			// when
			IEnumerable<IPackage> list = pluginInstaller.List();

			// then
			Assert.That(list.Count(), Is.EqualTo(3));
			Assert.That(list.FirstOrDefault().Id, Is.EqualTo("id-1"));
		}

		[Test]
		public void DownloadAndUnzip_should_get_latestid_version_and_install()
		{
			// given
			var repository = new PackageRepositoryMock();
			var packageManager = new PackageManagerMock();
			repository.AddPackage("id-1");
			repository.AddPackage("id-1", null, false);
			repository.AddPackage("id-2");

			var pluginInstaller = new PluginManager(repository, packageManager, _logger);

			// when
			pluginInstaller.DownloadAndUnzip("id-1");

			// then
			Assert.That(packageManager.InstalledIds.Count, Is.EqualTo(1));
			Assert.That(packageManager.InstalledIds, Contains.Item("id-1"));
		}
	}
}