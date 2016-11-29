using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Remy.Core.Tasks
{
	public class PluginManager
	{
		private readonly IPackageRepository _packageRepository;
		public static readonly string TAGNAME = "remy-plugin";

		public PluginManager(IPackageRepository packageRepository)
		{
			_packageRepository = packageRepository;
		}

		public IEnumerable<IPackage> List()
		{
			var list = new List<IPackage>();

			IEnumerable<IPackage> packages = _packageRepository.GetPackages()
												.Where(p => p.IsLatestVersion && p.Tags.Contains(TAGNAME));

			foreach (IPackage package in packages)
			{
				list.Add(package);
			}

			return list;
		}

		public void Install(string packageId)
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			IEnumerable<IPackage> packages = _packageRepository.FindPackagesById(packageId)
												.Where(p => p.IsLatestVersion);

			foreach (IPackage package in packages)
			{
				PackageManager packageManager = new PackageManager(_packageRepository, path);
				packageManager.InstallPackage(package, true, false);
			}
		}
	}
}