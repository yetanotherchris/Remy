using System;
using System.Collections.Generic;
using System.IO;
using Remy.Core.Tasks;
using Serilog;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Remy.Core.Config.Yaml
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
			if (dictionary.ContainsKey(key))
			{
				return dictionary[key].ToString();
			}

			return "";
		}

		public List<ITask> Parse(Uri uri)
		{
		    var tasks = new List<ITask>();

		    try
		    {
		        string yaml = _configFileReader.Read(uri);
		        var input = new StringReader(yaml);

		        var deserializer = new Deserializer();
		        var children = deserializer.Deserialize(input) as Dictionary<object, object>;

		        if (children != null && children.ContainsKey("tasks"))
		        {
		            tasks = ParseTasks(children);
		        }
		    }
		    catch (RemyException ex)
		    {
		        _logger.Error(ex.Message);
            }
            catch (SyntaxErrorException ex)
            {
                _logger.Error($"An error occurred parsing the Yaml' in '{uri}': {ex.Message}");
            }

            return tasks;
		}

		private List<ITask> ParseTasks(Dictionary<object, object> children)
		{
			List<object> tasks = children["tasks"] as List<object>;
		    if (tasks == null)
		        return new List<ITask>();

		    var registeredTasks = new List<ITask>();

            foreach (var taskNode in tasks)
			{
				Dictionary<object, object> taskProperties = taskNode as Dictionary<object, object>;

				if (taskProperties.ContainsKey("runner"))
				{
                    string runner = taskProperties["runner"].ToString();

                    if (_registeredTasks.ContainsKey(runner))
					{ 
						ITask task = _registeredTasks[runner];

						ITaskConfig taskConfig = new TaskConfig();
						taskConfig.Description = GetOptionalKey(taskProperties, "description");
						taskConfig.Runner = runner;

						// "config:" element, eg environmental variables
						taskConfig.Config = new Dictionary<string, object>();
						if (taskProperties.ContainsKey("config"))
						{
						    ParseConfigSection(taskProperties, taskConfig.Config);
						}

                        task.SetConfiguration(taskConfig, taskProperties);
                        registeredTasks.Add(task);
                    }
					else
					{
						_logger.Warning("No task plugin found for '{runner}'", runner);
					}
				}
				else
				{
					_logger.Warning("'runner' key was missing for a task.");
				}
			}

		    return registeredTasks;
		}

	    private void ParseConfigSection(Dictionary<object, object> taskProperties, Dictionary<string, object> configDictionary)
	    {
            List<object> config = taskProperties["config"] as List<object>;

            foreach (var configItem in config)
	        {
	            Dictionary<object, object> keyValue = configItem as Dictionary<object, object>;

	            if (keyValue != null && keyValue.ContainsKey("name") && keyValue.ContainsKey("value"))
	            {
	                string keyName = keyValue["name"].ToString();

	                if (!string.IsNullOrEmpty(keyName) && !configDictionary.ContainsKey(keyName))
	                {
                        configDictionary.Add(keyName, keyValue["value"]);
	                }
	            }
	        }
	    }
	}
}