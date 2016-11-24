using System;
using Remy.Core.Config;

namespace Remy.Tests.StubsAndMocks
{
	public class ConfigFileReaderMock : IConfigFileReader
	{
		public string Yaml { get; set; }

		public string Read(Uri uri)
		{
			return Yaml;
		}
	}
}
