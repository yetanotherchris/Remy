$configuration = "Debug";
$ErrorActionPreference = "Stop"
$solutionFile      = "Remy.sln"
$platform          = "Any CPU"
$msbuild           = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

if (!(Test-Path $msbuild))
{
	$msbuild = "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
}

# Nuget restore
Write-Host "Performing Nuget restore" -ForegroundColor Green
nuget restore $solutionFile

# Build the sln file
Write-Host "Building $solutionFile." -ForegroundColor Green

& $msbuild $solutionFile /p:Configuration=$configuration /p:Platform=$platform /target:Build /verbosity:minimal 
if ($LastExitCode -ne 0)
{
	throw "Building solution failed."
}
else
{
	Write-Host "  Building solution complete."-ForegroundColor Green
}
