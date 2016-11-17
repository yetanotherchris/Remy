using System.Collections.Generic;

namespace Ainsley.Core.Config
{
	public interface ITaskConfig
	{
		string Description { get; set; }
		string Runner { get; set; }
		Dictionary<object, object> Config { get; set; }
	}

	public class TaskConfig : ITaskConfig
	{
		public string Description { get; set; }
		public string Runner { get; set; }
		public Dictionary<object, object> Config { get; set; }
	}
}