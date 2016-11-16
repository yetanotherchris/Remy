using System.Collections.Generic;

namespace Ainsley.Core.Yaml
{
	public class Configuration : IConfiguration
	{
		public string Name { get; set; }
		public List<ITaskConfig> Tasks { get; set; }
	}
}