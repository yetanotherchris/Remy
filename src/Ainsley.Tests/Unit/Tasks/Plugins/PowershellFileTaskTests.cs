using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks.Plugins;
using NUnit.Framework;

namespace Ainsley.Tests.Unit.Tasks.Plugins
{
    [TestFixture]
	public class PowershellFileTaskTests
    {
        [Test]
        public void should_have_yaml_name()
        {
            Assert.That(new PowershellFileTask().YamlName, Is.EqualTo("powershell-file"));
        }

        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
            var task = new PowershellFileTask();
            var config = new PowershellFileTaskConfig();

            var properties = new Dictionary<object, object>();
	        properties["uri"] = "http://www.example.com/powershell.ps1";

            // when
            task.SetConfiguration(config, properties);

            // then
            var actualconfig = task.Config as PowershellFileTaskConfig;
            Assert.That(actualconfig, Is.Not.Null);

            Assert.That(actualconfig.Uri, Is.EqualTo("http://www.example.com/powershell.ps1"));
        }
    }
}