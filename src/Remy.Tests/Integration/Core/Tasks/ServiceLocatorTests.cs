using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Remy.Core.Tasks;
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
		public void should_add_tasks_from_plugins_directory()
		{
			// given + when
			Dictionary<string, ITask> tasks = ServiceLocator.GetRegisteredTaskInstances(_logger);

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