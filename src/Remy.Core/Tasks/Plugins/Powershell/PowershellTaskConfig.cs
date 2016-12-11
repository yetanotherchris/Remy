using System.Collections.Generic;
using Remy.Core.Config;

namespace Remy.Core.Tasks.Plugins.Powershell
{
	public class PowershellTaskConfig : ITaskConfig
	{
	    public string Description { get; set; }
	    public Dictionary<string, object> Config { get; set; }

	    public List<string> Commands { get; set; }
	}
}