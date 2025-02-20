# Trivial.Scaf
Automating code generation and scaffolding

> [!WARNING]
> This tool is very early on in development, many design and implementation details are yet to be solidified.
> As such any interfaces, models and/or workflows are highly unstable and bound to change.

> [!NOTE]
> This is an internal tool used at [Trivial Software](https://trivialsoftware.co.uk/) to handle scaffolding in a uniform way to allow for easier automation of scaffolding/code generation projects.

# Dependencies
Prerequisites:
* [.NET 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) (If installing via dotnet)
* [Powershell v7.5](https://github.com/PowerShell/PowerShell/releases/tag/v7.5.0)

# Installation
### Dotnet
Install the tool via dotnet.
```pwsh
dotnet install tool Trivial.Scaf --global --prerelease
```

### Standalone
Portable standalone executables for Windows, MacOS and Linux.
> [!NOTE]
> This still requires Powershell v7.5 to be installed on system.

### Docker
Build docker image with predefined defaults.
```pwsh
./Build-Docker.ps1
```
Run the built docker image interactively in a temporary container.
```pwsh
./Run-Docker.ps1
```

# Commands
#### scaf config <add | remove>
#### scaf folder \<path>
#### scaf init <repo | template>
#### scaf index \<repo>
#### scaf install <template | script>
#### scaf list <templates | repos | cfg | pwd | dirs>
#### scaf remove \<key>
#### scaf repo <add | remove>
#### scaf search \<query>
#### scaf workspace <add | remove>
#### scaf update <all | repos | templates>
#### scaf \<key>

# Concepts
### Repo
### Template
### Workspace

## License
Trivial.Scaf is licensed under the [MIT license](LICENSE).