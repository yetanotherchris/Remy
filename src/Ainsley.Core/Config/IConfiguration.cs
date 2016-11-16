using System.Collections.Generic;

namespace Ainsley.Core.Yaml
{
	public interface IConfiguration
	{
		string Name { get; set; }
		List<ITaskConfig> Tasks { get; set; }
	}
}