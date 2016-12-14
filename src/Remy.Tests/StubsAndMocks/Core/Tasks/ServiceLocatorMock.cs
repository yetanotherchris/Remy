using System.Collections.Generic;
using Autofac;
using Remy.Core.Tasks;

namespace Remy.Tests.StubsAndMocks.Core.Tasks
{
	public class ServiceLocatorMock : IServiceLocator
	{
		private ContainerBuilder _containerBuilder;

		public IContainer Container
		{
			get { return _containerBuilder.Build(); }
			set { }
		}

		public Dictionary<string, ITask> Tasks { get; set; }

		public ServiceLocatorMock()
		{
			_containerBuilder = new ContainerBuilder();
		}

		public void Register<T>(T child)
		{
			_containerBuilder.Register(c => child).As<T>();
		}

		public void BuildContainer()
		{
			Container = _containerBuilder.Build();
		}

		public Dictionary<string, ITask> TasksAsDictionary(IEnumerable<ITask> allTaskInstances)
		{
			return Tasks;
		}
	}
}