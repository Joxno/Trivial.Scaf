Set-Location $PSScriptRoot
Get-ChildItem ./nupkg/* | Remove-Item

dotnet tool uninstall --global trivial.scaf
dotnet build -c Release
dotnet pack
dotnet tool install --global --add-source ./nupkg Trivial.Scaf --prerelease