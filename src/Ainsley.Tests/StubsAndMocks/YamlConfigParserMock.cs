using System;
using System.Collections.Generic;
using Ainsley.Core.Config.Yaml;
using Ainsley.Core.Tasks;

namespace Ainsley.Tests.StubsAndMocks
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