using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace Remy.Core.Tasks
{
	public class PluginInstaller
	{
		public void Run()
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testy");

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
			IEnumerable<IPackage> packages = repo.GetPackages().Where(p => p.Tags.Contains("remy"));
			foreach (IPackage package in packages)
			{
				
				PackageManager packageManager = new PackageManager(repo, path);
				packageManager.InstallPackage(package, true, false);
			}
		}
	}
}
