namespace Remy.Tests.Unit.Core.Config.Yaml
{
    public class ExampleYaml
    {
        public const string InvalidYaml = @"
Some text
dd:- dsds : -";

        public const string InvalidYamlWithTabs = @"
name: ""Basic example""
tasks:
			-
description: ""invalid""
		runner: invalid";

        public const string SimpleYaml = @"
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

        public const string MockTest = @"
name: ""Basic example""
tasks:
  -
    description: ""Do something""
    runner: mock-task
    customProperty: ahyeh
    config:
      -
        name: ""KEY1""
        value: ""abc""
      -
        name: ""KEY2""
        value: ""xyz""
    ";

        public const string DuplicateConfigKeys = @"
name: ""Basic example""
tasks:
  -
    description: ""Do something""
    runner: mock-task
    customProperty: ahyeh
    config:
      -
        name: ""KEY1""
        value: ""abc""
      -
        name: ""KEY1""
        value: ""abc""
      -
        name: ""KEY2""
        value: ""xyz""
    ";

        public const string EmptyConfigKeys = @"
name: ""Basic example""
tasks:
  -
    description: ""Do something""
    runner: mock-task
    customProperty: ahyeh
    config:
      -
        name: """"
        value: ""abc""
      -
        name: ""KEY2""
        value: ""xyz""
    ";

        public const string MissingRunner = @"
name: ""Basic example""
tasks:
  -
    description: ""Do something""
    ";

        public const string AdvancedYaml = @"name: ""Advanced example""
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
    config:
      -
        name: ""KEY1""
        value: ""xyz""
      -
        name: ""KEY2""
        value: ""xyz""
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
    }
}