using System;
using System.Collections.Generic;
using System.IO;
using Ainsley.Core.Tasks;
using Serilog;
using YamlDotNet.Serialization;

namespace Ainsley.Core.Config.Yaml
{
	public class YamlConfigParser : IYamlConfigParser
	{
		private readonly IConfigFileReader _configFileReader;
		private readonly Dictionary<string, ITask> _registeredTasks;
		private readonly ILogger _logger;

		public YamlConfigParser(IConfigFileReader configFileReader, Dictionary<string, ITask> registeredTasks, ILogger logger)
		{
			_configFileReader = configFileReader;
			_registeredTasks = registeredTasks;
			_logger = logger;
		}

		private string GetOptionalKey(Dictionary<object, object> dictionary, string key)
		{
			if (dictionary.ContainsKey("name"))
			{
				return dictionary["name"].ToString();
			}

			return "";
		}

		public IConfiguration Parse(string filename)
		{
			string yaml = _configFileReader.Read(filename);
			var input = new StringReader(yaml);

			// There's no auto serialization as YamlDotNet doesn't support down casting
			var deserializer = new Deserializer();
			var children = deserializer.Deserialize(input) as Dictionary<object, object>;

			var configuration = new Configuration();
			configuration.Tasks = new List<ITaskConfig>();
			configuration.Name = GetOptionalKey(children, "name");

			if (children.ContainsKey("tasks"))
			{
				ParseTasks(children, configuration);
			}

			return configuration;
		}

		private void ParseTasks(Dictionary<object, object> children, Configuration configuration)
		{
			List<object> tasks = children["tasks"] as List<object>;

			foreach (var taskNode in tasks)
			{
				Dictionary<object, object> taskProperties = taskNode as Dictionary<object, object>;
				string runner = taskProperties["runner"].ToString();
				if (taskProperties.ContainsKey("runner"))
				{
					if (_registeredTasks.ContainsKey(runner))
					{ 
						ITask task = _registeredTasks[runner];
						Type imp = task.GetType().GetProperty("Config").GetType();

						ITaskConfig taskConfig = new TaskConfig();
						taskConfig.Description = GetOptionalKey(taskProperties, "description");
						taskConfig.Runner = runner;
						task.SetConfiguration(taskConfig, taskProperties);


						// config: element, eg environmental variables
						taskConfig.Config = new Dictionary<object, object>();
						if (taskProperties.ContainsKey("config"))
						{
							List<object> config = taskProperties["config"] as List<object>;
							foreach (var configItem in config)
							{
								Dictionary<object, object> keyValue = configItem as Dictionary<object, object>;
								//taskConfig.Config.Add(keyValue)
							}
						}
					}
					else
					{
						_logger.Warning("No plugin found for {runner}", runner);
					}
				}
				else
				{
					_logger.Warning("No runner was found for a task.");
				}
			}
		}
	}
}