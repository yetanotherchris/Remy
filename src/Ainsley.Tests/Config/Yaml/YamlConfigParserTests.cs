using Ainsley.Core.Yaml;
using Ainsley.Tests.StubsAndMocks;
using NUnit.Framework;

namespace Ainsley.Tests.Yaml
{
	[TestFixture]
	public class YamlConfigParserTests
	{
		private const string _simpleYml = @"
name: ""Basic example""
tasks:
  -
    description: ""Install IIS, .NET framework""
    runner: windows-feature
    includeAllSubFeatures: true
    features: 
      - NET-Framework-Core
      - Web-Server
      - NET-Framework-Features
      - NET-Framework-45-ASPNET
      - Application-Server
      - MSMQ
      - WAS";

		private const string _advancedYml = @"name: ""Advanced example""
tasks:
  - 
    description: ""Update execution policy for Powershell""
    runner: execution-policy
    policy: unrestricted
  - 
    description: ""Install chocolatey""
    runner: install-chocolatey
  - 
    description: ""Update machine keys""
    runner: machine-keys
    config:
      -
        name: ""KEY1""
        value: ""xyz""
      -
        name: ""KEY2""
        value: ""xyz""
  - 
    description: ""Update SMTP config""
    runner: smtp-config
  -
    description: ""Install IIS, .NET framework""
    runner: windows-feature
    includeAllSubFeatures: true
    features: 
      - NET-Framework-Core
      - Web-Server
      - NET-Framework-Features
      - NET-Framework-45-ASPNET
      - Application-Server
  - 
    description: ""Install Octopus""
    runner: octopus-tentacle
    config:
      -
        name: ""KEY1""
        value: ""xyz""
      -
        name: ""KEY2""
        value: ""xyz""";

		[Test]
		public void should()
		{
			// given
			var readerMock = new ConfigFileReaderMock();
			readerMock.Yaml = _advancedYml;

			var yamlParser = new YamlConfigParser(readerMock);

			// when
			yamlParser.Parse("not used");

			// then
		}

	}
}
