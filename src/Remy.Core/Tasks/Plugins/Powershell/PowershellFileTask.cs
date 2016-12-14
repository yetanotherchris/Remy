using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Remy.Core.Config;
using Serilog;

namespace Remy.Core.Tasks.Plugins.Powershell
{
    public class PowershellFileTask : ITask
    {
        private PowershellFileTaskConfig _config;
	    private readonly IFileProvider _fileProvider;
	    private readonly IPowershellRunner _powershellRunner;

	    public ITaskConfig Config => _config;
        public string YamlName => "powershell-file";

		public PowershellFileTask(IFileProvider fileProvider, IPowershellRunner powershellRunner)
		{
			_fileProvider = fileProvider;
			_powershellRunner = powershellRunner;
		}

		public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
        {
            _config = new PowershellFileTaskConfig();
            _config.Description = config.Description;
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
		        if (!_powershellRunner.RunFile(powershellFilePath))
		        {
			        logger.Error($"An error occurred running {powershellFilePath} - see previous errors.");
		        }
	        }
        }

		private string ParseUri(string uri, ILogger logger)
	    {
		    if (uri.StartsWith("file://"))
		    {
			    return new Uri(uri).LocalPath;
		    }

		    if (uri.StartsWith("http"))
		    {
				//
			    // Remote
				//
			    try
			    {
				    Uri remoteUri = new Uri(uri);
				    string powershellText = _fileProvider.Download(remoteUri);
				    string tempFilename = _fileProvider.WriteTemporaryFile(powershellText);

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
		    else
		    {
				//
			    // Local
				//
			    if (uri.StartsWith("./"))
			    {
				    uri = Path.Combine(_fileProvider.GetCurrentDirectory(), uri);
			    }
				else if (!uri.Contains(Environment.NewLine))
			    {
					uri = Path.Combine(_fileProvider.GetCurrentDirectory(), uri);
				}

			    Uri fileUri = new Uri("file://" + uri);
			    return fileUri.LocalPath;
		    }
	    }
    }
}