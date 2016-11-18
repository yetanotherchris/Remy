using System.Collections.Generic;
using Ainsley.Core.Config;
using Serilog;

namespace Ainsley.Core.Tasks
{
	public interface ITask
	{
        /// <summary>
        /// The name of the "runner" element in the YAML file.
        /// </summary>
	    string YamlName { get; }

        ITaskConfig Config { get; }
		void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties);
		void Run(ILogger logger);
	}
}