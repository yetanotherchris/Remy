using System.Collections.Generic;
using Ainsley.Core.Config;

namespace Ainsley.Tests.Unit.Config.Yaml
{
    public class MockTaskConfig : ITaskConfig
    {
        public string Description { get; set; }
        public string Runner { get; set; }
        public Dictionary<string, object> Config { get; set; }

        public string CustomProperty { get; set; }
    }
}