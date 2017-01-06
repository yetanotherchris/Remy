param(
	[Parameter(Mandatory=$true)]
	[String]
	$apiKey,
	[Parameter(Mandatory=$true)]
	[String]
	$versionNumber
)

pushd src\Remy.Core
rm *.nupkg

# Replace the version number in the nuspec file
$pwd = pwd
$content = [IO.File]::ReadAllText("$pwd\Remy.Core.template.nuspec")
$content = $content.Replace("{VERSION}", $versionNumber)
[IO.File]::WriteAllText("$pwd\Remy.Core.nuspec", $content)
rm "$pwd\Remy.Core.template.nuspec"

nuget pack .\Remy.Core.csproj -properties Configuration=release
nuget push *.nupkg $apiKey -source https://www.nuget.org/api/v2/package
popd
