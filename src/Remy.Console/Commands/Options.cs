using CommandLine;

namespace Remy.Console.Commands
{
    public class Options
    {
		[VerbOption("init")]
		public InitCommand InitCommand { get; set; }

		[VerbOption("run")]
		public RunCommand RunCommand { get; set; }

		[VerbOption("plugins")]
		public PluginsCommand PluginCommand { get; set; }
	}
}
