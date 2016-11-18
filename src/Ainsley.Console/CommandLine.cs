using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser = CommandLine;

namespace Ainsley.Core.CommandLine
{
    public class Options
    {
        [CommandLineParser.Option('f', "configfile", Required = true, HelpText = "The YAML configuration file to use.")]
        public string ConfigFile { get; set; }

        [CommandLineParser.Option('v', "verbose", Required = false, HelpText = "Verbose output (warnings and info).")]
        public bool Verbose { get; set; }
    }
}
