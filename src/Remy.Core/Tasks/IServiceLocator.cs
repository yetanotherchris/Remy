using System.Collections.Generic;
using StructureMap;

namespace Remy.Core.Tasks
{
	public interface IServiceLocator
	{
		IContainer Container { get; set; }
		void BuildContainer();
		Dictionary<string, ITask> TasksAsDictionary(IEnumerable<ITask> allTaskInstances);
	}
}