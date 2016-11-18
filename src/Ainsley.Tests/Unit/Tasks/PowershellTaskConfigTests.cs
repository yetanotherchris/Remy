using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks;
using NUnit.Framework;

namespace Ainsley.Tests.Unit.Tasks
{
    [TestFixture]
	public class PowershellTaskConfigTests
    {
        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
            var task = new PowershellTask();
            var config = new PowershellTaskConfig();

            var properties = new Dictionary<object, object>();
            properties["commands"] = new List<object>()
            {
                "ls",
                "echo hello"
            };

            // when
            task.SetConfiguration(config, properties);

            // then
            var actualconfig = task.Config as PowershellTaskConfig;
            Assert.That(actualconfig, Is.Not.Null);

            Assert.That(actualconfig.Commands.Count, Is.EqualTo(2));
            Assert.That(actualconfig.Commands[0], Is.EqualTo("ls"));
            Assert.That(actualconfig.Commands[1], Is.EqualTo("echo hello"));
        }
    }
}