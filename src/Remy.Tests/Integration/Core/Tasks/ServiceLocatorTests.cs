using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Remy.Core.Config;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Remy.Core.Tasks.Plugins;
using Remy.Core.Tasks.Plugins.Powershell;
using Remy.Tests.StubsAndMocks.Core.Tasks;
using Serilog;

namespace Remy.Tests.Integration.Core.Tasks
{
	[TestFixture]
	[Category("Foo")]
	public class ServiceLocatorTests
	{
		private ILogger _logger;
		private string _currentDir;
		private string _pluginsDirectory;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_logger = new LoggerConfiguration()
				.WriteTo
				.LiterateConsole()
				.CreateLogger();

			_currentDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
			_pluginsDirectory = Path.Combine(_currentDir, "plugins");

			if (Directory.Exists(_pluginsDirectory))
				Directory.Delete(_pluginsDirectory, true);
				
			Directory.CreateDirectory(_pluginsDirectory);

			// copy MockTask (Remy.tests.dll)into plugins /
			string nugetFolder = Path.Combine(_pluginsDirectory, "Remy.tests", "lib", "net461");
			Directory.CreateDirectory(nugetFolder);
			File.Copy(Path.Combine(_currentDir, "Remy.tests.dll"), Path.Combine(nugetFolder, "Remy.tests.dll"));
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			Directory.Delete(_pluginsDirectory, true);
		}

		[Test]
		public void should_register_remy_types()
		{
			// given
			var serviceLocator = new ServiceLocator(_logger);
			serviceLocator.BuildContainer();

			// when
			IContainer container = serviceLocator.Container;

			// then
			Assert.That(container.Resolve<IPowershellRunner>, Is.TypeOf<PowershellRunner>());
			Assert.That(container.Resolve<IFileProvider>, Is.TypeOf<FileProvider>());
			Assert.That(container.Resolve<IConfigFileReader>, Is.TypeOf<ConfigFileReader>());
			Assert.That(container.Resolve<ILogger>, Is.Not.Null);
			Assert.That(container.Resolve<IYamlConfigParser>, Is.TypeOf<YamlConfigParser>());
		}

		[Test]
		public void should_add_tasks_from_plugins_directory()
		{
			// given + when
			var serviceLocator = new ServiceLocator(_logger);
			serviceLocator.BuildContainer();
			IEnumerable<ITask> taskInstances = serviceLocator.Container.Resolve<IEnumerable<ITask>>();

			Dictionary<string, ITask> tasks = serviceLocator.TasksAsDictionary(taskInstances);

			// then
			Assert.That(tasks.Count, Is.EqualTo(5));

			KeyValuePair<string, ITask> task = tasks.FirstOrDefault(x => x.Key == "mock-task");
			Assert.That(task, Is.Not.Null);
			Assert.That(task.Key, Is.EqualTo(task.Value.YamlName));
			Assert.That(task.Value, Is.Not.Null);
			Assert.That(task.Value.GetType().Name, Is.EqualTo(typeof(MockTask).Name));
		}
	}
}