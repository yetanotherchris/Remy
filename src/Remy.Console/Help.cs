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
			return GetStringFromResource("Remy.Console.help.txt")
					.Replace("{version}", GetVersion());
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

		/// <summary>
		/// This version number WILL NOT match the one produced by Appveyor - appveyor patches the AssemblyInfo.cs file.
		/// The version number here is only ever used for development.
		/// </summary>
		/// <returns></returns>
		internal static string GetVersion()
		{
			return typeof(Help).Assembly.GetName().Version.ToString();
		}
	}
}