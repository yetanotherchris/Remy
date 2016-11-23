using System.Collections.Generic;
using Ainsley.Core.Config;

namespace Ainsley.Core.Tasks.Plugins
{
	public class PowershellFileTaskConfig : ITaskConfig
	{
	    public string Description { get; set; }
	    public string Runner { get; set; }
	    public Dictionary<string, object> Config { get; set; }
		public string Uri { get; set; }
	}
}