using System.Collections.Generic;
using System.Linq;
using NuGet;
using Remy.Core.Tasks;

namespace Remy.Tests.StubsAndMocks
{
	public class PackageRepositoryMock : IPackageRepository
	{
		public List<IPackage> Packages { get; set; }

		public string Source { get; }
		public PackageSaveModes PackageSaveMode { get; set; }
		public bool SupportsPrereleasePackages { get; }

		public PackageRepositoryMock()
		{
			Packages = new List<IPackage>();
		}

		public void AddPackage(string id, string tags = null, bool isLatestVersion = true)
		{
			Packages.Add(new DataServicePackage()
			{
				Id = id,
				Tags = tags ?? PluginManager.TAGNAME,
				IsLatestVersion = isLatestVersion
			});
		}

		public IQueryable<IPackage> GetPackages()
		{
			return Packages.AsQueryable();
		}

		public void AddPackage(IPackage package)
		{
			Packages.Add(package);
		}

		public void RemovePackage(IPackage package)
		{
			Packages.Remove(package);
		}
	}
}