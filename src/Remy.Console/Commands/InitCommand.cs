using System.IO;
using System.Text;
using CommandLine;
using Remy.Core.Tasks;
using Remy.Core.Tasks.Plugins;
using Remy.Core.Tasks.Plugins.Powershell;
using Serilog;

namespace Remy.Console.Commands
{
	public class InitCommand : ICommand
	{
		private IFileProvider _fileProvider;

		[Option('v', "verbose", Required = false)]
		public bool VerboseLogging { get; set; }

		public ILogger Logger { get; set; }

		public InitCommand()
		{
		}

		internal InitCommand(IFileProvider fileProvider)
		{
			_fileProvider = fileProvider;
		}

		public void Run(IServiceLocator serviceLocator)
		{
			var stringbuilder = new StringBuilder();
			stringbuilder.AppendLine("name: \"Example file\"");
			stringbuilder.AppendLine("tasks:");
			stringbuilder.AppendLine("  -");
			stringbuilder.AppendLine("    description: \"Says hello in powershell\"");
			stringbuilder.AppendLine("    runner: powershell");
			stringbuilder.AppendLine("    commands:");
			stringbuilder.AppendLine("      - echo 'Hello from Remy!'");


			if (_fileProvider == null)
				_fileProvider = new FileProvider(Logger);

			string path = Path.Combine(_fileProvider.GetCurrentDirectory(), "remy.yml");
			_fileProvider.WriteAllText(path, stringbuilder.ToString());

			Logger.Information("Example remy.yml file written. Now try use 'remy.exe run'.");
		}
	}
}