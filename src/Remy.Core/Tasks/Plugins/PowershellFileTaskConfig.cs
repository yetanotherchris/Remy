using System.Collections.Generic;
using Remy.Core.Config;

namespace Remy.Core.Tasks.Plugins
{
	public class PowershellFileTaskConfig : ITaskConfig
	{
	    public string Description { get; set; }
	    public Dictionary<string, object> Config { get; set; }
		public string Uri { get; set; }
	}
}