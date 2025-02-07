Set-Location $PSScriptRoot

dotnet tool uninstall --global trivial.cli
dotnet pack
dotnet tool install --global --add-source .\nupkg Trivial.CLI