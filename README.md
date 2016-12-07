# Remy

[![Build status](https://ci.appveyor.com/api/projects/status/udw791pwvc8wanf8/branch/master?svg=true)](https://ci.appveyor.com/project/yetanotherchris/remy/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/yetanotherchris/Remy/badge.svg?branch=master)](https://coveralls.io/github/yetanotherchris/Remy?branch=master)
[![Chocolatey](https://img.shields.io/chocolatey/dt/Remy.svg)](https://chocolatey.org/packages/remy)
[![Nuget.org](https://img.shields.io/nuget/v/Remy.Core.svg?style=flat)](https://www.nuget.org/packages/Remy.Core)

Remy (like the rat Chef) is a lightweight YAML-based Windows server configuration tool, written in C#. Its sole purpose is to provision a Windows Server 2012 upwards, running on the server itself.

It's heavily inspired by Travis and Appveyor YML configuration files, and design goal is to be as simple to use as possible.

Its not a replacement nor trying to compete with: Chef, Ansible, Puppet or Salt. It can of course be used with those tools although its main purpose is to run **common** setup tasks on Windows, *and Windows only*.

## Getting started

All tasks are defined in a YAML file. By default Remy will look in the current directory for "remy.yml" and use this for configuration. You can also specify a path for the configuration from a file or url:

    .\remy.exe --config=c:\somepath\config.yml
    .\remy.exe --config=https://raw.githubusercontent.com/yetanotherchris/Remy/master/someconfig.yml
  
## Example YAML configuration

**NOTE: YAML requires indentation with spaces**

A basic example, this installs IIS and .NET:

    name: "Basic example"
    tasks:
      -
        description: "Install IIS, .NET framework"
        runner: windows-feature
        includeAllSubFeatures: true
        features: 
          - NET-Framework-Core
          - Web-Server
          - NET-Framework-Features
          - NET-Framework-45-ASPNET
          - Application-Server
          - MSMQ
          - WAS

This is a more advanced example

    name: "Advanced example"
    tasks:
      - 
        description: "Update execution policy for Powershell"
        runner: execution-policy
        policy: unrestricted
      - 
        description: "Install chocolatey"
        runner: install-chocolatey
      -
        description: "Install IIS, .NET framework"
        runner: windows-feature
        includeAllSubFeatures: true
        features: 
          - NET-Framework-Core
          - Web-Server
          - NET-Framework-Features
          - NET-Framework-45-ASPNET
          - Application-Server
    - 
      description: "Install Octopus"
      runner: octopus-tentacle
      config:
        -
          name: "KEY1"
          value: "xyz"
        -
          name: "KEY2"
          value: "xyz"
      - 
        description: "Set file permissions"
        runner: acl
        user: Local Service
        directory: "c:\windows\temp"
      - 
        description: "Disable firewall"
        runner: toggle-firewall
        enable: false
      - 
        description: "Install WebApi"
        runner: install-webpi
    
## Plugins

You can load custom tasks via plugins in Remy. There are various commands available:

```
remy.exe plugins list
remy.exe plugins install MyPlugin 
remy.exe plugins list --source=http://my-nuget-server
remy.exe plugins install MyPrivatePlugin --source=http://my-nuget-server
```

Remy uses Nuget to download and install plugins into the `plugins/` under the directory remy.exe is running from. To find plugins, Remy searches nuget.org (or your custom nuget repository) for all packages with the `remy-plugin` tags.


### Writing your own Plugin

Plugins are simple to write:

1. `install-package Remy.Core`
2. Implement `ITask`
3. If you require custom elements in the YAML file for your task, implement `ITaskConfig`. See the [WindowsFeatureTask](https://github.com/yetanotherchris/Remy/blob/master/src/Remy.Core/Tasks/Plugins/WindowsFeatureTask.cs) and [WindowsFeatureTaskConfig](https://github.com/yetanotherchris/Remy/blob/master/src/Remy.Core/Tasks/Plugins/WindowsFeatureTaskConfig.cs) files for examples. The `SetConfiguration` method is the important part.

You should then pack and push your plugin onto nuget.org or your custom nuget server, and tag it with "remy-plugin" in the nuspec file.
