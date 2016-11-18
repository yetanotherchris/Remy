using System;
using System.IO;
using System.Net;
using Serilog;

namespace Ainsley.Core.Config
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
                    throw new AinsleyException($"Unable to download open file '{uri}' - {e.Message}");
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
                    throw new AinsleyException("Unable to download '{uri}' - {e.Message}");
				}
			}

			throw new AinsleyException($"The scheme {uri.Scheme} for {uri} is not supported");
		}

	    public static string GetFullPathForFile(string filename)
	    {
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }
	}
}