Set-Location $PSScriptRoot

# NuGet Package
dotnet build .\Trivial.CLI -p:BuildType=tool -p:PackageVersion="1.0.0-D$([DateTime]::UtcNow.ToString("yyMMddHHmmss"))"

# Standalone
@(
    @{ Runtime="win-x64"; SourceName="Trivial.CLI.exe"; TargetName="scaf.exe" },
    @{ Runtime="win-x86"; SourceName="Trivial.CLI.exe"; TargetName="scaf.exe" },
    @{ Runtime="win-arm64"; SourceName="Trivial.CLI.exe"; TargetName="scaf.exe" },
    @{ Runtime="linux-x64"; SourceName="Trivial.CLI"; TargetName="scaf" },
    @{ Runtime="linux-musl-x64"; SourceName="Trivial.CLI"; TargetName="scaf" },
    @{ Runtime="linux-musl-arm64"; SourceName="Trivial.CLI"; TargetName="scaf" },
    @{ Runtime="linux-arm"; SourceName="Trivial.CLI"; TargetName="scaf" },
    @{ Runtime="linux-arm64"; SourceName="Trivial.CLI"; TargetName="scaf" },
    @{ Runtime="osx-x64"; SourceName="Trivial.CLI"; TargetName="scaf" },
    @{ Runtime="osx-arm64"; SourceName="Trivial.CLI"; TargetName="scaf" }
) | % {
    dotnet publish .\Trivial.CLI -c Release -p:BuildType="standalone-$($_.Runtime)" --output "./Trivial.CLI/standalone-tool/$($_.Runtime)"
    if(Test-Path -Path (Join-Path "./Trivial.CLI/standalone-tool/$($_.Runtime)" $_.TargetName))
    {
        Remove-Item -Path (Join-Path "./Trivial.CLI/standalone-tool/$($_.Runtime)" $_.TargetName) -Force
    }
    Rename-Item -Path (Join-Path "./Trivial.CLI/standalone-tool/$($_.Runtime)" $_.SourceName) -NewName $_.TargetName -Force
}