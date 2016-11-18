using System.Collections.Generic;

namespace Ainsley.Core.Config
{
    public class TaskConfig : ITaskConfig
    {
        public string Description { get; set; }
        public string Runner { get; set; }
        public Dictionary<string, object> Config { get; set; }
    }
}