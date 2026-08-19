[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$Runtime = 'win-x64',
    [string]$OutputPath = ''
)

$ErrorActionPreference = 'Stop'

$projectPath = Join-Path $PSScriptRoot 'src\RaqmiTweaks.App\RaqmiTweaks.App.csproj'
if (-not (Test-Path $projectPath)) {
    throw "Project file not found: $projectPath"
}

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $PSScriptRoot 'artifacts\release'
}

$dotnetCandidates = @(
    (Join-Path $HOME '.dotnet\dotnet.exe'),
    (Join-Path ${env:ProgramFiles} 'dotnet\dotnet.exe')
)

$dotnetPath = $dotnetCandidates | Where-Object { $_ -and (Test-Path $_) } | Where-Object {
    try {
        $sdkList = & $_ --list-sdks 2>$null
        return ($null -ne $sdkList) -and ($sdkList -match '\d+\.\d+\.\d+')
    }
    catch {
        return $false
    }
} | Select-Object -First 1

if (-not $dotnetPath) {
    throw 'No .NET SDK installation could be found. Install .NET 8 SDK and rerun this script.'
}

New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
& $dotnetPath publish $projectPath -c $Configuration -r $Runtime --self-contained false -o $OutputPath --nologo
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$zipPath = Join-Path $PSScriptRoot 'artifacts\RaqmiTweaks-Release.zip'
if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Compress-Archive -Path (Join-Path $OutputPath '*') -DestinationPath $zipPath -Force

Write-Host "Published release to: $OutputPath"
Write-Host "Setup package created at: $zipPath"
Write-Host 'Use the built app from the output folder, or distribute the zip package as the setup bundle.'
