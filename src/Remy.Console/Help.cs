using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Remy.Console
{
	public class Help
	{
		public static string GetHelpText()
		{
			var fileVersionInfo = FileVersionInfo.GetVersionInfo(typeof(Help).Assembly.Location);
			string version = fileVersionInfo.ProductVersion;
			
			return GetStringFromResource("Remy.Console.help.txt")
					.Replace("{version}", version);
		}

		private static string GetStringFromResource(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("path", "The path is null or empty");

			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
			if (stream == null)
				throw new InvalidOperationException(string.Format("Unable to find '{0}' as an embedded resource", path));

			string result = "";
			using (StreamReader reader = new StreamReader(stream))
			{
				result = reader.ReadToEnd();
			}

			return result;
		}
	}
}