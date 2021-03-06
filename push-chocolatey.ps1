param(
	[Parameter(Mandatory=$true)]
	[String]
	$apiKey,
	[Parameter(Mandatory=$true)]
	[String]
	$versionNumber
)

$ErrorActionPreference = "Stop"

pushd src\Remy.Console

try
{
	rm *.nupkg

	# Download the release to get its checksum
	wget https://github.com/yetanotherchris/Remy/releases/download/remy-v$versionNumber/Remy-$versionNumber.zip -OutFile Remy-$versionNumber.zip
	$hash = Get-FileHash Remy-$versionNumber.zip
	$hash = $hash.Hash
	Write-Host "Hash is: $hash"

	# Replace the checksum and version in the chocoinstall.ps1 file
	$pwd = pwd
	$content = [IO.File]::ReadAllText("$pwd\chocolateyinstall.template.ps1")
	$content = $content.Replace("{CHECKSUM}", $hash)
	$content = $content.Replace("{VERSION}", $versionNumber)

	[IO.File]::WriteAllText("$pwd\chocolateyinstall.ps1", $content)

	# Push to chocolatey.org
	choco pack Remy.Console.nuspec --version $versionNumber
	choco push --api-key=$apiKey
}
finally
{
	popd
}