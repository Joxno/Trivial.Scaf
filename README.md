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

# Quickstart
Prerequisites: Installation

## Initialise the tool
```pwsh
scaf init
```
## Add remote & install your first template
```pwsh
scaf repo add "https://github.com/Joxno/Trivial.Scaf.Templates"
```

```pwsh
scaf search template "test"
```

```pwsh
scaf install template "test"
```

```pwsh
scaf test
```

```pwsh
> Hello, World! from test template.
```

## Create your first repo
```pwsh
scaf init repo ./repo
```
> Adding **--add-remote** to the command will automatically add the newly initialised repo as a file repo to your scaf tool config.

## Create your first template
```pwsh
scaf init template "test" "test_key"
```

```pwsh
scaf install template ./ --from-path
```

## Index your repo
```pwsh
scaf index repo
```

## Create your first workspace & add it to your config
```pwsh
scaf init workspace
```

```pwsh
scaf workspace add
```

## Add custom data to your workspace
```pwsh
scaf workspace configure data add "test_key" '{ "Test": "Testing" }'
```

## Remove data from your workspace
```pwsh
scaf workspace configure data remove "test_key"
```

# License
Trivial.Scaf is licensed under the [MIT license](LICENSE).