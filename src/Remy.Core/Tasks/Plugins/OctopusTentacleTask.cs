using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Remy.Core.Config;
using Serilog;

namespace Remy.Core.Tasks.Plugins
{
	public class OctopusTentacleTask : ITask
	{
		public string YamlName => "octopus-tentacle";
		private OctopusTentacleTaskConfig _config;
		public ITaskConfig Config => _config;

		public void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties)
		{
			_config = new OctopusTentacleTaskConfig();
			_config.Description = config.Description;

			// Guards: role, environment, apikey, server, thumbprint, tentacleName are all required
			Guard(properties, "role");
			Guard(properties, "environment");
			Guard(properties, "apikey");
			Guard(properties, "octopusServer");
			Guard(properties, "thumbprint");
			Guard(properties, "tentacleName");

			if (properties.ContainsKey("apikey"))
				_config.ApiKey = properties["apikey"].ToString();

			if (properties.ContainsKey("environment"))
				_config.Environment = properties["environment"].ToString();

			if (properties.ContainsKey("installDirectory"))
				_config.TentacleExePath = properties["installDirectory"].ToString();

			if (properties.ContainsKey("octopusServer"))
				_config.OctopusServer = properties["octopusServer"].ToString();

			if (properties.ContainsKey("role"))
				_config.Role = properties["role"].ToString();

			if (properties.ContainsKey("thumbprint"))
				_config.Thumbprint = properties["thumbprint"].ToString();

			if (properties.ContainsKey("certText"))
				_config.CertText = properties["certText"].ToString();

			if (properties.ContainsKey("port"))
				_config.Port = properties["port"].ToString();

			if (properties.ContainsKey("homeDir"))
				_config.HomeDirectory = properties["homeDir"].ToString();

			if (properties.ContainsKey("appDir"))
				_config.AppDirectory = properties["appDir"].ToString();

			if (properties.ContainsKey("tentacleName"))
				_config.TentacleName = properties["tentacleName"].ToString();

			if (properties.ContainsKey("exePath"))
				_config.TentacleName = properties["exePath"].ToString();
		}

		private void Guard(Dictionary<object, object> properties, string propertyName)
		{
			if (!properties.ContainsKey(propertyName))
				throw new RemyException($"The '{propertyName}' property is missing and is required by the {YamlName} task");
		}

		public void Run(ILogger logger)
		{
			string ip = GetIpAddress();

			bool success = RunCommand(logger, $@"create-instance --instance ""Tentacle"" --config ""{_config.ConfigPath}"" --console");

			if (!string.IsNullOrEmpty(_config.CertText))
			{
				string tempCertPath = Path.GetTempFileName() + ".txt";
				File.WriteAllText(tempCertPath, _config.CertText);
				success &= RunCommand(logger, $@"import-certificate --instance ""Tentacle"" -f ""{tempCertPath}"" --console");
			}

			success &= RunCommand(logger, $@"configure --instance ""Tentacle"" --home=""{_config.HomeDirectory}"" --console");
			success &= RunCommand(logger, $@"configure --instance ""Tentacle"" --app=""{_config.AppDirectory}"" --console");
			success &= RunCommand(logger, $@"configure --instance ""Tentacle"" --port=""{_config.Port}"" --console");
			success &= RunCommand(logger, $@"configure --instance ""Tentacle"" --trust=""{_config.Thumbprint}""");
			success &= RunCommand(logger, $@"register-with --instance ""Tentacle"" --name=""{_config.TentacleName}"" --publicHostName=""{ip}"" --server=""{_config.OctopusServer}"" --apiKey=""{_config.ApiKey}"" --role=""{_config.Role}"" --environment=""{_config.Environment}"" --comms-style=TentaclePassive --console --force");
			success &= RunCommand(logger, @"service --instance ""Tentacle"" --install --console");
			success &= RunCommand(logger, @"service --instance ""Tentacle"" --start --console");

			if (!success)
				throw new RemyException("The Octopus tentacle configuration failed, see the previous messages for information.");
		}

		private string GetIpAddress()
		{
			string ip = Dns.GetHostName();
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

			foreach (IPAddress ipAddress in host.AddressList)
			{
				if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					ip = ipAddress.ToString();
				}
			}
			return ip;
		}

		public bool RunCommand(ILogger logger, string startArgs)
		{
			var startInfo = new ProcessStartInfo(_config.TentacleExePath);
			startInfo.Arguments = startArgs;
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
					logger.Error(args.Data);
					errors = true;
				}
			};

			process.OutputDataReceived += (sender, args) => logger.Information(args.Data);
			process.EnableRaisingEvents = true;
			process.StartInfo = startInfo;

			Console.WriteLine($"Running {_config.TentacleExePath} {startInfo.Arguments}");
			process.Start();
			process.BeginErrorReadLine();
			process.BeginOutputReadLine();
			process.WaitForExit((int)TimeSpan.FromMinutes(1).TotalMilliseconds);
			process.CancelOutputRead();
			process.CancelErrorRead();

			return process.ExitCode == 0 && !errors;
		}
	}
}
