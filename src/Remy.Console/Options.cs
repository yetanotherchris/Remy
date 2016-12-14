
using CommandLine;

namespace Remy.Console
{
    public class Options
    {
        [Option('c', "configfile", Required = false, HelpText = "The YAML configuration file to use.")]
        public string ConfigFile { get; set; }
	}
}
