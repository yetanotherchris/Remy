using System.Collections.Generic;

namespace Ainsley.Core.Yaml
{
	public interface ITaskConfig
	{
		string Description { get; set; }
		string Runner { get; set; }
		Dictionary<object, object> Config { get; set; }

		ITaskConfig Deserialize(Dictionary<object, object> properties);
	}
}