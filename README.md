# Trivial.Scaf
Automating code generation and scaffolding

## WIP
This tool is very early on in development, many design and implementation details are yet to be solidified.
As such any interfaces, models and/or workflows are highly unstable and bound to change.

## Disclaimer
This is an internal tool used at Trivial Software to handle scaffolding in a uniform way to allow for easier automation of scaffolding/code generation projects.

# Dependencies
Prerequisites:
* .NET 9.0
* Powershell v7.5

# Installation
### Dotnet
Easily install the tool via dotnet.
```pwsh
dotnet install tool Trivial.Scaf --global --prerelease
```

### Standalone
Portable standalone executables

### Docker
Build docker image with predefined defaults.
```pwsh
.\Build-Docker.ps1
```
Run the built docker image interactively in a temporary container.
```pwsh
.\Run-Docker.ps1
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
#### scaf workspace
#### scaf \<key>

## License
[MIT](LICENSE)