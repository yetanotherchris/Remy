using Ainsley.Core.Config;

namespace Ainsley.Tests.StubsAndMocks
{
	public class ConfigFileReaderMock : IConfigFileReader
	{
		public string Yaml { get; set; }

		public string Read(string filename)
		{
			return Yaml;
		}
	}
}
