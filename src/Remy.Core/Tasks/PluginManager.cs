using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Remy.Core.Tasks
{
	public class PluginManager
	{
		public static readonly string TAGNAME = "remy-plugin";

		private readonly IPackageRepository _packageRepository;
		private readonly IPackageManager _packageManager;

		public PluginManager(IPackageRepository packageRepository, IPackageManager packageManager)
		{
			_packageRepository = packageRepository;
			_packageManager = packageManager;
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

		public void DownloadAndUnzip(string packageId)
		{ 
			IEnumerable<IPackage> packages = _packageRepository.FindPackagesById(packageId)
												.Where(p => p.IsLatestVersion);

			foreach (IPackage package in packages)
			{
				_packageManager.InstallPackage(package, true, false);
			}
		}

		public void CopyAssemblies()
		{
			// TODO
		}

		public void EnsurePluginDirectoryExists(string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}
	}
}