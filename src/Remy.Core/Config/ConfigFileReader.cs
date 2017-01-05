using System;
using System.IO;
using System.Net;

namespace Remy.Core.Config
{
	public class ConfigFileReader : IConfigFileReader
	{
		public string Read(Uri uri)
		{
			if (uri.IsFile)
			{
				string yaml = ReadLocalFile(uri);
				yaml = ReplaceAllEnvironmentalVariables(yaml);

				return yaml;
			}
			else if (uri.Scheme.StartsWith("http"))
			{
				string yaml = ReadRemoteFile(uri);
				yaml = ReplaceAllEnvironmentalVariables(yaml);

				return yaml;
			}

			throw new RemyException($"The scheme {uri.Scheme} for {uri} is not supported");
		}

		private string ReadLocalFile(Uri uri)
		{
			try
			{
				return File.ReadAllText(uri.LocalPath);
			}
			catch (IOException e)
			{
				throw new RemyException($"Unable to download open file '{uri}' - {e.Message}", e);
			}
		}

		private string ReadRemoteFile(Uri uri)
		{
			try
			{
				return new WebClient().DownloadString(uri);
			}
			catch (WebException e)
			{
				throw new RemyException($"Unable to download '{uri}' - {e.Message}", e);
			}
		}

		private string ReplaceAllEnvironmentalVariables(string yaml)
		{
			yaml = ReplaceEnvironmentalVariables(yaml, EnvironmentVariableTarget.User);
			yaml = ReplaceEnvironmentalVariables(yaml, EnvironmentVariableTarget.Process);
			yaml = ReplaceEnvironmentalVariables(yaml, EnvironmentVariableTarget.Machine);

			return yaml;
		}

		private string ReplaceEnvironmentalVariables(string yaml, EnvironmentVariableTarget target)
		{
			var envVars = Environment.GetEnvironmentVariables(target);
			foreach (string key in envVars.Keys)
			{
				yaml = yaml.Replace("{{" + key + "}}", envVars[key].ToString());
			}

			return yaml;
		}

	    public static string GetFullPathForFile(string filename)
	    {
			return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
	}
}