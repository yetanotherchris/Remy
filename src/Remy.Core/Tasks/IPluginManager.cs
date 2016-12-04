using System.Collections.Generic;
using NuGet;

namespace Remy.Core.Tasks
{
	public interface IPluginManager
	{
		void DownloadAndUnzip(string packageId);
		void EnsurePluginDirectoryExists(string path);
		IEnumerable<IPackage> List();
	}
}