using System.Collections.Generic;

namespace Ainsley.Core.Config
{
	public class Configuration : IConfiguration
	{
		public string Name { get; set; }
		public List<ITaskConfig> Tasks { get; set; }
	}
}