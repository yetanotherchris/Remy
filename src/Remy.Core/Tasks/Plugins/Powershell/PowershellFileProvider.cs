using System;
using System.IO;
using System.Net;
using Serilog;

namespace Remy.Core.Tasks.Plugins.Powershell
{
	public class PowershellFileProvider : IPowershellFileProvider
	{
		private readonly ILogger _logger;

		public PowershellFileProvider(ILogger logger)
		{
			_logger = logger;
		}

		public string Download(Uri uri)
		{
			return new WebClient().DownloadString(uri);
		}

		public string WriteTemporaryFile(string powershellText)
		{
			string tempFilename = Path.GetTempFileName() + ".ps1";

			if (string.IsNullOrEmpty(tempFilename))
			{
				_logger.Error("Unable to run powershell script - temp file name creation failed.");
				return "";
			}

			File.WriteAllText(tempFilename, powershellText);
			return tempFilename;
		}

		public string GetCurrentDirectory()
		{
			return Directory.GetCurrentDirectory();
		}
	}
}