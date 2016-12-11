using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;
using Remy.Core.Tasks.Plugins;
using Remy.Tests.StubsAndMocks;
using Remy.Tests.StubsAndMocks.Core.Config;
using Remy.Tests.StubsAndMocks.Core.Config.Yaml;
using Remy.Tests.StubsAndMocks.Core.Tasks;
using Remy.Tests.StubsAndMocks.Core.Tasks.Plugins.Powershell;
using Serilog;
using Serilog.Core;

namespace Remy.Tests.Unit.Core.Config.Yaml
{
    [TestFixture]
	public class YamlConfigParserTests
	{
	    private Logger _logger;

	    [SetUp]
	    public void Setup()
	    {
            _logger = new LoggerConfiguration()
                .WriteTo
                .LiterateConsole()
                .CreateLogger();
        }

        [Test]
        public void should_use_registeredTasks_to_get_task_from_name_in_config()
        {
            // given
            var readerMock = new ConfigFileReaderMock();
            readerMock.Yaml = ExampleYaml.AdvancedYaml;

            var registeredTasks = new Dictionary<string, ITask>();
			var windowsFeatureTask = new WindowsFeatureTask(new PowershellRunnerMock());
			registeredTasks.Add("windows-feature", windowsFeatureTask);

            var yamlParser = new YamlConfigParser(readerMock, registeredTasks, _logger);

            // when
            List<ITask> tasks = yamlParser.Parse(new Uri("file://filename"));

            // then
            Assert.That(tasks.Count, Is.EqualTo(1));

            ITask task = tasks.First();
            Assert.That(task, Is.Not.Null);
            Assert.That(task, Is.TypeOf<WindowsFeatureTask>());
            Assert.That(task.Config, Is.TypeOf<WindowsFeatureTaskConfig>());
        }

        [Test]
        public void should_parse_custom_property_using_task()
        {
            // given
            var readerMock = new ConfigFileReaderMock();
            readerMock.Yaml = ExampleYaml.MockTest;

            var registeredTasks = new Dictionary<string, ITask>();
            registeredTasks.Add("mock-task", new MockTask());

            var yamlParser = new YamlConfigParser(readerMock, registeredTasks, _logger);

            // when
            List<ITask> tasks = yamlParser.Parse(new Uri("file://filename"));

            // then
            Assert.That(tasks.Count, Is.EqualTo(1));

            ITask task = tasks.First();
            Assert.That(task, Is.Not.Null);
            Assert.That(task, Is.TypeOf<MockTask>());

            MockTaskConfig config = task.Config as MockTaskConfig;
            Assert.That(config, Is.Not.Null);
            Assert.That(config.CustomProperty, Is.EqualTo("ahyeh"));
        }

        [Test]
        public void should_parse_config_section()
        {
            // given
            var readerMock = new ConfigFileReaderMock();
            readerMock.Yaml = ExampleYaml.MockTest;

            var registeredTasks = new Dictionary<string, ITask>();
            registeredTasks.Add("mock-task", new MockTask());

            var yamlParser = new YamlConfigParser(readerMock, registeredTasks, _logger);

            // when
            List<ITask> tasks = yamlParser.Parse(new Uri("file://filename"));

            // then
            ITask task = tasks.First();
            MockTaskConfig config = task.Config as MockTaskConfig;
            Assert.That(config, Is.Not.Null);
            Assert.That(config.Config["KEY1"], Is.EqualTo("abc"));
            Assert.That(config.Config["KEY2"], Is.EqualTo("xyz"));
        }

        [Test]
        public void should_ignore_duplicate_config_keys()
        {
            // given
            var readerMock = new ConfigFileReaderMock();
            readerMock.Yaml = ExampleYaml.DuplicateConfigKeys;

            var registeredTasks = new Dictionary<string, ITask>();
            registeredTasks.Add("mock-task", new MockTask());

            var yamlParser = new YamlConfigParser(readerMock, registeredTasks, _logger);

            // when
            List<ITask> tasks = yamlParser.Parse(new Uri("file://filename"));

            // then
            ITask task = tasks.First();
            MockTaskConfig config = task.Config as MockTaskConfig;
            Assert.That(config, Is.Not.Null);
            Assert.That(config.Config["KEY1"], Is.EqualTo("abc"));
        }

        [Test]
        public void should_ignore_empty_config_keys()
        {
            // given
            var readerMock = new ConfigFileReaderMock();
            readerMock.Yaml = ExampleYaml.EmptyConfigKeys;

            var registeredTasks = new Dictionary<string, ITask>();
            registeredTasks.Add("mock-task", new MockTask());

            var yamlParser = new YamlConfigParser(readerMock, registeredTasks, _logger);

            // when
            List<ITask> tasks = yamlParser.Parse(new Uri("file://filename"));

            // then
            ITask task = tasks.First();
            MockTaskConfig config = task.Config as MockTaskConfig;
            Assert.That(config, Is.Not.Null);
            Assert.That(config.Config.Count, Is.EqualTo(1));
        }

        [Test]
        public void should_ignore_empty_runner_key_on_task()
        {
            // given
            var readerMock = new ConfigFileReaderMock();
            readerMock.Yaml = ExampleYaml.MissingRunner;

            var registeredTasks = new Dictionary<string, ITask>();
            registeredTasks.Add("mock-task", new MockTask());

            var yamlParser = new YamlConfigParser(readerMock, registeredTasks, _logger);

            // when
            List<ITask> tasks = yamlParser.Parse(new Uri("file://filename"));

            // then
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [Test]
        [TestCase(ExampleYaml.InvalidYaml)]
        [TestCase(ExampleYaml.InvalidYamlWithTabs)]
        public void should_log_invalid_yml_and_return_empty_list(string yaml)
        {
            var logStringBuilder = new StringBuilder();
            var logMessages = new StringWriter(logStringBuilder);

            var logger = new LoggerConfiguration()
                .WriteTo
                .TextWriter(logMessages)
                .CreateLogger();
        
            // given
            var readerMock = new ConfigFileReaderMock();
            readerMock.Yaml = yaml;

            var registeredTasks = new Dictionary<string, ITask>();
	        var task = new WindowsFeatureTask(new PowershellRunnerMock());
	        registeredTasks.Add("windows-feature", task);

            var yamlParser = new YamlConfigParser(readerMock, registeredTasks, logger);

            // when
            List<ITask> tasks = yamlParser.Parse(new Uri("file://myfilename.txt"));

            // then
            Assert.That(tasks.Count, Is.EqualTo(0));

	        string logText = logStringBuilder.ToString();
			Assert.That(logText, Does.Contain("An error occurred parsing the Yaml"), logText);
			Assert.That(logText, Does.Contain("myfilename.txt"), logText);
		}
	}
}