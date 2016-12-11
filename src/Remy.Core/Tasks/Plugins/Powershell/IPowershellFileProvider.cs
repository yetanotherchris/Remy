using System;

namespace Remy.Core.Tasks.Plugins.Powershell
{
	public interface IPowershellFileProvider
	{
		/// <summary>
		/// Downloads a file from the url and returns it as a string.
		/// </summary>
		string Download(Uri uri);

		/// <summary>
		/// Writes the powershell script contents to a temporary file, and returns the path including the .ps1 extension.
		/// </summary>
		/// <returns></returns>
		string WriteTemporaryFile(string powershellText);

		/// <summary>
		/// Gets the current directory remy.exe is being run from, which is where the local powershell file will run from.
		/// </summary>
		/// <returns></returns>
		string GetCurrentDirectory();
	}
}