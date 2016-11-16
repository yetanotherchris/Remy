using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace Ainsley.Core.Yaml
{
	public class YamlConfigParser : IYamlConfigParser
	{
		private readonly IConfigFileReader _configFileReader;

		public YamlConfigParser(IConfigFileReader configFileReader)
		{
			_configFileReader = configFileReader;
		}

		public IConfiguration Parse(string filename)
		{
			var configuration = new Configuration();
			configuration.Tasks = new List<ITaskConfig>();

			string yaml = _configFileReader.Read(filename);
			var input = new StringReader(yaml);

			// There's no auto serialization as YamlDotNet doesn't support down casting
			var deserializer = new Deserializer();
			Dictionary<object,object> o = deserializer.Deserialize(input) as Dictionary<object, object>;
			List<object> bleh = o["tasks"] as List<object>;

			var bleh2 = bleh.Cast<List<ITaskConfig>>();

			foreach (var task in bleh)
			{
				Dictionary<object, object> items = task as Dictionary<object, object>;
				string description = items["description"].ToString();
				string runner = items["runner"].ToString();

				// Example, impl will get it from a dictionary of all plugins
				if (runner == "windows-feature")
				{
					ITaskConfig taskConfig = new WindowsFeatureTaskConfig();
					taskConfig = taskConfig.Deserialize(items);
					configuration.Tasks.Add(taskConfig);
				}

				if (items.ContainsKey("config"))
				{
					List<object> config = items["config"] as List<object>;
					foreach (var configItem in config)
					{
						Dictionary<object, object> keyValue = configItem as Dictionary<object, object>;
					}
				}
			}

			return configuration;
		}
	}
}
