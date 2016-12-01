using System;
using System.Diagnostics;
using System.IO;
using Serilog;

namespace Remy.Core.Tasks.Runners
{
    public class PowershellRunner
    {
        private readonly ILogger _logger;

        public PowershellRunner(ILogger logger)
        {
            _logger = logger;
        }

	    public bool RunCommands(string[] commands)
	    {
			if (commands == null || commands.Length == 0)
				return true;

			string tempFilename = "";
			try
			{
				tempFilename = Path.GetTempFileName() + ".ps1";
				File.WriteAllLines(tempFilename, commands);
			}
			catch (IOException e)
			{
				_logger.Error(e.Message);
				return false;
			}

			if (string.IsNullOrEmpty(tempFilename))
			{
				_logger.Error("Unable to run powershell script - temp file name creation failed.");
				return false;
			}

		    return RunFile(tempFilename);
	    }

	    public bool RunFile(string tempFilename)
        {
            var startInfo = new ProcessStartInfo("powershell.exe");
            startInfo.Arguments = "-ExecutionPolicy Unrestricted -File " + tempFilename;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            bool errors = false;
            var process = new Process();
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    _logger.Error(args.Data);

                    // This flag is a work around for -File always returning an exit code of 0
                    errors = true;
                }
            };
            process.OutputDataReceived += (sender, args) => _logger.Information(args.Data);
            process.EnableRaisingEvents = true;
            process.StartInfo = startInfo;

            _logger.Information($"Running powershell.exe {startInfo.Arguments}");
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit((int) TimeSpan.FromMinutes(1).TotalMilliseconds);
            process.CancelOutputRead();
            process.CancelErrorRead();

            return process.ExitCode == 0 && !errors;
        }
    }
}