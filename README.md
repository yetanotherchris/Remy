# Ainsley

Ainsley (like the TV Chef) is a lightweight YAML-based Windows server configuration tool, written in C#. Its sole purpose is to provision a Windows Server 2012 upwards, running on the server itself.

Its not a replacement nor trying to compete with: Chef, Ansible, Puppet or Salt. It can of course be used with those tools although its main purpose is to run **common** setup tasks on Windows, *and Windows only*.

## Getting started

All tasks are defined in a YAML file, similarl to Ansible, Docker compose and others. By default Ainsley will look in the current directory for "ainsley.yml" and use this for configuration. You can also specify a path for the configuration:

    ainsley --config=c:\somepath\config.yml
  
## Example YAML configuration

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
        description: "Update machine keys"
        runner: machine-keys
          config:
            -
              name: "KEY1"
              value: "xyz"
            -
              name: "KEY2"
              value: "xyz"
      - 
        description: "Update SMTP config"
        runner: smtp-config
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
    
## Extensions

Ainsley runs using a simple `Command` pattern. Although not implemented yet, future versions will allow for plugins to register custom tasks by simplying implementing the `ITask` interface, which as a plugin author gives you access to the YAML parser.
