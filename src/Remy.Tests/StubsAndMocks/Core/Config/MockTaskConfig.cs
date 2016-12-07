using System.Collections.Generic;
using Remy.Core.Config;

namespace Remy.Tests.StubsAndMocks.Core.Config
{
    public class MockTaskConfig : ITaskConfig
    {
        public string Description { get; set; }
        public string Runner { get; set; }
        public Dictionary<string, object> Config { get; set; }

        public string CustomProperty { get; set; }
    }
}