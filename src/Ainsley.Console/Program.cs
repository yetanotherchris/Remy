using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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


                    Type type = typeof(ITask);
                    IEnumerable<Type> taskTypes = type.Assembly
                        .GetTypes()
                        .Where(x => type.IsAssignableFrom(x) && x.IsClass);

                    var registeredTasks = new Dictionary<string, ITask>();
                    foreach (Type taskType in taskTypes)
                    {
                        var instance = Activator.CreateInstance(taskType);

                        ITask taskInstance = instance as ITask;
                        registeredTasks.Add(taskInstance.YamlName, taskInstance);

                        logger.Information($"Registered '{taskType.Name}' as '{taskInstance.YamlName}'");
                    }

                    var configReader = new ConfigFileReader();
                    var parser = new YamlConfigParser(configReader, registeredTasks, logger);

                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ainsley.yml");
                    var tasks = parser.Parse(new Uri(fullPath));

                    foreach (ITask task in tasks)
                    {
                        task.Run(logger);
                    }

                    System.Console.WriteLine("");
                });
        }
    }
}
