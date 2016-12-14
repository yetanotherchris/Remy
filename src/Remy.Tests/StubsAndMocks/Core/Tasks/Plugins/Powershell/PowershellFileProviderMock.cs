using System;
using Remy.Core.Tasks;
using Remy.Core.Tasks.Plugins.Powershell;

namespace Remy.Tests.StubsAndMocks.Core.Tasks.Plugins.Powershell
{
	public class FileProviderMock : IFileProvider
	{
		public string DownloadContent { get; set; }
		public string TempFilePath { get; set; }
		public string CurrentDirectory { get; set; }

		public string FileWritePath { get; set; }
		public string FileWriteContent { get; set; }

		public string Download(Uri uri)
		{
			return DownloadContent;
		}

		public string WriteTemporaryFile(string fileContent)
		{
			return TempFilePath;
		}

		public string GetCurrentDirectory()
		{
			return CurrentDirectory;
		}

		public void WriteAllText(string path, string content)
		{
			FileWritePath = path;
			FileWriteContent = content;
		}
	}
}