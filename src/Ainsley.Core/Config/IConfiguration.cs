using System.Collections.Generic;

namespace Ainsley.Core.Config
{
	public interface IConfiguration
	{
		string Name { get; set; }
		List<ITaskConfig> Tasks { get; set; }
	}
}