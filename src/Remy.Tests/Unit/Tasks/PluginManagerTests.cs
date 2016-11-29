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
		[Test]
		public void should()
		{
			// given
			string repositoryUrl = "https://packages.nuget.org/api/v2";
			IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(repositoryUrl);

			var repository = new PackageRepositoryMock();
			var pluginInstaller = new PluginManager(repo);

			// when
			IEnumerable<IPackage> list = pluginInstaller.List();

			// then
			list.ToList().ForEach(p => System.Console.WriteLine(p));
		}
	}
}