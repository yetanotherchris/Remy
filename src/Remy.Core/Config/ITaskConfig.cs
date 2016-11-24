using System.Collections.Generic;

namespace Remy.Core.Config
{
	public interface ITaskConfig
	{
		string Description { get; set; }
		string Runner { get; set; }
		Dictionary<string, object> Config { get; set; }
	}
}