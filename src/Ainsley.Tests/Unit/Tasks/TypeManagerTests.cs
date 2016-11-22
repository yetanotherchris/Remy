using System.Collections.Generic;
using System.Linq;
using Ainsley.Core.Tasks;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Ainsley.Tests.Unit.Tasks
{
    [TestFixture]
	public class TypeManagerTests
    {
		private ILogger _logger;

		[SetUp]
		public void Setup()
		{
			_logger = new LoggerConfiguration()
				.WriteTo
				.LiterateConsole()
				.CreateLogger();
		}

		[Test]
        public void should_return_all_itasks_and_register_yamlname_for_keys()
        {
            // given + when
	        Dictionary<string, ITask> tasks = TypeManager.GetRegisteredTaskInstances(_logger);

	        // then
			Assert.That(tasks.Count, Is.EqualTo(3));

	        KeyValuePair<string, ITask> task = tasks.First();
	        Assert.That(task.Value, Is.Not.Null);
	        Assert.That(task.Key, Is.EqualTo(task.Value.YamlName));
        }
	}
}