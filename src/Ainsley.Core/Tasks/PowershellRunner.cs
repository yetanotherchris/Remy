using System;
using System.Diagnostics;
using System.Text;

namespace Ainsley.Core.Tasks
{
    public class PowershellRunner
    {
        public string RunCommand(string command)
        {
            var outputBuilder = new StringBuilder();

            var startInfo = new ProcessStartInfo("powershell.exe");
            startInfo.Arguments = command;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            var process = new Process();
            process.ErrorDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            process.OutputDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            process.EnableRaisingEvents = true;
            process.StartInfo = startInfo;

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit((int) TimeSpan.FromMinutes(1).TotalMilliseconds);
            process.CancelOutputRead();

            return outputBuilder.ToString();
        }
    }
}