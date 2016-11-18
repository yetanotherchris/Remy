using System;
using System.Collections.Generic;
using System.IO;
using Ainsley.Core.Config;
using Ainsley.Core.Config.Yaml;
using Ainsley.Core.Tasks;
using CommandLine;
using Serilog;

namespace Ainsley.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    var logger = new LoggerConfiguration()
                                    .WriteTo
                                    .LiterateConsole()
                                    .CreateLogger();


                    var registeredTasks = new Dictionary<string, ITask>();
                    registeredTasks.Add("powershell", new PowershellTask());
                    registeredTasks.Add("windows-feature", new WindowsFeatureTask());


                    var configReader = new ConfigFileReader();
                    var parser = new YamlConfigParser(configReader, registeredTasks, logger);

                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ainsley.yml");
                    var tasks = parser.Parse(new Uri(fullPath));

                    foreach (ITask task in tasks)
                    {
                        task.Run(logger);
                    }
                });
        }
    }
}
