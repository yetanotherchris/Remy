$ErrorActionPreference = "Stop";

$packageName = "remy"
$toolsDir    = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$checkSum = "{CHECKSUM}"

$url = "https://github.com/yetanotherchris/Remy/releases/download/remy-v{VERSION}/Remy-{VERSION}.zip"

Install-ChocolateyZipPackage "packageName" -url "$url" -unzipLocation "$toolsDir" -checksumType "sha256" -checksum "$checkSum"