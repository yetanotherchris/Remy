using System;
using System.IO;
using System.Net;
using Serilog;

namespace Remy.Core.Tasks.Plugins
{
	public class FileProvider : IFileProvider
	{
		private readonly ILogger _logger;
		public string TempFileExtension { get; set; }

		public FileProvider(ILogger logger)
		{
			_logger = logger;
			TempFileExtension = ".ps1";
		}

		public string Download(Uri uri)
		{
			return new WebClient().DownloadString(uri);
		}

		/// <summary>
		/// Writes the powershell script contents to a temporary file, and returns the path (by default including a .ps1 extension).
		/// </summary>
		public string WriteTemporaryFile(string fileContent)
		{
			string tempFilename = Path.GetTempFileName() + TempFileExtension;

			if (string.IsNullOrEmpty(tempFilename))
			{
				_logger.Error("Unable to run powershell script - temp file name creation failed.");
				return "";
			}

			File.WriteAllText(tempFilename, fileContent);
			return tempFilename;
		}

		/// <summary>
		/// Gets the current directory remy.exe is being run from, which is where the local powershell file will run from.
		/// </summary>
		/// <returns></returns>
		public string GetCurrentDirectory()
		{
			return Directory.GetCurrentDirectory();
		}

		public void WriteAllText(string path, string content)
		{
			File.WriteAllText(path, content);
		}
	}
}