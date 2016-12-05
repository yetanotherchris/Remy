$ErrorActionPreference = "Stop";

# http://onlinemd5.com/ to generate the SHA1
# TODO: generate SHA1 automatically
$packageName = "remy"
$toolsDir    = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$checkSum = "TODO"

$url = "github release url here"

Install-ChocolateyZipPackage "packageName" -url "$url" -unzipLocation "$toolsDir" -checksumType "sha1" -checksum "$checkSum"