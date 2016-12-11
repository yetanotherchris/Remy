using System;
using Remy.Core.Tasks.Plugins.Powershell;

namespace Remy.Tests.StubsAndMocks.Core.Tasks.Plugins.Powershell
{
	public class PowershellFileProviderMock : IPowershellFileProvider
	{
		public string DownloadContent { get; set; }
		public string TempFilePath { get; set; }
		public string CurrentDirectory { get; set; }

		public string Download(Uri uri)
		{
			return DownloadContent;
		}

		public string WriteTemporaryFile(string powershellText)
		{
			return TempFilePath;
		}

		public string GetCurrentDirectory()
		{
			return CurrentDirectory;
		}
	}
}