using System;

namespace Remy.Core.Tasks
{
	public interface IFileProvider
	{
		/// <summary>
		/// Downloads a file from the url and returns it as a string.
		/// </summary>
		string Download(Uri uri);

		/// <summary>
		/// Writes the file contents to a temporary file, and returns the path.
		/// </summary>
		/// <returns></returns>
		string WriteTemporaryFile(string fileContent);

		/// <summary>
		/// Gets the current directory remy.exe is being run from.
		/// </summary>
		/// <returns></returns>
		string GetCurrentDirectory();

		/// <summary>
		/// It's basically File.WriteAllText
		/// </summary>
		void WriteAllText(string path, string content);
	}
}