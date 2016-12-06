using System;
using System.IO;
using System.Net;
using Serilog;

namespace Remy.Core.Config
{
	public class ConfigFileReader : IConfigFileReader
	{
		public string Read(Uri uri)
		{
			if (uri.IsFile)
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
			else if (uri.Scheme.StartsWith("http"))
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

			throw new RemyException($"The scheme {uri.Scheme} for {uri} is not supported");
		}

	    public static string GetFullPathForFile(string filename)
	    {
			return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
	}
}