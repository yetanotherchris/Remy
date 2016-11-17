using System.Collections.Generic;
using Ainsley.Core.Config;
using Ainsley.Core.Tasks;
using NUnit.Framework;

namespace Ainsley.Tests.Unit.Tasks
{
    [TestFixture]
	public class WindowsFeatureTaskTests
    {
        [Test]
        public void SetConfiguration_should_set_config_from_properties()
        {
            // given
            var task = new WindowsFeatureTask();
            var config = new WindowsFeatureTaskConfig();

            var properties = new Dictionary<object, object>();
            properties["includeAllSubFeatures"] = true;
            properties["features"] = new List<object>()
            {
                "NET-Framework-Core",
                "Web-Server"
            };

            // when
            task.SetConfiguration(config, properties);

            // then
            var actualconfig = task.Config as WindowsFeatureTaskConfig;
            Assert.That(actualconfig, Is.Not.Null);

            Assert.That(actualconfig.IncludeAllSubFeatures, Is.True);
            Assert.That(actualconfig.Features.Count, Is.EqualTo(2));
            Assert.That(actualconfig.Features[0], Is.EqualTo("NET-Framework-Core"));
            Assert.That(actualconfig.Features[1], Is.EqualTo("Web-Server"));
        }
    }
}