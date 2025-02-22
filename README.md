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

# Glossary
### Repo
Repos are indexed collections that contain templates. These repos can either be stored on disk, file-share or hosted on a git service.
> [!NOTE]
> Currently only publically accessible git repositories are supported. Support for private repositories requires further implementation.

### Template
Templates are scaffolds that are able to be executed.

### Workspace
Workspaces are configurations for storing custom data specifically for a folder/project.

# Quickstart
Prerequisites: Installation

## Initialise the tool
In-order for the scaf tool to function correctly you must first initialise it by running the following command:
```pwsh
scaf init
```
This will setup all necessary directories and config files.

## Show help
Help can be shown by adding '-h' at the end of any command to display available commands, descriptions, required arguments and available options.
```pwsh
scaf -h
```

## List directories & config used by scaf
To find out what dirs are currently used by scaf run the list dirs command:
```pwsh
scaf list dirs
```

Example output:
```pwsh
/root/.local/share/Trivial.Scaf
/root/.local/share/Trivial.Scaf/templates
/root/.local/share/Trivial.Scaf/cfg
/root/.local/share/Trivial.Scaf/cfg/remotes
```

Running list config all will output the scaf.config.json file which is the primary global config for the scaf tool.
```pwsh
scaf list config all
```

Example output:
```pwsh
{
  "Templates": {
    "InstalledTemplatesPaths": [
      "/root/.local/share/Trivial.Scaf/templates"
    ]
  },
  "Repos": {
    "Repos": []
  },
  "Workspaces": []
}
```
## Add remote & install your first template
Repo add command will download the latest repository index and add it to scaf config as an installed repository.
```pwsh
scaf repo add "https://github.com/Joxno/Trivial.Scaf.Templates"
```

> Viewing installed repos may be done by running "scaf list repos" command.

Search for templates in any installed repo indexes.
```pwsh
scaf search template "test"
```
Search output:
```pwsh
Id                                          Name        Key         
--------------------------------------------------------------------
62fdfd93-6d8b-4175-9e07-0d969f8690d9        test        test        
```

Install template will copy from the local cache into an installed templates path.
```pwsh
scaf install template "test"
```
> Running the command "scaf list config templates" will list the currently configured paths that templates can be installed into.

Running the newly installed template by using its key.
```pwsh
scaf test
```

Default output from "test" template:
```pwsh
> Hello, World! from test template.
```

## Create your first repo
To initialise a new repo run the init command and supply a path to a local folder for the repo to be initialised into. If the directory doesn't exist it will be created for you ahead of the initialisation.
```pwsh
scaf init repo ./repo
```
> Adding **--add-remote** to the command will automatically add the newly initialised repo as a file repo to your scaf tool config.

Init will setup the repo with certain defaults, to configure the repo further run the repo configure command to set the name and url for the repo.
```pwsh
scaf repo configure ./repo --name "test-repo" --url ""
```
> If the repo is meant to be hosted on a git server such as github or gitlab, configure the Url to point to the git repository url.

## Create your first template
```pwsh
scaf init template "test" "test_key"
```
> Adding **--only-config** will setup the template with only the config file and leave out the default .ps1 scripts.

```pwsh
scaf install template ./ --from-path
```

## Index your repo
When templates are added or removed from your repo you must run the index command.
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