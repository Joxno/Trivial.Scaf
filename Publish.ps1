param($NugetApiKey)

$PackageName = Get-ChildItem -Path .\Trivial.CLI\nupkg\*.nupkg | select -first 1 -ExpandProperty Name
dotnet nuget push "./Trivial.CLI/nupkg/$PackageName" -k $NugetApiKey -s https://api.nuget.org/v3/index.json