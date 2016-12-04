using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;
using ILogger = Serilog.ILogger;

namespace Remy.Core.Tasks
{
	public class PluginManager : IPluginManager
	{
		public static readonly string TAGNAME = "remy-plugin";

		private readonly IPackageRepository _packageRepository;
		private readonly IPackageManager _packageManager;
		private readonly ILogger _logger;

		public PluginManager(IPackageRepository packageRepository, IPackageManager packageManager, ILogger logger)
		{
			_packageRepository = packageRepository;
			_packageManager = packageManager;
			_logger = logger;
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
			IList<IPackage> packages = _packageRepository.FindPackagesById(packageId)
												.Where(p => p.IsLatestVersion)
												.ToList();

			_logger.Information($"Found {packages.Count} packages with id '{packageId}'");

			foreach (IPackage package in packages)
			{
				_logger.Information($" - Installing '{package.Id}'");
				_packageManager.InstallPackage(package, true, false);
			}
		}

		public void EnsurePluginDirectoryExists(string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}
	}
}