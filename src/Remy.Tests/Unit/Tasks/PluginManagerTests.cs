using System.Collections.Generic;
using System.Linq;
using NuGet;
using NUnit.Framework;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks;

namespace Remy.Tests.Unit.Tasks
{
	[TestFixture]
	public class PluginManagerTests
	{
		//string repositoryUrl = "https://packages.nuget.org/api/v2";
		//IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(repositoryUrl);

		[Test]
		public void List_should_only_return_packages_for_remy_plugin_tags()
		{
			// given
			var repository = new PackageRepositoryMock();
			repository.AddPackage("id-1");
			repository.AddPackage("id-2");
			repository.AddPackage("id-3");
			repository.AddPackage("not remy", "other-tag");
			repository.AddPackage("not latest version", null, false);

			var pluginInstaller = new PluginManager(repository);

			// when
			IEnumerable<IPackage> list = pluginInstaller.List();

			// then
			Assert.That(list.Count(), Is.EqualTo(3));
			Assert.That(list.FirstOrDefault().Id, Is.EqualTo("id-1"));
		}

		[Test]
		public void blah()
		{
			// given
			var repository = new PackageRepositoryMock();
			repository.AddPackage("id-1");
			repository.AddPackage("id-2");
			repository.AddPackage("id-3");
			repository.AddPackage("blah-id", "other-tag");
			repository.AddPackage("blah-id", null);


			var pluginInstaller = new PluginManager(repository);

			// when
			IEnumerable<IPackage> list = pluginInstaller.List();

			// then
			Assert.That(list.Count(), Is.EqualTo(3));
			Assert.That(list.FirstOrDefault().Id, Is.EqualTo("id-1"));
		}
	}
}