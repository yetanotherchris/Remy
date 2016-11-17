using System.Collections.Generic;
using Ainsley.Core.Config;

namespace Ainsley.Core.Tasks
{
	public interface ITask
	{
        ITaskConfig Config { get; }
		void SetConfiguration(ITaskConfig config, Dictionary<object, object> properties);
		void Run();
	}
}