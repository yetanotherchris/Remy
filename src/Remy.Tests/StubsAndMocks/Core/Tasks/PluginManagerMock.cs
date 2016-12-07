using System.Collections.Generic;
using NuGet;
using Remy.Core.Tasks;

namespace Remy.Tests.StubsAndMocks.Core.Tasks
{
	public class PluginManagerMock : IPluginManager
	{
		public List<IPackage> Packages { get; set; }
		public bool Downloaded { get; set; }

		public PluginManagerMock()
		{
			Packages = new List<IPackage>();
		}

		public void DownloadAndUnzip(string packageId)
		{
			Downloaded = true;
		}

		public void EnsurePluginDirectoryExists(string path)
		{
		}

		public IEnumerable<IPackage> List()
		{
			return Packages;
		}
	}
}