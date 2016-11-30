using NuGet;
using System;
using System.Collections.Generic;

namespace Remy.Tests.StubsAndMocks
{
	public class PackageManagerMock : IPackageManager
	{
		public IFileSystem FileSystem { get; set; }
		public IPackageRepository LocalRepository { get; }
		public ILogger Logger { get; set; }
		public DependencyVersion DependencyVersion { get; set; }
		public bool WhatIf { get; set; }
		public IPackageRepository SourceRepository { get; }
		public IPackagePathResolver PathResolver { get; }
		
		public event EventHandler<PackageOperationEventArgs> PackageInstalled;
		public event EventHandler<PackageOperationEventArgs> PackageInstalling;
		public event EventHandler<PackageOperationEventArgs> PackageUninstalled;
		public event EventHandler<PackageOperationEventArgs> PackageUninstalling;

		public List<string> InstalledIds { get; set; }

		public void InstallPackage(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions)
		{
			InstalledIds = new List<string>();
			InstalledIds.Add(package.Id);
		}

		public void InstallPackage(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions, bool ignoreWalkInfo)
		{
			throw new System.NotImplementedException();
		}

		public void InstallPackage(string packageId, SemanticVersion version, bool ignoreDependencies, bool allowPrereleaseVersions)
		{
			throw new System.NotImplementedException();
		}

		public void UpdatePackage(IPackage newPackage, bool updateDependencies, bool allowPrereleaseVersions)
		{
			throw new System.NotImplementedException();
		}

		public void UpdatePackage(string packageId, SemanticVersion version, bool updateDependencies, bool allowPrereleaseVersions)
		{
			throw new System.NotImplementedException();
		}

		public void UpdatePackage(string packageId, IVersionSpec versionSpec, bool updateDependencies, bool allowPrereleaseVersions)
		{
			throw new System.NotImplementedException();
		}

		public void UninstallPackage(IPackage package, bool forceRemove, bool removeDependencies)
		{
			throw new System.NotImplementedException();
		}

		public void UninstallPackage(string packageId, SemanticVersion version, bool forceRemove, bool removeDependencies)
		{
			throw new System.NotImplementedException();
		}
	}
}