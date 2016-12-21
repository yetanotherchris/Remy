using System.Collections.Generic;
using Remy.Core.Tasks;
using StructureMap;

namespace Remy.Tests.StubsAndMocks.Core.Tasks
{
	public class ServiceLocatorMock : IServiceLocator
	{
		public IContainer Container { get; set; }

		public Dictionary<string, ITask> Tasks { get; set; }

		public ServiceLocatorMock()
		{
			Container = new Container();
		}

		public void Register<T>(T instance)
		{
			Container.Configure(x => x.For<T>().Use(() => instance));
		}

		public void BuildContainer()
		{
			
		}

		public Dictionary<string, ITask> TasksAsDictionary(IEnumerable<ITask> allTaskInstances)
		{
			return Tasks;
		}
	}
}