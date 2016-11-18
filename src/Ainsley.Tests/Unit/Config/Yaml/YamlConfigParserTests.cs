using System;
using System.Collections.Generic;
using System.Linq;
using Ainsley.Core.Config;
using Ainsley.Core.Config.Yaml;
using Ainsley.Core.Tasks;
using Ainsley.Tests.StubsAndMocks;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Ainsley.Tests.Unit.Config.Yaml
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
            registeredTasks.Add("windows-feature", new WindowsFeatureTask());

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
    }
}