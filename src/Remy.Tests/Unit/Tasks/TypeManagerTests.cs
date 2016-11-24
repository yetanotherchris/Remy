using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Remy.Core.Tasks;
using Remy.Tests.StubsAndMocks;
using Serilog;

namespace Remy.Tests.Unit.Tasks
{
	[TestFixture]
	public class TypeManagerTests
	{
		private ILogger _logger;
		private string _currentDir;
		private string _pluginsDirectory;

		[SetUp]
		public void Setup()
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
		}

		[TearDown]
		public void TearDown()
		{
			Directory.Delete(_pluginsDirectory, true);
		}

		[Test]
		public void should_return_all_itasks_and_register_yamlname_for_keys()
		{
			// given + when
			Dictionary<string, ITask> tasks = TypeManager.GetRegisteredTaskInstances(_logger);

			// then
			Assert.That(tasks.Count, Is.EqualTo(4));

			KeyValuePair<string, ITask> task = tasks.First();
			Assert.That(task.Value, Is.Not.Null);
			Assert.That(task.Key, Is.EqualTo(task.Value.YamlName));
		}

		[Test]
		public void should_add_tasks_from_plugins_directory()
		{
			// given - copy MockTask (Remy.tests.dll) into plugins/
			File.Copy(Path.Combine(_currentDir, "Remy.tests.dll"), Path.Combine(_pluginsDirectory, "Remy.tests.dll"));

			// when
			Dictionary<string, ITask> tasks = TypeManager.GetRegisteredTaskInstances(_logger);

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