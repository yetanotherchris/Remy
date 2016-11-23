using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks.Runners;
using Serilog;

namespace Ainsley.Core.Tasks.Plugins
{
    public class PowershellFileTask : ITask
    {
        private PowershellFileTaskConfig _config;

        public ITaskConfig Config => _config;
        public string YamlName => "powershell-file";

        public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new PowershellFileTaskConfig();
            _config.Description = config.Description;
            _config.Runner = config.Runner;
	        _config.Uri = properties["uri"].ToString();
        }

        public void Run(ILogger logger)
        {
            if (string.IsNullOrEmpty(_config.Uri))
            {
                logger.Warning($"Skipping task '{_config.Description}' as no uri is set for the task.");
                return;
            }

			string powershellFilePath = ParseUri(_config.Uri, logger);

	        if (!string.IsNullOrEmpty(powershellFilePath))
	        {
		        var runner = new PowershellRunner(logger);
		        runner.RunFile(powershellFilePath);
	        }
        }

	    private string ParseUri(string uri, ILogger logger)
	    {
		    if (!uri.StartsWith("http") && !uri.StartsWith("file://"))
		    {
			    if (!uri.StartsWith("/") && !uri.StartsWith("./"))
			    {
				    uri = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uri);
			    }

				Uri fileUri = new Uri("file://" + uri);
			    if (fileUri.IsAbsoluteUri && !File.Exists(fileUri.LocalPath))
			    {
				    logger.Error($"The powershell file '{fileUri.LocalPath}' does not exist");
				    return null;
			    }

			    return fileUri.LocalPath;
		    }
		    else
		    {
				try
				{
					Uri remoteUri = new Uri(uri);
					string powershellText = new WebClient().DownloadString(remoteUri);
					string tempFilename = Path.GetTempFileName() + ".ps1";
					if (string.IsNullOrEmpty(tempFilename))
					{
						logger.Error("Unable to run powershell script - temp file name creation failed.");
						return null;
					}

					File.WriteAllText(tempFilename, powershellText);

					return tempFilename;
				}
				catch (WebException e)
				{
					logger.Error($"Unable to download '{uri}' - {e.Message}", e);
					return null;
				}
				catch (IOException e)
				{
					logger.Error(e.Message);
					return null;
				}
			}
		}
    }
}
