using System.Collections.Generic;

namespace Ainsley.Core.Config
{
	public interface ITaskConfig
	{
		string Description { get; set; }
		string Runner { get; set; }
		Dictionary<string, object> Config { get; set; }
	}

	public class TaskConfig : ITaskConfig
	{
		public string Description { get; set; }
		public string Runner { get; set; }
		public Dictionary<string, object> Config { get; set; }
	}
}