using System;
using System.Collections.Generic;
using Remy.Core.Config.Yaml;
using Remy.Core.Tasks;

namespace Remy.Tests.StubsAndMocks.Core.Config.Yaml
{
	public class YamlConfigParserMock : IYamlConfigParser
	{
		public List<ITask> ExpectedTasks { get; set; }
		public Uri ActualUri { get; set; }

		public List<ITask> Parse(Uri uri)
		{
			ActualUri = uri;
			return ExpectedTasks;
		}
	}
}