using System;
using System.Collections.Generic;
using System.Reflection;
using Remy.Core.Config;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Serilog;

namespace Remy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                                .WriteTo
                                .LiterateConsole()
                                .CreateLogger();

	        try
	        {
				Dictionary<string, ITask> registeredTasks = TypeManager.GetRegisteredTaskInstances(logger);
				var configReader = new ConfigFileReader();
				var parser = new YamlConfigParser(configReader, registeredTasks, logger);

				var commandLineRunner = new CommandLineRunner(logger, parser);
				commandLineRunner.Run(args);
	        }
	        catch (Exception e)
	        {
		        logger.Error($"Unhandled error: {e.Message}");
	        }
        }
	}
}
