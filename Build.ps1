# NuGet Package
dotnet build .\Trivial.CLI -p:BuildType=tool -p:PackageVersion="1.0.0-D$([DateTime]::UtcNow.ToString("yyMMddHHmmss"))"

# standalone-win-x64
dotnet publish .\Trivial.CLI -c Release -p:BuildType=standalone-win-x64 --output ./Trivial.CLI/standalone-tool/win-x64
Rename-Item -Path .\Trivial.CLI\standalone-tool\win-x64\Trivial.CLI.exe -NewName scaf.exe