# Built in Remy plugins

Remy has a number of built in tasks you can use in your YAML config file.

## Install Chocolatey  plugin

`install-chocolatey`

Installs Chocolatey via a Powershell command window. This runs using `-ExecutionPolicy Unrestricted` in Powershell.

### Example

    name: "Install choco example"
    tasks:
      -
        description: "Install chocolatey"
        runner: install-chocolatey

## Windows features plugin

`windows-feature`

### Example config

    name: "Windows features example"
    tasks:
      description: "Install IIS, .NET framework"
          runner: windows-feature
          includeAllSubFeatures: true
          features: 
            - NET-Framework-Core
            - Web-Server
            - NET-Framework-Features
            - NET-Framework-45-ASPNET
            - Application-Server

### Options

- includeAllSubFeatures: installs all features of a parent feature, eg Web-Server.
- features: A list of features, you can find the names using `Get-WindowsFeature` in Powershell.

## Powershell commands plugin

`powershell`

Runs a list of powershell commands, combined together inside a single `.ps1` file.  This runs using `-ExecutionPolicy Unrestricted` in Powershell.

### Example config

    name: "Run powershell commands example"
    tasks:
      description: "Say hello to the world"
          runner: powershell
          commands: 
            - $env:test="hello world"
            - Write-Host $env:test

### Options

- commands: A list of powershell commands, which are combined into a single script file.

## Powershell file plugin

`powershell-file`

Runs a local or remote powershell file.  This runs using `-ExecutionPolicy Unrestricted` in Powershell.

### Example config

    name: "Run powershell file example"
    tasks:
      description: "Run a file"
          runner: powershell
          uri: http://www.example.com/myfile.ps1

### Options

- uri: A remote or local path. Possible values can be in these formats:
 - 'myfile.ps1`
 - `./myfile.ps1`
 - `http://example.com/myfile.ps1`
 - `file://c:\myfile.ps1`

## Octopus tentacle configure plugin

`octopus-tentacle`

Configures a pre-installed Octopus tentacle.

### Options

Defaults:

        HomeDirectory = @"C:\Octopus";
        AppDirectory = @"C:\Octopus\Applications";
        ConfigPath = @"C:\Octopus\Tentacle\Tentacle.config";
        TentacleExePath = @"C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe";

- octopusServer:  (Required) 
- environment:  (Required) 
- role: (Required) 
- apiKey: (Required) 
- thumbprint: (Required) 
- tentacleExePath: 
- configPath: The Tentacle config path
- certText: 
- port: 
- homeDirectory: 
- appDirectory: 
- tentacleName: (Required) name for the tentacle.
