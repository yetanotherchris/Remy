using System.IO;
using System.Text;
using CommandLine;
using Remy.Core.Tasks;
using Serilog;

namespace Remy.Console.Commands
{
	public class InitCommand : ICommand
	{
		[Option('v', "verbose", Required = false)]
		public bool VerboseLogging { get; set; }

		public ILogger Logger { get; set; }
		
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


			string path = Path.Combine(Directory.GetCurrentDirectory(), "remy.yml");
			File.WriteAllText(path, stringbuilder.ToString());
			Logger.Information("Example remy.yml file written. Now try use 'remy.exe run'.");
		}
	}
}